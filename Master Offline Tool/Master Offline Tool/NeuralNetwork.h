#pragma once
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <string>
#include <vector>

struct BestEpoch
{
    float bestMSE;
    int bestEpoch;
	// List of all MSEs as given by reports
	std::vector<float> mseList;
	// Difference between first and last value
	float difference;

	float top;
	float bottom;
};

// Should probably have a constructor, or at least default values
struct NetworkSettings
{
    int inputCells;
    int outputCells;
    int hiddenLayers;
    // array, should be the lenght of hiddenLayers
    int* hiddenCells;

    float learningRate;
    float steepnessOutput;
    float steepnessHidden;
    FANN::activation_function_enum functionHidden;
    FANN::activation_function_enum functionOutput;

    bool deterministicWeights;

    FANN::training_data* trainingData;
    FANN::training_data* validationData;
    /**
    Saves all the inparams to the structs variables, should be used for all but input- and output-cells
    Note that the pointer to hidden cells is set to the inparam, so dont delete it after this func
    */
    void SetVaryingVariables(const int& p_hiddenLayers, int* p_hiddenCells, const float& p_learningRate, const float& p_steepnessOutput,
        const float& p_steepnessHidden, const FANN::activation_function_enum& p_functionHidden,
        const FANN::activation_function_enum& p_functionOutput, const bool& p_deterministicWeights, const std::string p_netID = "noID")
    {
        hiddenLayers = p_hiddenLayers;
        hiddenCells = p_hiddenCells;
        learningRate = p_learningRate;
        steepnessOutput = p_steepnessOutput;
        steepnessHidden = p_steepnessHidden;
        functionHidden = p_functionHidden;
        functionOutput = p_functionOutput;
        deterministicWeights = p_deterministicWeights;
        idString = p_netID;
    }

    // The following variables is about performans
    /*
    Mean square error of this network
    Should NOT be set at creation. This is derived after having run the network through validation.
    It's just easier to have this struct contain the number. We're lazy that way*/
    float mse;

    /**
    String used to identify this network. This is meant to be used to differentiate networks from
    one another in a sensible way*/
    std::string idString;

    /**
    How high percentile that was correct when validating
    */
    float correctPercentile;

    /**
    Mean error
    */
    float meanError;

    /**
    how many epochs the network trained to reach the result
    */
    BestEpoch bestEpoch;

    /**
    If retraining was performed
    */
    bool didRetrain;

    /**
    If the reatraining gave beter validation results
    */
    bool retrainingWasGood;

    /**
    How many time the network guessed lost when it was found, as a %
    If this is low it might be fine with a low guessing accuracy since we never guess "wrong", 
    we only guess with to much uncertainty
    */
    float lostWrongGuesses;
};

namespace NetworkSorting
{
    static bool SortNetworkSettingsByPercentile(const NetworkSettings& i, const NetworkSettings& j)
    {
        return i.correctPercentile > j.correctPercentile;
    }
	static bool SortNetwroKSettingsByMSEDiff(const NetworkSettings& i, const NetworkSettings& j)
	{
		return i.bestEpoch.difference > j.bestEpoch.difference;
	}
}

class NeuralNetwork
{
public:
    NeuralNetwork();
    ~NeuralNetwork();
    /**
    Sets the neural network settings
    */
    void SetSettings(const NetworkSettings& p_settings);
    /**
    Sets up the neural network with appropriate parameters
    defined by the network settings. These settings will
    have to be specified for each network*/
    void SetupNetwork();
    /**
    Trains this neural network on the training data contained
    in the file specified by "training data file name". */
    void TrainOnData(const int& p_numberOfEpochs, const int& p_reportFrequency, const float& p_errorAccaptance);
    /**
    Validates a trained neural network by running it on data
    specified by "validation data file name"
     REMOVED*/
    void ValidateOnFile();
    /**
    Validates the network if any validation data is set
    */
    void ValidateNetwork();
    /**
    Trains and validates the data using a increasing amount of epochs, validating after each training session
    */
    void TrainAndValidateNetwork(const int& p_epochs, const int& p_reportRate, const float& p_errorAcceptance);

    /**
    Returns MSE of this particular network*/
    float GetMSE() { return m_net.get_MSE(); };

    /**
    Returns network settings of this particular network*/
    NetworkSettings GetNetworkSettings() { return m_networkSettings; };

    /**
    Saves the network to the filename specified*/
    void SaveNetworkToFile(const std::string& p_fileName);

protected:
    // HELPER FUNCTIONS
    void InitializeWeights();

    // Neural network
    FANN::neural_net m_net;
    // Filename for data used to training the network
    std::string m_trainingDataFilename;
    // Filename for data used to validated a trained network
    std::string m_validationDataFilename;
    // Settings for this particular network
    NetworkSettings m_networkSettings;
    // The best MSE during training
    BestEpoch m_bestEpoch;
    // How far from the best MSE we find excaptable
    float m_MSEExcaptableDifference;
    // The validation error the first training got, if retraining is needed
    float m_firstTrainError;
};

