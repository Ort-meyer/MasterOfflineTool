#include "RawDataWriter.h"
#include <fstream>
#include <GLM/glm.hpp>

using namespace glm;
using namespace std;

RawDataWriter::RawDataWriter()
{
	string savePath = "../Debug/DEBUGData/";
	m_dispOutputName = savePath+"DEBUGRawPositionData.rawdata";
	m_mouseOutputName = savePath+"DEBUGMouseData.rawdata";
	m_keyDownOutputName = savePath+"DEBUGKeyData.rawdata";
	m_samplesPerIntervall = 10;
	m_halfNumberOfIntervalls = 3;

	m_keyLost = m_halfNumberOfIntervalls;
	m_mouseLost = m_halfNumberOfIntervalls;
	m_dispLost = m_halfNumberOfIntervalls;
	WriteKeyDownData();
	WriteDisplacementData();
	WriteMeanMouseDisplacementData();
}


RawDataWriter::~RawDataWriter()
{
}

void RawDataWriter::WriteKeyDownData()
{
	// Index used to map the different data together
	int intervalIndex = 0;



	ofstream file;
	file.open(m_keyDownOutputName);
	for (size_t i = 0; i < m_halfNumberOfIntervalls; i++)
	{
		float numWPressed = 0;
		float numSpacePressed = 0;
		float numAPressed = 0;
		float numDPressed = 0;
		float numSPressed = 0;
		// Write intervall index at start of each intervall
		file << intervalIndex << endl;
		intervalIndex++;
		// Decide if this set should be lost or not
		bool lost = false;
		if (m_keyLost < m_halfNumberOfIntervalls)
		{
			lost = true;
			m_keyLost++;
		}
		for (size_t i = 0; i < m_samplesPerIntervall; i++)
		{
			// Some percentages to do stuff this frame
			int thisSeed = rand() % (100 + 1);
			if (lost)
			{
				// 60% chance of using W
				if (thisSeed > 40)
					numWPressed++;

				// 20% chance of using S
				if (thisSeed < 20)
					numSPressed++;

				// 30% chance of using A
				if (thisSeed > 70)
					numAPressed++;

				// 30% chance of using D
				if (thisSeed < 30)
					numDPressed++;

				// 30% chance of using space
				if (thisSeed < 30)
					numSpacePressed++;
			}
			else
			{
				// 90% chance of using W
				if (thisSeed > 10)
					numWPressed++;

				// 5% chance of using S
				if (thisSeed < 5)
					numSPressed++;

				// 15% chance of using A
				if (thisSeed > 85)
					numAPressed++;

				// 15% chance of using D
				if (thisSeed < 15)
					numDPressed++;

				// 20% chance of using space
				if (thisSeed < 30)
					numSpacePressed++;
			}
		}
		file << numWPressed << " " << numSPressed << " " << numSpacePressed << " " << numAPressed << " " << numDPressed << endl;
		file << lost << endl;
	}

}
vec2 RawDataWriter::CreateRandomMouseVector(float min, float max)
{
	vec2 movement;
	for (size_t i = 0; i < 2; i++)
		movement[i] = min + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX) / (max - min));
	return movement;
}

void RawDataWriter::WriteMeanMouseDisplacementData()
{
	// Index used to map the different data together
	int intervalIndex = 0;
	float min = -1;
	float max = 1;
	// How often a non-lost player moves his mouse as a denominator
	int notLostMoveDenom = 2;
	// How much less a player that is not lost moves his mouse
	float notLostMoveFactor = 0.3;

	ofstream file;
	file.open(m_mouseOutputName);
	for (size_t i = 0; i < m_halfNumberOfIntervalls; i++)
	{
		// Write intervall index at start of each intervall
		file << intervalIndex << endl;
		intervalIndex++;
		vec2 mouseMove = vec2(0, 0);
		// Decide if this set should be lost or not
		bool lost = false;
		if (m_mouseLost < m_halfNumberOfIntervalls)
		{
			lost = true;
			m_mouseLost++;
		}
		for (size_t i = 0; i < m_samplesPerIntervall; i++)
		{
			// Update move vector if we're nost
			if (lost)
				mouseMove = CreateRandomMouseVector(min, max);
			// Check if its time to move the mouse for a non-lost player
			else if (i%notLostMoveDenom)
				mouseMove = CreateRandomMouseVector(min*notLostMoveFactor, max*notLostMoveFactor);
			else
				mouseMove = vec2(0, 0);
			// Store this movement
			file << mouseMove.x << " " << mouseMove.y << " ";
		}
		file << endl << lost << endl;
	}
	file.close();
}

vec3 RawDataWriter::CreateRandomMoveVector(float min, float max)
{
	vec3 movement;
	for (size_t i = 0; i < 3; i++)
		movement[i] = min + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX) / (max - min));
	return movement;
}

void RawDataWriter::WriteDisplacementData()
{
	// Index used to map the different data together
	int intervalIndex = 0;
	float min = -1;
	float max = 1;

	ofstream file;
	file.open(m_dispOutputName);
	/**
	The idea is to build a path of displacements with every other
	value being the displacement distance, and every other being
	the angle to the last vector*/
	for (size_t i = 0; i < m_halfNumberOfIntervalls; i++)
	{
		// Write intervall index at start of each intervall
		file << intervalIndex << endl;
		intervalIndex++;
		// Decide if this set should be lost or not
		bool lost = false;
		if (m_dispLost < m_halfNumberOfIntervalls)
		{
			lost = true;
			m_dispLost++;
		}
		vec3 movement;
		// We want to start off having moved in a random direction
		movement = CreateRandomMoveVector(min, max);
		// Build up our path
		for (size_t i = 0; i < m_samplesPerIntervall; i++)
		{
			// If we're lost we always want a new movement direction
			if (lost)
			{
				movement = CreateRandomMoveVector(min, max);
			}
			// Write our values to the file
			file << movement.x << " " << movement.y << " " << movement.x << " ";
		}
		file << endl << lost << endl;
	}
	file.close();
}