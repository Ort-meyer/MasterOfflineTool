#include "NeuralNetworkFactory.h"
#include "NeuralNetwork.h"
#include <ConfigHandler.h>
#include <FileHandler.h>
#include <sstream>

#include <iostream>
using namespace std;
NeuralNetworkFactory::NeuralNetworkFactory() : m_validationData(nullptr), m_deleteCompletedNetworks(true)
{
    ConfigHandler* ch = ConfigHandler::Get();
    m_maxNumberOfHiddenLayers = ch->m_maxNumberOfHiddenLayers;
    m_numberOfHiddenLayersStart = ch->m_numberOfHiddenLayersStart;
    m_maxNumberOfHiddenCellsPerLayer = ch->m_maxNumberOfHiddenCellsPerLayer;
    m_numberOfHiddenCellIncrement = ch->m_numberOfHiddenCellIncrement;
    m_learningRateIncrement = ch->m_learningRateIncrement;
    m_hiddenSteepnessIncrement = ch->m_hiddenSteepnessIncrement;
    m_outputSteepnessIncrement = ch->m_outputSteepnessIncrement;
    m_maxNetsInMemoryAtOneTime = ch->m_maxNetworkThreads;
    m_totalNrOfnetworks = 1;
    m_totalNrOfnetworks *= m_maxNumberOfHiddenLayers;
    m_totalNrOfnetworks *= m_maxNumberOfHiddenCellsPerLayer / m_numberOfHiddenCellIncrement;
    m_totalNrOfnetworks *= 1 / m_learningRateIncrement;
    m_totalNrOfnetworks *= 1 / m_hiddenSteepnessIncrement;
    m_totalNrOfnetworks *= 1 / m_outputSteepnessIncrement;
}


NeuralNetworkFactory::~NeuralNetworkFactory()
{
    JoinNetworkThreads();
}

void NeuralNetworkFactory::CreateNewNeuralNetworkActivationFunctionCombinationFromData(FANN::training_data * p_trainingData, const int & p_numberOfHiddenLayers,
    int * p_hiddenLayerCells, const float & p_learningRateSteepness,
    const float & p_steepnessOutput, const float & p_steepnessHidden, const bool & p_deteministicWeights,
    const int& p_numberOfEpochsToTrain, const int& p_reportRate, const float& p_accaptableError, FANN::training_data* p_validationData,
    const std::string& p_netIdString)
{
    NetworkSettings newNetSettings;
    // Set the constant variables
    newNetSettings.inputCells = p_trainingData->num_input_train_data();
    newNetSettings.outputCells = p_trainingData->num_output_train_data();
    // This will be a leak...
    newNetSettings.trainingData = new FANN::training_data(*p_trainingData);
    newNetSettings.hiddenCells = p_hiddenLayerCells;
    newNetSettings.hiddenLayers = p_numberOfHiddenLayers;
    newNetSettings.learningRate = p_learningRateSteepness;
    newNetSettings.steepnessHidden = p_steepnessHidden;
    newNetSettings.steepnessOutput = p_steepnessOutput;
    newNetSettings.deterministicWeights = p_deteministicWeights;
    newNetSettings.idString = p_netIdString;
    m_epocsToTrain = p_numberOfEpochsToTrain;
    m_reportRate = p_reportRate;
    m_errorAcceptance = p_accaptableError;
    
    // Se if we have any validation data
    if (p_validationData == nullptr)
    {
        // If m_validationdata is nullptr no validation will take place
        if (m_validationData == nullptr)
        {
            newNetSettings.validationData = nullptr;
        }
        else
            newNetSettings.validationData = new FANN::training_data(*m_validationData);
    }
    else
    {
        newNetSettings.validationData = new FANN::training_data(*p_validationData);
    }
    CreateFANNFunctionOutputSpecificCombinations(&newNetSettings);
}

