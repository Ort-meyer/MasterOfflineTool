#include "NeuralNetworkFactory.h"
#include "NeuralNetwork.h"


NeuralNetworkFactory::NeuralNetworkFactory()
{
}


NeuralNetworkFactory::~NeuralNetworkFactory()
{
}

void NeuralNetworkFactory::CreateNewNeuralNetworkCombinationsFromData(FANN::training_data p_trainingData)
{
    NeuralNetwork* newNet = new NeuralNetwork();
    NetworkSettings newNetSettings;
    // Set the constant variables
    newNetSettings.inputCells = p_trainingData.num_input_train_data();
    newNetSettings.outputCells = p_trainingData.num_output_train_data();
    
    CreateNeuralNetworkCombinations(&newNetSettings,0);



    // After the setting is created and done we pass it to the net
    newNet->SetSettings(newNetSettings);
    newNet->SetupNetwork();

}

void NeuralNetworkFactory::testFunc()
{
    // Loop over and set the varying variables to all kinds of combinations
    // number of hidden layers
    for (size_t hiddenLayers = 0; hiddenLayers < m_maxNumberOfHiddenLayers; hiddenLayers++)
    {
        int* hiddenCells = (int*)malloc(hiddenLayers * sizeof(int));

        for (size_t j = 0; j < (m_maxNumberOfHiddenCellsPerLayer / m_numberOfHiddenCellIncrease) * hiddenLayers; j++)
        {
            // Reached max in one lane, increase the next with 1
            for (size_t k = hiddenLayers; k > 0; k--)
            {
                if (j % (m_maxNumberOfHiddenCellsPerLayer*k) == 0)
                {
                    hiddenCells[k - 1] += m_numberOfHiddenCellIncrease;
                    for (size_t l = 0; l < hiddenLayers; l++)
                    {
                        if (l%k != 0)
                        {
                            hiddenCells[l] = 0;
                        }
                    }
                }

            }
            hiddenCells[0] += m_numberOfHiddenCellIncrease;
        }
    }
}

void NeuralNetworkFactory::CreateNeuralNetworkCombinations(NetworkSettings * p_netWorkSettings, const int& p_depth)
{
    if (p_depth != m_maxNumberOfHiddenLayers)
    {
        CreateNeuralNetworkCombinations(p_netWorkSettings, p_depth + 1);
    }
}
