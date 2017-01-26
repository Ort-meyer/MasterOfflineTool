#include "PositionNetwork.h"
#include <fstream>
#include <GLM\glm.hpp>
using namespace std;
using namespace glm;

const int g_numSteps = 10;

PositionNetwork::PositionNetwork()
{
	m_trainingDataFilename = "PositionTraining.data";
	m_validationDataFilename = "PositionValidation.data";
	m_networkSettings.inputCells = g_numSteps * 2;
	m_networkSettings.outputCells = 1;
	m_networkSettings.hiddenCells = new int(10); // Yup, it's an array
	m_networkSettings.hiddenLayers = 1;

	m_networkSettings.learningRate = 1;
	m_networkSettings.steepnessOutput = 1;
	m_networkSettings.steepnessHidden = 1;
	m_networkSettings.functionOutput = FANN::SIGMOID_SYMMETRIC;
	m_networkSettings.functionHidden = FANN::SIGMOID_SYMMETRIC;

	m_networkSettings.randomSeed = true;

	CreateTrainingFile();
	NeuralNetwork::SetupNetwork();
	NeuralNetwork::TrainOnData(200, 20, 0.0001f);
}


PositionNetwork::~PositionNetwork()
{
}

void PositionNetwork::CreateTrainingFile()
{
	float min = -0.5;
	float max = 0.5f;
	int numTestCases = 1000;
	vec3 currentPosition = vec3(0, 0, 0);
	vec3 oldMovement = vec3(0, 0, 0);

	ofstream file;
	file.open(m_trainingDataFilename);

	file << numTestCases << " " << m_networkSettings.inputCells << " " << m_networkSettings.outputCells << endl;

	/**
	The idea is to build a path of displacements with every other
	value being the displacement distance, and every other being 
	the angle to the last vector*/
	for (size_t i = 0; i < numTestCases; i++)
	{
		// Decide if this set should be lost or not
		bool lost = rand() % 2;
		vec3 movement;
		// If we're lost, we want to set a constant movement directino
		if (!lost)
			for (size_t i = 0; i < 3; i++)
				movement[i] = min + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX) / (max - min));
		// Build up our path
		for (size_t i = 0; i < g_numSteps; i++)
		{
			// Create displacement
			for (size_t i = 0; i < 3; i++)
			{
				if (lost)
					movement[i] = min + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX) / (max - min));
			}
			// Calculate length of our movement
			float displacement = length(movement);
			// Calculate angle
			float angle = dot(oldMovement, movement);
			// Update old movement
			oldMovement = movement;
			// Update position
			currentPosition += movement;

			// Write our values to the file
			file << displacement << " " << angle << " ";
		}
		file << endl << lost << endl;
	}
	file << endl;
	file.close();
}