void NeuralNetworkFactory::CreateNewNeuralNetworkCombinationsFromData(FANN::training_data* p_trainingData, FANN::training_data* p_validationData)
{
    m_networksTrainedWithCurrentData = 0;
    NetworkSettings newNetSettings;
    // Set the constant variables
    newNetSettings.inputCells = p_trainingData->num_input_train_data();
    newNetSettings.outputCells = p_trainingData->num_output_train_data();
    newNetSettings.deterministicWeights = ConfigHandler::Get()->m_deterministicWeights;
    newNetSettings.trainingAlgorithm = FANN::training_algorithm_enum::TRAIN_RPROP;
    newNetSettings.trainingData = p_trainingData;
    // Se if we have any validation data
    if (p_validationData == nullptr)
    {
        // If m_validationdata is nullptr no validation will take place
        newNetSettings.validationData = m_validationData;
    }
    else
    {
        newNetSettings.validationData = p_validationData;
    }

    int whereToStart = m_numberOfHiddenLayersStart;
    // Unroll the first special case where we have no hidden layers
    if (m_numberOfHiddenLayersStart == 0)
    {
        newNetSettings.hiddenLayers = 0;
        CreateLearningRateSteepnessAndDeterministicSpecificCombinations(&newNetSettings);
        whereToStart = 1;
    }
    // Now we start looping over the different combinations of things, starting with 
    for (size_t hiddenLayers = whereToStart; hiddenLayers <= m_maxNumberOfHiddenLayers; hiddenLayers++)
    {
        // Save number of layers to the settingstruct
        newNetSettings.hiddenLayers = hiddenLayers;
        // We allocate memory for the hidden cells
        int* hiddenCells = (int*)malloc(hiddenLayers * sizeof(int));
        for (size_t i = 0; i < hiddenLayers; i++)
        {
            hiddenCells[i] = m_numberOfHiddenCellIncrement;
        }

        // Here we unroll one of the loops that is in the recursive method
        // This is done because we want to use the hidden cell count without it having been incremented once

        newNetSettings.hiddenLayers = hiddenLayers;
        newNetSettings.hiddenCells = hiddenCells;
        // With this combo we create even more combos from other settings
        CreateLearningRateSteepnessAndDeterministicSpecificCombinations(&newNetSettings);

        CreateHiddenLayerCombinations(&newNetSettings, hiddenCells, hiddenLayers, 0);
        std::cout << std::endl;
    }
}

