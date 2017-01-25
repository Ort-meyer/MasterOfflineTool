#include "RandomNetwork.h"


RandomNetwork::RandomNetwork()
{
	m_trainingDataFilename = "RandomTest.data";
	m_validationDataFilename = "RandomTest.data";
	m_networkSettings.inputCells = 10;
	m_networkSettings.outputCells = 1;
	m_networkSettings.learningRate = 0.7;
	m_networkSettings.steepnessOutput = 1;
	m_networkSettings.functionOutput = FANN::SIGMOID_SYMMETRIC_STEPWISE;
	m_networkSettings.hiddenLayers = 0;
	
}


RandomNetwork::~RandomNetwork()
{
}

