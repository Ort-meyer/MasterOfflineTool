#pragma once
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <vector>
class NeuralNetwork;
struct NetworkSettings;
class NeuralNetworkFactory
{
public:
    NeuralNetworkFactory();
    ~NeuralNetworkFactory();
    
    /**
    Sets up different settings the factory needs to know how many different combinations to try
    */
    void SetVariables(const int& p_maxNumberOfHiddenLayers, const int& p_maxNumberOfHiddenCellsPerLayer, const int& p_numberOfHiddenCellIncrement,
        const float& p_learningRateIncrement, const float& p_hiddenSteepnessIncrement, const float& p_outputSteepnessIncrement, const int& p_maxNetsInMemoryAtOneTime)
    {
        m_maxNumberOfHiddenLayers = p_maxNumberOfHiddenLayers;
        m_maxNumberOfHiddenCellsPerLayer = p_maxNumberOfHiddenCellsPerLayer;
        m_numberOfHiddenCellIncrement = p_numberOfHiddenCellIncrement;
        m_learningRateIncrement = p_learningRateIncrement;
        m_hiddenSteepnessIncrement = p_hiddenSteepnessIncrement;
        m_outputSteepnessIncrement = p_outputSteepnessIncrement;
        m_maxNetsInMemoryAtOneTime = p_maxNetsInMemoryAtOneTime;
    }
    /**
    Uses the data to form a lot of neural networks with different settings, train them and validate them.
    Validation along with other info about the network is printed to file
    */
    void CreateNewNeuralNetworkCombinationsFromData(FANN::training_data* p_trainingData);
private:
    // First in a series of creating all different combinations of neural nets
    void CreateHiddenLayerCombinations(NetworkSettings * p_netWorkSettings, int* p_hiddenCells, const int& p_numberOfLayers, const int& p_depth);
    
    // Second step to create all different combinations of neural nets
    void CreateLearningRateSteepnessAndDeterministicSpecificCombinations(NetworkSettings * p_netWorkSettings);

    // Third step to create all different combinations of neural nets, switches over functions
    void CreateFANNFunctionOutputSpecificCombinations(NetworkSettings* p_networkSettings);

    // fourth step to create all different combinations of neural nets, switches over functions
    void CreateFANNFunctionHiddenSpecificCombinations(NetworkSettings* p_networkSettings);

    // Last step in the chain, here we simply create and ave the network
    void CreateTheNetwork(NetworkSettings* p_netWorkSettings);

    std::vector<NeuralNetwork*> m_networks;
    int m_maxNumberOfHiddenLayers;
    int m_maxNumberOfHiddenCellsPerLayer;
    // States how much the hidden cell per layer will increase per loop run
    int m_numberOfHiddenCellIncrement;
    // States how fast the learningrate will increment, by how much, each loop. Lower numbers creates more combinations
    // learningrate goes from 0-1
    float m_learningRateIncrement;
    // States how fast the hiddenSteepness will increment, by how much, each loop. Lower numbers creates more combinations
    // hiddenSteepness goes from 0-1
    float m_hiddenSteepnessIncrement;
    // States how fast the outputSteepness will increment, by how much, each loop. Lower numbers creates more combinations
    // outputSteepness goes from 0-1
    float m_outputSteepnessIncrement;
    // This decides how many network combinations are set up before we run them and remove from memory
    // Note that combinations with higher hidden layer count takes more memory. This could possibly be changed to mbytes allowed
    // to keep it a bit better, that will however not work when using cascading.
    int m_maxNetsInMemoryAtOneTime;
};

