#pragma once
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <string>

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

    /**
    Saves all the inparams to the structs variables, should be used for all but input- and output-cells
    Note that the pointer to hidden cells is set to the inparam, so dont delete it after this func
    */
    void SetVaryingVariables(const int& p_hiddenLayers, int* p_hiddenCells, const float& p_learningRate, const float& p_steepnessOutput,
        const float& p_steepnessHidden, const FANN::activation_function_enum& p_functionHidden, 
        const FANN::activation_function_enum& p_functionOutput, const bool& p_deterministicWeights)
    {
        hiddenLayers = p_hiddenLayers;
        hiddenCells = p_hiddenCells;
        learningRate = p_learningRate;
        steepnessOutput = p_steepnessOutput;
        steepnessHidden = p_steepnessHidden;
        functionHidden = p_functionHidden;
        functionOutput = p_functionOutput;
        deterministicWeights = p_deterministicWeights;
    }
};

class NeuralNetwork
{
public:
	NeuralNetwork();
	~NeuralNetwork();
    /**
    Sets the neural network settings
    */
    void SetSettings(const NetworkSettings& p_settings) { m_networkSettings = p_settings; };
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
	specified by "validation data file name"*/
	void ValidateOnFile();
	
protected:
	// Neural network
	FANN::neural_net m_net;
	// Data used by the neural network
	FANN::training_data m_data;
	// Filename for data used to training the network
	std::string m_trainingDataFilename;
	// Filename for data used to validated a trained network
	std::string m_validationDataFilename;
	// Settings for this particular network
	NetworkSettings m_networkSettings;
};