void NeuralNetworkFactory::CreateSpecificNeuralNetwork(const NetworkSettings& p_setting)
{
    NetworkSettings newNetSettings;
    newNetSettings = p_setting;
    LaunchNewNet(&newNetSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
    return;
}


void NeuralNetworkFactory::CreateAndRunNetworksFromBaseline(NetworkSettings p_baseline, FANNSettingToTest p_whatToTest)
{
    NetworkSettings t_currentSettings = p_baseline;
    vector<string> t_networks;
    LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);

    if (((int)p_whatToTest & (int)FANNSettingToTest::TestHiddenCells) == (int)FANNSettingToTest::TestHiddenCells)
    {
        // Hidden cells
        int t_maxHiddenCells = 100;
        int t_hiddenCellsIncrement = 10;
        for (size_t i = t_hiddenCellsIncrement; i <= t_maxHiddenCells; i += t_hiddenCellsIncrement)
        {
            for (size_t j = 0; j < t_currentSettings.hiddenLayers; j++)
            {
                t_currentSettings.hiddenCells[j] = i;
            }
            LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
            //SetupAndTrainNetworkAndAddResultsToList(&t_networks, t_currentSettings);
        }
        JoinNetworkThreads();
    }  

    // Hidden layers
    if (((int)p_whatToTest & (int)FANNSettingToTest::TestHiddenLayers) == (int)FANNSettingToTest::TestHiddenLayers)
    {
        // Potential memory leak?
        t_currentSettings = p_baseline;
        int t_maxLayers = 5;
        for (size_t i = 0; i < t_maxLayers; i++)
        {
            t_currentSettings.hiddenLayers = i;
            for (size_t j = 0; j < t_currentSettings.hiddenLayers; j++)
            {
                if (p_baseline.hiddenLayers > 0)
                {
                    t_currentSettings.hiddenCells[j] = p_baseline.hiddenCells[0];
                }
                else
                {
                    t_currentSettings.hiddenCells[j] = 10; // This shouldn't happen though. Ever
                }
            }
            LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
            //SetupAndTrainNetworkAndAddResultsToList(&t_networks, t_currentSettings);
        }
        JoinNetworkThreads();
    }

    // Learning rate
    if (((int)p_whatToTest & (int)FANNSettingToTest::TestLearningRate) == (int)FANNSettingToTest::TestLearningRate)
    {
        t_currentSettings = p_baseline;
        float t_learningRateIncrement = 0.1;
        for (float i = 0; i <= 1; i += t_learningRateIncrement)
        {
            t_currentSettings.learningRate = i;
            LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
            // SetupAndTrainNetworkAndAddResultsToList(&t_networks, t_currentSettings);
        }
        JoinNetworkThreads();
    }

    // Steepness hidden
    if (((int)p_whatToTest & (int)FANNSettingToTest::TestHiddenSteepness) == (int)FANNSettingToTest::TestHiddenSteepness)
    {
        t_currentSettings = p_baseline;
        float t_steepnessHiddenIncrement = 0.1;
        for (float i = 0; i <= 1; i += t_steepnessHiddenIncrement)
        {
            t_currentSettings.steepnessHidden = i;
            if (t_currentSettings.steepnessHidden == p_baseline.steepnessHidden)
            {
                continue;
            }
            LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
            //SetupAndTrainNetworkAndAddResultsToList(&t_networks, t_currentSettings);
        }
        JoinNetworkThreads();
    }

    // Steepness output
    if (((int)p_whatToTest & (int)FANNSettingToTest::TestOutputSteepness) == (int)FANNSettingToTest::TestOutputSteepness)
    {
        t_currentSettings = p_baseline;
        float t_steepnessOutputIncrement = 0.1;
        for (float i = 0; i <= 1; i += t_steepnessOutputIncrement)
        {
            t_currentSettings.steepnessOutput = i;
            if (t_currentSettings.steepnessOutput == p_baseline.steepnessOutput)
            {
                continue;
            }
            LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
            // SetupAndTrainNetworkAndAddResultsToList(&t_networks, t_currentSettings);
        }
        JoinNetworkThreads();
    }

    //  Hidden Function
    if (((int)p_whatToTest & (int)FANNSettingToTest::TestHiddenLayerFunction) == (int)FANNSettingToTest::TestHiddenLayerFunction)
    {
        t_currentSettings = p_baseline;
        // Loop over all different functions, COS_SYMMETRIC is the last in the enum
        for (size_t i = 0; i < FANN::activation_function_enum::COS_SYMMETRIC + 1; i++)
        {
            if (i == FANN::activation_function_enum::THRESHOLD || i == FANN::activation_function_enum::THRESHOLD_SYMMETRIC
                || m_hiddenActivationFunctionsToUse.count((FANN::activation_function_enum)i) == 0)
            {
                continue;
            }
            t_currentSettings.functionHidden = static_cast<FANN::activation_function_enum>(i);
            if (t_currentSettings.functionHidden == p_baseline.functionHidden)
            {
                continue;
            }
            LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
        }
        JoinNetworkThreads();
    }

    //  output Function
    if (((int)p_whatToTest & (int)FANNSettingToTest::TestOutputLayerFunction) == (int)FANNSettingToTest::TestOutputLayerFunction)
    {
        t_currentSettings = p_baseline;
        // Loop over all different functions, COS_SYMMETRIC is the last in the enum
        for (size_t i = 0; i < FANN::activation_function_enum::COS_SYMMETRIC + 1; i++)
        {
            if (i == FANN::activation_function_enum::THRESHOLD || i == FANN::activation_function_enum::THRESHOLD_SYMMETRIC
                || m_outputActivationFunctionsToUse.count((FANN::activation_function_enum)i) == 0)
            {
                continue;
            }
            t_currentSettings.functionOutput = static_cast<FANN::activation_function_enum>(i);
            if (t_currentSettings.functionOutput == p_baseline.functionOutput)
            {
                continue;
            }
            LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
        }
        JoinNetworkThreads();
    }

    //  training algorithm
    if (((int)p_whatToTest & (int)FANNSettingToTest::TestTrainingAlgorithm) == (int)FANNSettingToTest::TestTrainingAlgorithm)
    {
        t_currentSettings = p_baseline;
        // Loop over all different functions, TRAIN_SARPROP is the last in the enum
        for (size_t i = 0; i < FANN::training_algorithm_enum::TRAIN_SARPROP + 1; i++)
        {
            t_currentSettings.trainingAlgorithm = static_cast<FANN::training_algorithm_enum>(i);
            if (t_currentSettings.trainingAlgorithm == p_baseline.trainingAlgorithm)
            {
                continue;
            }
            LaunchNewNet(&t_currentSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
        }
        JoinNetworkThreads();
    }
}

void NeuralNetworkFactory::SetupAndTrainNetworkAndAddResultsToList(std::vector<std::string>* p_netResults, const NetworkSettings& p_netSettings)
{
    NeuralNetwork t_net;
    t_net.SetSettings(p_netSettings);
    t_net.SetupNetwork();
    t_net.TrainAndValidateNetwork(m_epocsToTrain, m_reportRate, m_errorAcceptance);
    p_netResults->push_back(FileHandler::SaveNetworkToString(t_net.GetNetworkSettings()));
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
            p_hiddenCells[p_depth] = m_numberOfHiddenCellIncrement; // Start at increment, might want a start at variable
            return;
        }

        p_hiddenCells[p_depth] += m_numberOfHiddenCellIncrement;

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
    for (float learningRate = m_learningRateIncrement; learningRate <= 1; learningRate += m_learningRateIncrement)
    {
        p_netWorkSettings->learningRate = learningRate;
        for (float hiddenSteepness = m_hiddenSteepnessIncrement; hiddenSteepness <= 1; hiddenSteepness += m_hiddenSteepnessIncrement)
        {
            p_netWorkSettings->steepnessHidden = hiddenSteepness;
            for (float outputSteepness = m_outputSteepnessIncrement; outputSteepness <= 1; outputSteepness += m_outputSteepnessIncrement)
            {
                p_netWorkSettings->steepnessOutput = outputSteepness;
                CreateFANNFunctionOutputSpecificCombinations(p_netWorkSettings);

            }
        }
        if (p_netWorkSettings->trainingAlgorithm == FANN::training_algorithm_enum::TRAIN_RPROP)
        {
            break; // No need to continue, it does not use learning rate...
        }
    }

}

