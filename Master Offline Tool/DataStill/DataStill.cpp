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
	FilterAvrage("DEBUGKeyData.rawdata", "DEBUGKeyData.filtereddata", 10);
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
	vector<string> lines = ReadFileIntoLines(p_rawDataFileName);

	// Write to output file
	ofstream outFile;
	string outFilePath = m_rawDataFilePath;
	outFilePath += "/";
	outFilePath += p_filteredFileName;
	outFile.open(outFilePath);
	int nrOfRowsPerdataSet = 3; // Will have to be tweaked!
	int size = lines.size() / nrOfRowsPerdataSet;
	for (size_t i = 1; i < size + 1; i++)
	{
		// Skip every p_increment:th write
		if (i%p_increment != 0)
		{
			int derp = 5;
			for (size_t j = 0; j < nrOfRowsPerdataSet; j++)
			{
				outFile << lines.at((i - 1)*nrOfRowsPerdataSet + j) << endl;
			}
		}

	}
	outFile.close();

}

void DataStill::FilterAwayDataKeep(std::string p_rawDataFileName, std::string p_filteredFileName, int p_increment)
{

	vector<string> lines = ReadFileIntoLines(p_rawDataFileName);

	// Write to output file
	ofstream outFile;
	string outFilePath = m_rawDataFilePath;
	outFilePath += "/";
	outFilePath += p_filteredFileName;
	outFile.open(outFilePath);
	int nrOfRowsPerdataSet = 3; // Will have to be tweaked!
	int size = lines.size() / nrOfRowsPerdataSet;
	for (size_t i = 1; i < size + 1; i++)
	{
		// Skip every p_increment:th write
		if (i%p_increment == 0)
		{
			int derp = 5;
			for (size_t j = 0; j < nrOfRowsPerdataSet; j++)
			{
				outFile << lines.at((i - 1)*nrOfRowsPerdataSet + j) << endl;
			}
		}

	}
	outFile.close();
}

void DataStill::FilterAvrage(std::string p_rawDataFileName, std::string p_filteredFileName, int p_numToAvrage)
{
	vector<string> lines = ReadFileIntoLines(p_rawDataFileName);

	int nrOfRowsPerDataEntry = 3; // Will have to be tweaked!
	int strideToData = 1; //How many rows are skipped before data is read, for each data point
	int nrOfDataPoints = lines.size() / nrOfRowsPerDataEntry;
	// Read into two vectors: one with true, one with false. Makes things easier
	vector<string> trueList;
	vector<string> falseList;
	for (size_t i = 0; i < nrOfDataPoints; i++)
	{
		// This is haxy. It relies on the output for a raw data set to be the last row in a data entry
		if (!strcmp(lines.at((i + 1)*nrOfRowsPerDataEntry - 1).c_str(), "0"))
		{
			// Write the rows for this data entry
			for (size_t j = 0; j < nrOfRowsPerDataEntry; j++)
			{
				falseList.push_back(lines.at(i*nrOfRowsPerDataEntry + j));
			}
		}
		else
		{
			for (size_t j = 0; j < nrOfRowsPerDataEntry; j++)
			{
				trueList.push_back(lines.at(i*nrOfRowsPerDataEntry + j));
			}
		}
	}
	// Use our amazing help method
	string trueAvrages = AvrageNumbers(trueList, nrOfRowsPerDataEntry, p_numToAvrage);
	string falseAvrages = AvrageNumbers(falseList, nrOfRowsPerDataEntry, p_numToAvrage);


}

