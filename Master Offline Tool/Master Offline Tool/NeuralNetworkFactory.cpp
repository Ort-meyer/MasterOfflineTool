#include "NeuralNetworkFactory.h"
#include "NeuralNetwork.h"

#include <iostream>
NeuralNetworkFactory::NeuralNetworkFactory()
{
}


NeuralNetworkFactory::~NeuralNetworkFactory()
{
}

void NeuralNetworkFactory::CreateNewNeuralNetworkCombinationsFromData(FANN::training_data* p_trainingData)
{
    NetworkSettings newNetSettings;
    // Set the constant variables
    newNetSettings.inputCells = p_trainingData->num_input_train_data();
    newNetSettings.outputCells = p_trainingData->num_output_train_data();
    newNetSettings.trainingData = p_trainingData;

    // Unroll the first special case where we have no hidden layers
    newNetSettings.hiddenLayers = 0;
    CreateLearningRateSteepnessAndDeterministicSpecificCombinations(&newNetSettings);
    // Now we start looping over the different combinations of things, starting with 
    for (size_t hiddenLayers = 1; hiddenLayers <= m_maxNumberOfHiddenLayers; hiddenLayers++)
    {
        // Save number of layers to the settingstruct
        newNetSettings.hiddenLayers = hiddenLayers;
        // We allocate memory for the hidden cells
        int* hiddenCells = (int*)malloc(hiddenLayers * sizeof(int));
        for (size_t i = 0; i < hiddenLayers; i++)
        {
            hiddenCells[i] = 1;
        }

        // Here we unroll one of the loops that is in the recursive method
        // This is done because we want to use the hidden cell count without it having been incremented once
        for (size_t i = 0; i < hiddenLayers; i++)
        {
            std::cout << hiddenCells[i];
        }
        std::cout << std::endl;

        newNetSettings.hiddenLayers = hiddenLayers;
        newNetSettings.hiddenCells = hiddenCells;
        // With this combo we create even more combos from other settings
        CreateLearningRateSteepnessAndDeterministicSpecificCombinations(&newNetSettings);

        CreateHiddenLayerCombinations(&newNetSettings, hiddenCells, hiddenLayers, 0);
        std::cout << std::endl;
    }
}

void NeuralNetworkFactory::CreateHiddenLayerCombinations(NetworkSettings * p_netWorkSettings, int* p_hiddenCells, const int& p_numberOfLayers, const int& p_depth)
{
    while (true)
    {
        if (p_depth + 1 != p_numberOfLayers)
        {
            CreateHiddenLayerCombinations(p_netWorkSettings, p_hiddenCells, p_numberOfLayers, p_depth + 1);
        }
        if (p_hiddenCells[p_depth] >= m_maxNumberOfHiddenCellsPerLayer)
        {
            p_hiddenCells[p_depth] = 1; // Might need to be 1, dont know if FANN accepts 0 as in
            return;
        }
 
        p_hiddenCells[p_depth] += m_numberOfHiddenCellIncrement;
        for (size_t i = 0; i < p_numberOfLayers; i++)
        {
            std::cout << p_hiddenCells[i];
        }
        std::cout << std::endl;
        // Now we put in the info just created into the info and pass it to the next chain in the functions 
        // to create more combinations that builds on this combo
        p_netWorkSettings->hiddenLayers = p_numberOfLayers;
        p_netWorkSettings->hiddenCells = p_hiddenCells;
        // With this combo we create even more combos from other settings
        CreateLearningRateSteepnessAndDeterministicSpecificCombinations(p_netWorkSettings);
    }
}

void NeuralNetworkFactory::CreateLearningRateSteepnessAndDeterministicSpecificCombinations(NetworkSettings * p_netWorkSettings)
{
    for (float learningRate = 0; learningRate <= 1; learningRate += m_learningRateIncrement)
    {
        p_netWorkSettings->learningRate = learningRate;
        for (float hiddenSteepness = 0; hiddenSteepness <= 1; hiddenSteepness += m_hiddenSteepnessIncrement)
        {
            p_netWorkSettings->steepnessHidden = hiddenSteepness;
            for (float outputSteepness = 0; outputSteepness <= 1; outputSteepness+=m_outputSteepnessIncrement)
            {
                p_netWorkSettings->steepnessOutput = outputSteepness;
                for (size_t i = 0; i < 2; i++)
                {
                    // This should give 0/1 = 0 and 1/1 = 1 aka false and true
                    p_netWorkSettings->deterministicWeights = i/2;
                    CreateFANNFunctionOutputSpecificCombinations(p_netWorkSettings);
                }
            }
        }
    }
    
}

void NeuralNetworkFactory::CreateFANNFunctionOutputSpecificCombinations(NetworkSettings * p_networkSettings)
{
    // Loop over all different functions, COS_SYMMETRIC is the last in the enum
    for (size_t i = 0; i < FANN::activation_function_enum::COS_SYMMETRIC + 1; i++)
    {
        p_networkSettings->functionOutput = static_cast<FANN::activation_function_enum>(i);
        CreateFANNFunctionHiddenSpecificCombinations(p_networkSettings);
    }
}

void NeuralNetworkFactory::CreateFANNFunctionHiddenSpecificCombinations(NetworkSettings * p_networkSettings)
{
    // Loop over all different functions, COS_SYMMETRIC is the last in the enum
    for (size_t i = 0; i < FANN::activation_function_enum::COS_SYMMETRIC + 1; i++)
    {
        p_networkSettings->functionHidden = static_cast<FANN::activation_function_enum>(i);
        // This is the last stepp, now we create and save the network!
        CreateTheNetwork(p_networkSettings);
    }
}

void NeuralNetworkFactory::CreateTheNetwork(NetworkSettings * p_netWorkSettings)
{
    // We create a new net and save it after passing the accuiered setting
    NeuralNetwork* newNet = new NeuralNetwork();
    newNet->SetSettings(*p_netWorkSettings);
    newNet->SetupNetwork();
    m_networks.push_back(newNet);
    size_t size = m_networks.size();
    if (size > m_maxNetsInMemoryAtOneTime)
    {
        // A inverted for loop for easy removes while we dont have it threaded
        for (int i = size - 1; i >= 0; i--)
        {
            // TODO Train and validate the network, use threads here if needed.
            // Note that after training and validation is done the network will delete itself
            m_networks.at(i)->TrainAndValidateNetwork();
            // TODO Join threads

            // Remove the networks
            delete m_networks.at(i);
            m_networks.erase(m_networks.begin() + i);
        }
    }
}
