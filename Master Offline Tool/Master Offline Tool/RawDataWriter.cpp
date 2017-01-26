#include "RawDataWriter.h"
#include <fstream>
#include <GLM/glm.hpp>

using namespace glm;
using namespace std;

RawDataWriter::RawDataWriter()
{
	m_dispOutputName = "DEBUGRawPositionData.data";
	m_mouseOutputName = "DEBUGMouseData.data";
	m_samplesPerIntervall = 10;
	m_numberOfIntervalls = 1000;
}


RawDataWriter::~RawDataWriter()
{
}

void RawDataWriter::WriteKeyDownData()
{

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
	float min = -1;
	float max = 1;
	// How often a non-lost player moves his mouse as a denominator
	int notLostMoveDenom = 2;
	// How much less a player that is not lost moves his mouse
	float notLostMoveFactor = 0.3;

	ofstream file;
	file.open(m_mouseOutputName);
	for (size_t i = 0; i < m_numberOfIntervalls; i++)
	{
		vec2 mouseMove = vec2(0, 0);
		// Decide if this set should be lost or not
		bool lost = rand() % 2;
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
	float min = -1;
	float max = 1;

	ofstream file;
	file.open(m_dispOutputName);
	/**
	The idea is to build a path of displacements with every other
	value being the displacement distance, and every other being
	the angle to the last vector*/
	for (size_t i = 0; i < m_numberOfIntervalls; i++)
	{
		// Decide if this set should be lost or not
		bool lost = rand() % 2;
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