std::string DataStill::AvrageNumbers(const std::vector<string>& p_rows, int p_nrOfdataRowsPerEntry, int p_nrToAvrage)
{
	string r_avrageLines = "";
	int nrOfValueRows = p_nrOfdataRowsPerEntry - 2; // Hard-coded. Index is one row, output is one row
	// Figure out how many inputs each data row stores
	vector<int> nrOfInputs;
	// We use only the first data entry, since all data entries have to be foramtted the same
	for (size_t i = 0; i < nrOfValueRows; i++)
	{
		int inputs = distance(istream_iterator<string>(istringstream(p_rows.at(i + 1)) >> ws), istream_iterator<string>());
		nrOfInputs.push_back(inputs);
	}

	// Now we avrage the values in each list
	for (size_t i = 0; i < p_rows.size() / p_nrToAvrage; i++)
	{
		// Per batch of data entries
		/*
		Inner vector: contains values of one row
		Middle vector: contains all rows with values within one data entry
		Outer vector: contains all data entries that will be avraged*/
		string index;
		string output;
		bool readIndexAndOutput = false;
		vector<vector<vector<float>>> allDataEntries;
		for (size_t j = 0; j < p_nrToAvrage; j++)
		{
			vector<vector<float>> dataEntriesOfthisSet;
			for (size_t k = 0; k < nrOfValueRows; k++)
			{
				// Number of inputs for this row
				int inputs = nrOfInputs.at(k);
				vector<float> valuesOfThisRow;
				int rowStartEntry = i*p_nrOfdataRowsPerEntry*p_nrToAvrage + j*p_nrOfdataRowsPerEntry + k;
				if (rowStartEntry >= p_rows.size())
				{
					break;
				}
				// Ugly way of doing this, but it works
				if (!readIndexAndOutput)
				{
					index = p_rows.at(rowStartEntry);
					output = p_rows.at(rowStartEntry + p_nrOfdataRowsPerEntry - 1);
					readIndexAndOutput = true;
				}
				istringstream in(p_rows.at(rowStartEntry + 1));
				for (size_t l = 0; l < inputs; l++)
				{
					float thisValue;
					in >> thisValue;
					valuesOfThisRow.push_back(thisValue);
				}
				dataEntriesOfthisSet.push_back(valuesOfThisRow);
			}
			if (dataEntriesOfthisSet.size()> 0)
				allDataEntries.push_back(dataEntriesOfthisSet);
		}
		// Now we avrage the numbers
		/**
		Each inner vector is one row of values.
		Each outer vector contains one data set*/
		vector<vector<float>> avrages;

		// Make room for our values
		avrages.resize(nrOfInputs.size());
		for (size_t j = 0; j < nrOfInputs.size(); j++)
		{
			avrages.at(j).resize(nrOfInputs.at(j));
		}

		// Get totals
		for (size_t j = 0; j < allDataEntries.size(); j++)
		{
			for (size_t k = 0; k < allDataEntries.at(j).size(); k++)
			{
				for (size_t l = 0; l < allDataEntries.at(j).at(k).size(); l++)
				{
					avrages.at(k).at(l) += allDataEntries.at(j).at(k).at(l);
				}
			}
		}
		// Write results to return string
		r_avrageLines += index;
		r_avrageLines += "\n";
		// Now actually avrage
		for (size_t j = 0; j < avrages.size(); j++)
		{
			for (size_t k = 0; k < avrages.at(j).size(); k++)
			{
				avrages.at(j).at(k) /= p_nrToAvrage;
				// Write value
				r_avrageLines += to_string(avrages.at(j).at(k));
				r_avrageLines += " ";
			}
			r_avrageLines += "\n";
		}
		r_avrageLines += output;
		r_avrageLines += "\n";
	}
	cout << r_avrageLines;
	return r_avrageLines;
}

std::vector<std::string> DataStill::ReadFileIntoLines(std::string p_fileName)
{
	// Read all lines of file into vector of strings
	string inFilePath = m_rawDataFilePath;
	inFilePath += "/";
	inFilePath += p_fileName;
	vector<string> lines;
	ifstream inFile(inFilePath);
	string line;
	while (std::getline(inFile, line))
	{
		lines.push_back(line);
	}
	inFile.close();
	return lines;
}