#pragma once
#include <string>
#include <GLM\glm.hpp>
using namespace std;
using namespace glm;
class RawDataWriter
{
public:
	RawDataWriter();
	~RawDataWriter();


	/**
	some terminology:
	-	Intervall: the larger time scope in which analysis is performed
		This is probably going to be a certain amount of seconds, 15 or so.
	-	Sampling: this is how often certain data is sampled, such as
		position
	-	Frame: this refers to one update of the game from which the data is
		meant to be extracted*/

	/**
	Writes player displacement since last sample as three input cells (one
	per dimension of movement)*/
	void WriteDisplacementData();
	/**
	Calculates mean mouse displacement. This is done by
	taking the total mouse displacement in the last sample
	divided by the "maximum" total mouse displacement
	(to get a value between 0 and 1).
	The idea is for this data to be calculated at a set sample rate
	but here it's obviously just generated more or less randomly*/
	void WriteMeanMouseDisplacementData();
	/**
	This takes certain keys and takes the number of frames the key has
	been held down, divided by total number of frames in the intervall,
	and generates one input cell per relevant key*/
	void WriteKeyDownData();
	/**
	TODO implement APM? */

private:
	// The output file names
	string m_dispOutputName;
	string m_mouseOutputName;
	string m_keyDownOutputName;
	// Half how many intervalls we want. Half total intervalls are lost
	int m_halfNumberOfIntervalls;
	// How long each intervall is. I.e. how many samples per intervall
	int m_samplesPerIntervall;

	// Iterators keeping track of number of lost intervalls
	int m_dispLost;
	int m_mouseLost;
	int m_keyLost;
	
	// Help method for WriteMeanMouseDisplacementData();
	vec2 CreateRandomMouseVector(float min, float max);
	// Help method for WriteDisplacementData
	vec3 CreateRandomMoveVector(float min, float max);
};

