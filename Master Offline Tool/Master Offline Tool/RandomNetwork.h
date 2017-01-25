#pragma once
#include "NeuralNetwork.h"
class RandomNetwork: public NeuralNetwork
{
public:
	RandomNetwork();
	~RandomNetwork();
	void CreateTrainingFile();
	
};

