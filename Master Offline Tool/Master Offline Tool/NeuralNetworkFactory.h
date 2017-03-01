#pragma once
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <vector>
#include <string>
#include "NeuralNetwork.h"
#include <thread>
#include <atomic>

struct ThreadedNetwork
{
    NeuralNetwork* net;
    std::thread thread;
};

class NeuralNetworkFactory
{
public:
    NeuralNetworkFactory();
    ~NeuralNetworkFactory();


    /**
    Sets up different settings the factory needs to know how many different combinations to try
    */
    void SetVariables(const int& p_maxNumberOfHiddenLayers, const int& p_maxNumberOfHiddenCellsPerLayer, const int& p_numberOfHiddenCellIncrement,
        const float& p_learningRateIncrement, const float& p_hiddenSteepnessIncrement, const float& p_outputSteepnessIncrement, const int& p_maxNetsInMemoryAtOneTime,
        const int& p_bestNetworks = 5)
    {
        m_maxNumberOfHiddenLayers = p_maxNumberOfHiddenLayers;
        m_maxNumberOfHiddenCellsPerLayer = p_maxNumberOfHiddenCellsPerLayer;
        m_numberOfHiddenCellIncrement = p_numberOfHiddenCellIncrement;
        m_learningRateIncrement = p_learningRateIncrement;
        m_hiddenSteepnessIncrement = p_hiddenSteepnessIncrement;
        m_outputSteepnessIncrement = p_outputSteepnessIncrement;
        m_maxNetsInMemoryAtOneTime = p_maxNetsInMemoryAtOneTime;
        m_numBestNetworks = p_bestNetworks;
        m_totalNrOfnetworks = 1;
        m_totalNrOfnetworks *= m_maxNumberOfHiddenLayers;
        m_totalNrOfnetworks *= m_maxNumberOfHiddenCellsPerLayer / m_numberOfHiddenCellIncrement;
        m_totalNrOfnetworks *= 1 / m_learningRateIncrement;
        m_totalNrOfnetworks *= 1 / m_hiddenSteepnessIncrement;
        m_totalNrOfnetworks *= 1 / m_outputSteepnessIncrement;
        m_totalNrOfnetworks *= 14 * 14; // For different activation functions. Maybe reduce?

    }
    /**
    Creates all the combinations of activation functions for the other given settings
    */
    void CreateNewNeuralNetworkActivationFunctionCombinationFromData(FANN::training_data * p_trainingData, const int & p_numberOfHiddenLayers, int * p_hiddenLayerCells, const float & p_learningRateSteepness, const float & p_steepnessOutput, const float & p_steepnessHidden, const bool & p_deteministicWeights, const int & p_numberOfEpochsToTrain, const int & p_reportRate, const float & p_accaptableError, FANN::training_data * p_validationData, const std::string & p_netIdString);
    
    /**
    Uses the data to form a lot of neural networks with different settings, train them and validate them.
    Validation along with other info about the network is printed to file
     The created networks will be validated by the specified p_validationdata. If this is not set validation will either use
     the validation data set by the call to function SetValidationData or skipp validation if validationdata is nullptr.
    */
    void CreateNewNeuralNetworkCombinationsFromData(FANN::training_data* p_trainingData, FANN::training_data* p_validationData = nullptr);

    /**
    Creates a specific neural network from the given settings and trains it on the training data
    If no hidden layers will be used the activation funciton for hidden layers is ignored
    The created networks will be validated by the specified p_validationdata. If this is not set validation will either use
    the validation data set by the call to function SetValidationData or skipp validation if validationdata is nullptr.
    */
    void CreateSpecificNeuralNetwork(FANN::training_data* p_trainingData, const int& p_numberOfHiddenLayers, int* p_hiddenLayerCells,
        const FANN::activation_function_enum& p_outputActivationFunction, const FANN::activation_function_enum& p_hiddenActivationFunction,
        const float& p_learningRateSteepness, const float& p_steepnessOutput, const float& p_steepnessHidden, const bool& p_deteministicWeights,
        const int& p_numberOfEpochsToTrain, const int& p_reportRate, const float& p_accaptableError, FANN::training_data* p_validationData = nullptr, const std::string& p_netIdString = "");

    void SetValidationData(FANN::training_data* p_validationData) { m_validationData = p_validationData; };

    void SetNumBestNetworks(const int& p_numBestNetworks) { m_numBestNetworks = p_numBestNetworks; };

    /**
    Sets the number of best networks we want to store. This is 5 by default*/
    void SetBestNetworkSize(const int& p_bestNetworks) { m_numBestNetworks = p_bestNetworks; };

    /**
    Returns a vector of the best networks that the factory has run since reset or startup*/
    std::vector<NetworkSettings> GetBestNetworks() const { return m_bestNetworks; };

    /**
    Clears the list of best vectors*/
    void ClearBestVectors() { m_bestNetworks.clear(); };

    /**
    Sets how many networks can be in memory at the same time before a thread joining is performed
    */
    void SetMaxNetworksInMemory(const int& p_max) { m_maxNetsInMemoryAtOneTime = p_max; };

    // Joins the networks and makes sure they are done
    void JoinNetworkThreads();

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

    // Creates a new net and launches it to train on the training data specified
    void LaunchNewNet(NetworkSettings* p_netWorkSettings, const int& p_epochs, const int& p_reportRate, const float& p_acceptedError);

    /**
    Checks m_bestNetworks if the new settings are any good and updates if necessary.
    If m_bestNetworks isn't full of networks (if m_bestNetworks.size() > m_numBestNetworks)
    the new network is simply added to the list*/
    void UpdateBestNetworks(NetworkSettings p_settings);

    std::vector<ThreadedNetwork*> m_networks;
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

    int m_networksTrainedWithCurrentData;
    // Variable to keep track of how many networks we need to run
    int m_totalNrOfnetworks;

    // How many networks we want to consider "best"
    int m_numBestNetworks;

    // How many epochs we want to train
    int m_epocsToTrain;
    // The reposrt rate
    int m_reportRate;
    // The error acceptance
    int m_errorAcceptance;
    // Will be used as validation data
    FANN::training_data* m_validationData;

    std::vector<NetworkSettings> m_bestNetworks;
};

