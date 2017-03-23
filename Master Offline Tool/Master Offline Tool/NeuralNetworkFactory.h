#pragma once
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <vector>
#include <string>
#include "NeuralNetwork.h"
#include <thread>
#include <atomic>
#include <unordered_set>

struct ThreadedNetwork
{
    NeuralNetwork* net;
    std::thread thread;
};

enum class FANNSettingToTest;

class NeuralNetworkFactory
{
public:
    NeuralNetworkFactory();
    ~NeuralNetworkFactory();
    /**
    Sets up training info
    */
    void SetTrainingVariables(const int& p_numberOfEpochs, const int& p_reportRate, const float& p_errorAcceptance)
    {
        m_epocsToTrain = p_numberOfEpochs;
        m_reportRate = p_reportRate;
        m_errorAcceptance = p_errorAcceptance;
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

    To later get access to the network created the SetDeleteCompletedNetworks(bool) needs to be set to false. If this is done
    the networks can all be found by calling GetNetworksInMemory().
    */
    void CreateSpecificNeuralNetwork(const NetworkSettings& p_setting);



    void SetValidationData(FANN::training_data* p_validationData) { m_validationData = p_validationData; };

    void SetNumBestNetworks(const int& p_numBestNetworks) { m_numBestNetworks = p_numBestNetworks; };


    /**
    Runs a set of networks based off of the provided baseline. One parameter is altered at a time and one network per
    increment of that parameter is created. Results are written to the folder of its type*/
    void CreateAndRunNetworksFromBaseline(NetworkSettings p_baseline, FANNSettingToTest p_whatToTest);

    /**
    Help method for baseline method*/
    void SetupAndTrainNetworkAndAddResultsToList(std::vector<std::string>* p_netResults, const NetworkSettings & p_netSettings);

    /**
    Sets the number of best networks we want to store. This is 5 by default*/
    void SetBestNetworkSize(const int& p_bestNetworks) { m_numBestNetworks = p_bestNetworks; };

    /**
    Returns a vector of the best networks that the factory has run since reset or startup*/
    std::vector<NetworkSettings> GetBestNetworks() const { return m_bestNetworks; };

    /**
    Clears the list of best vectors*/
    void ClearBestVectors();

    /**
    Sets how many networks can be in memory at the same time before a thread joining is performed
    */
    void SetMaxNetworksInMemory(const int& p_max) { m_maxNetsInMemoryAtOneTime = p_max; };

    // Joins the networks and makes sure they are done
    void JoinNetworkThreads();

    // Sets the delete networks variable. If called with true, all previously saved nets will be deleted
    void SetDeleteCompletedNetworks(bool p_delete);

    // States which output activation functions we want to use
    void UseTheseOutputActivationFunctions(const std::vector<FANN::activation_function_enum>& p_functions);

    // State which hidden activation functions we want to use
    void UseTheseHiddenActivationFunctions(const std::vector<FANN::activation_function_enum>& p_functions);

    // Cleares the hidden and output activation functions
    void ClearActivationFunctionsToUse() { m_hiddenActivationFunctionsToUse.clear(); m_outputActivationFunctionsToUse.clear(); }
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

    void SaveBestNetworksToString(std::vector<std::string>& o_savedNetVector);

    std::vector<ThreadedNetwork*> m_networks;
    std::vector<NeuralNetwork*> m_savedNetworks;
    int m_maxNumberOfHiddenLayers;
    int m_numberOfHiddenLayersStart;
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
    float m_errorAcceptance;
    // Will be used as validation data
    FANN::training_data* m_validationData;
    // States if completed networks will be removed or not (this does not have an impact to best networks)
    bool m_deleteCompletedNetworks;

    std::vector<NetworkSettings> m_bestNetworks;
    std::unordered_set<FANN::activation_function_enum> m_hiddenActivationFunctionsToUse;
    std::unordered_set<FANN::activation_function_enum> m_outputActivationFunctionsToUse;
    
};