void NeuralNetworkFactory::CreateFANNFunctionOutputSpecificCombinations(NetworkSettings * p_networkSettings)
{
    // Loop over all different functions, COS_SYMMETRIC is the last in the enum
    for (size_t i = 0; i < FANN::activation_function_enum::COS_SYMMETRIC + 1; i++)
    {
        if (i == FANN::activation_function_enum::THRESHOLD || i == FANN::activation_function_enum::THRESHOLD_SYMMETRIC
            || m_outputActivationFunctionsToUse.count((FANN::activation_function_enum)i) == 0)
        {
            continue;
        }
        p_networkSettings->functionOutput = static_cast<FANN::activation_function_enum>(i);
        CreateFANNFunctionHiddenSpecificCombinations(p_networkSettings);
    }
}

void NeuralNetworkFactory::CreateFANNFunctionHiddenSpecificCombinations(NetworkSettings * p_networkSettings)
{
    // Loop over all different functions, COS_SYMMETRIC is the last in the enum
    // The threshold once cant be used during training so for now we skipp them completely (Should maybe use them during validation?)
    for (size_t i = 0; i < FANN::activation_function_enum::COS_SYMMETRIC + 1; i++)
    {
        if (i == FANN::activation_function_enum::THRESHOLD || i == FANN::activation_function_enum::THRESHOLD_SYMMETRIC
            || m_hiddenActivationFunctionsToUse.count((FANN::activation_function_enum)i) == 0)
        {
            continue;
        }
        p_networkSettings->functionHidden = static_cast<FANN::activation_function_enum>(i);
        // This is the last stepp, now we create and save the network!
        CreateTheNetwork(p_networkSettings);
    }
}

void NeuralNetworkFactory::CreateTheNetwork(NetworkSettings * p_netWorkSettings)
{
    // We create a new net and save it after passing the accuiered setting
    LaunchNewNet(p_netWorkSettings, m_epocsToTrain, m_reportRate, m_errorAcceptance);
}

