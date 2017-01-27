#include "RandomNetwork.h"
#include <fstream>
using namespace std;
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
	m_networkSettings.deterministicWeights = true;
	
	CreateTrainingFile();
	NeuralNetwork::SetupNetwork();
	NeuralNetwork::TrainOnData(200, 20, 0.0001f);
}


RandomNetwork::~RandomNetwork()
{
}

void RandomNetwork::CreateTrainingFile()
{
	int input = 10;
	int min = 0;
	int max = 1;
	int numTestCases = 1000;
	ofstream file;
	file.open("RandomTest.data");
	// Write test data header
	file << numTestCases << " " << input << " " << 1 << endl;
	// Create test data sets
	for (int i = 0; i < numTestCases; i++)
	{
		// Determine is this particular set will be random
		bool isRandom = rand() % 2;
		// value if this data set isn't random
		int thisValue = rand() % (max - min + 1) + min;

		string thisLine;
		// create this data set
		for (int i = 0; i < input; i++)
		{
			if (isRandom)
			{
				// Was random. Create new number each iteration
				thisLine += to_string((long double)(rand() % (max - min + 1) + min));
				thisLine += " ";
			}
			else
			{
				// Wasn't random. Use same number
				thisLine += to_string((long double)(thisValue));
				thisLine += " ";
			}
		}
		file << thisLine << endl;
		// Write output
		file << isRandom << endl;

	}
	file.close();
}

