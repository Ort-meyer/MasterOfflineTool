#include "RawDataWriter.h"
#include <fstream>
#include <GLM/glm.hpp>

using namespace glm;
using namespace std;

RawDataWriter::RawDataWriter()
{
	m_dispOutputName = "DEBUGRawPositionData.data";
	m_samplesPerIntervall = 10;
	m_numberOfIntervalls = 1000;
}


RawDataWriter::~RawDataWriter()
{
}

void RawDataWriter::WriteKeyDownData()
{

}

void RawDataWriter::WriteMeanMouseDisplacementData()
{

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
	float min = -0.5;
	float max = 0.5f;

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
		file << endl;
	}
	file.close();
}