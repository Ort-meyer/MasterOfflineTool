#include "DataStill.h"
#include <fstream>
#include <iostream>
#include <iterator>
#include <sstream>
#include <Windows.h>
using namespace std;
DataStill::DataStill()
{
	m_rawDataFilePath = GetAbsoluteFilePath("DEBUGData");
	FilterAwayDataRemove("DEBUGKeyData.rawdata", "DEBUGKeyData.filtereddata", 100);
}


DataStill::~DataStill()
{
}

std::string DataStill::GetAbsoluteFilePath(std::string p_directory)
{
	string fullPath;
	// Get current working directory and add the specific directory we want after
	char buffer[MAX_PATH];
	GetModuleFileName(NULL, buffer, MAX_PATH);
	string::size_type pos = string(buffer).find_last_of("\\/");
	fullPath = string(buffer).substr(0, pos);
	// Add in directory and file ending of stuff we want
	fullPath += "\\";
	fullPath += p_directory;
	return fullPath;
}

void DataStill::FilterAwayDataRemove(std::string p_rawDataFileName, std::string p_filteredFileName, int p_increment)
{
	// Read all lines of file into vector of strings
	string inFilePath = m_rawDataFilePath;
	inFilePath += "/";
	inFilePath += p_rawDataFileName;
	vector<string> lines;
	ifstream inFile(inFilePath);
	string line;
	while (std::getline(inFile, line))
	{
		lines.push_back(line);
	}
	inFile.close();

	// Write to output file
	ofstream outFile;
	string outFilePath = m_rawDataFilePath;
	outFilePath += "/";
	outFilePath += p_filteredFileName;
	outFile.open(outFilePath);
	int nrOfRowsPerdataSet = 3; // Will have to be tweaked!
	int size = lines.size() / nrOfRowsPerdataSet;
	for (size_t i = 1; i < size+1; i++)
	{
		// Skip every p_increment:th write
		if (i%p_increment != 0)
		{
			int derp = 5;
			for (size_t j = 0; j < nrOfRowsPerdataSet; j++)
			{
				outFile << lines.at((i-1)*nrOfRowsPerdataSet + j) << endl;
			}
		}

	}
	outFile.close();

}

void DataStill::FilterAwayDataKeep(std::string p_rawDataFileName, std::string p_filteredFileName, int p_increment)
{

}