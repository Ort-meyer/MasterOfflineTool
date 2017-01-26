#pragma once
#include "NeuralNetwork.h"

/**
This network uses translation of players to
figure out whether the player is lost or not.
Position is represented with distance traveled
and angle between last displacement vector. 10 
displacements will be used as inputs (total of
20 inputs).*/
class PositionNetwork :
	public NeuralNetwork
{
public:
	PositionNetwork();
	~PositionNetwork();
	void CreateTrainingFile();



};