void NeuralNetworkFactory::LaunchNewNet(NetworkSettings * p_netWorkSettings, const int & p_epochs, const int & p_reportRate, const float & p_acceptedError)
{
    ThreadedNetwork* newThreadedNetwork = new ThreadedNetwork();
    NeuralNetwork* newNet = new NeuralNetwork();
    newNet->SetSettings(*p_netWorkSettings);
    newNet->SetupNetwork();
    newThreadedNetwork->net = newNet;
    // There is still place in the active network list
    if (m_networks.size() >= m_maxNetsInMemoryAtOneTime)
    {
        JoinNetworkThreads();
    }
    newThreadedNetwork->thread = std::thread(&NeuralNetwork::TrainAndValidateNetwork, newNet, p_epochs, p_reportRate, p_acceptedError);
    m_networks.push_back(newThreadedNetwork);
}

void NeuralNetworkFactory::ClearBestVectors()
{
    size_t length = m_bestNetworks.size();
    for (size_t i = 0; i < length; i++)
    {
        free(m_bestNetworks.at(i).hiddenCells);
    }
    m_bestNetworks.clear();
}
#include "FileCombiner.h"
void NeuralNetworkFactory::JoinNetworkThreads()
{
    for (size_t i = 0; i < m_networks.size(); i++)
    {
        m_networks[i]->thread.join();
        UpdateBestNetworks(m_networks[i]->net->GetNetworkSettings());
        if (m_deleteCompletedNetworks)
        {
            delete m_networks[i]->net;
        }
        else
        {
            m_savedNetworks.push_back(m_networks[i]->net);
        }
        delete m_networks[i]; // This should not delete the thing that ->net is pointing to
    }
    m_networks.clear();   

    if (ConfigHandler::Get()->m_creationType == NetworkCreationType::CreateAllCombinations)
    {
        // If this is true we need to clear our vector from time to time... Otherwise we risk running out of memory
        FileCombiner::SaveNetsOfSameSettingToFile(*this, "PosRot", "../SavedNetSettings/PosRot");
        ClearBestVectors();
    }
}

void NeuralNetworkFactory::SetDeleteCompletedNetworks(bool p_delete)
{
    m_deleteCompletedNetworks = p_delete;
    if (p_delete)
    {
        size_t length = m_savedNetworks.size();
        for (size_t i = 0; i < length; i++)
        {
            delete m_savedNetworks[i];
        }
        m_savedNetworks.clear();
    }
}

void NeuralNetworkFactory::UseTheseOutputActivationFunctions(const std::vector<FANN::activation_function_enum>& p_functions)
{
    m_totalNrOfnetworks /= max(m_outputActivationFunctionsToUse.size(), 1);
    size_t length = p_functions.size();
    for (size_t i = 0; i < length; i++)
    {
        m_outputActivationFunctionsToUse.insert(p_functions[i]);
    }
    m_totalNrOfnetworks *= max(m_outputActivationFunctionsToUse.size(), 1);
}

void NeuralNetworkFactory::UseTheseHiddenActivationFunctions(const std::vector<FANN::activation_function_enum>& p_functions)
{
    m_totalNrOfnetworks /= max(m_hiddenActivationFunctionsToUse.size(), 1);
    size_t length = p_functions.size();
    for (size_t i = 0; i < length; i++)
    {
        m_hiddenActivationFunctionsToUse.insert(p_functions[i]);
    }
    m_totalNrOfnetworks *= max(m_hiddenActivationFunctionsToUse.size(), 1);
}

void NeuralNetworkFactory::UpdateBestNetworks(NetworkSettings p_settings)
{
    // Fill up list of best networks if not already full
    if (m_bestNetworks.size() < m_numBestNetworks)
        m_bestNetworks.push_back(p_settings);
    else
    {
        // Find the worst mse of the best networks
        float t_worst = m_bestNetworks.at(0).mse;
        int t_index = 0;
        for (size_t i = 0; i < m_bestNetworks.size(); i++)
        {
            if (m_bestNetworks.at(i).mse > t_worst)
            {
                t_index = i;
                t_worst = m_bestNetworks.at(i).mse;
            }
        }
        // Check if new network is better than worst
        if (p_settings.mse < t_worst)
            m_bestNetworks.at(t_index) = p_settings;
    }
}

void NeuralNetworkFactory::SaveBestNetworksToString(std::vector<std::string>& o_savedNetVector)
{
    for (size_t i = 0; i < m_bestNetworks.size(); i++)
    {
        o_savedNetVector.push_back(FileHandler::SaveNetworkToString(m_bestNetworks[i]));
    }
}
