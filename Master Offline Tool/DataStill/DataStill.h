#pragma once
#include <string>
#include <vector>
class DataStill
{
public:
	DataStill();
	~DataStill();

private:
	///////////////////Specific methods///////////////
	/**
	Transforms world positions into displacement vectors.
	Formatting is important. Must be as follows:
	Index
	Values[3] // MUST be three input vectors
	Output
	In other words, multiple rows of input will not work.
	Output will be structured in the same way, but with displacement
	vectors instead of world positions*/
	void FilterDisplacement(std::string p_rawDataFileName, std::string p_filteredFileName);
	

	//////////////////General methods////////////////
	/**
	Reads through all files specified and normalizes values within and outputs
	results into respective p_filtereFileNames name. Make sure the two parameters 
	are mapped. All values will be between -1 and 1
	WARNING! This assume that there is only one data row per file. CBA with scalability.*/
	void NormalizeValues(std::vector<std::string> p_rawDataFileNames, std::vector <std::string> p_filteredFileNames);
	
	/**
	Takes p_numToMerge data sets and put them onto the same line.
	Index will be that of the first of the data sets.*/
	void MergeDataOntoSameLine(std::string p_rawDataFileName, std::string p_filteredFileName, int p_numToMerge);

	/**
	General filter method to remove every p_increment:th point of data
	For instance, setting p_increment to 3 will remove every 3rd data point.
	p_rawDataFileName specifies input file, and p_filteredFileName specifies
	output file name*/
	void FilterAwayDataRemove(std::string p_rawDataFileName, std::string p_filteredFileName, int p_increment);

	/**
	General filter method to keep every p_increment:th point of data
	For instance, setting p_increment to 3 will keep every 3rd data point,
	every other data point will be removed
	p_rawDataFileName specifies input file, and p_filteredFileName specifies
	output file name*/
	void FilterAwayDataKeep(std::string p_rawDataFileName, std::string p_filteredFileName, int p_increment);

	/**
	General filter method that takes p_nuToAvrage numer of values and stores
	as the avrage of those values. 
	p_rawDataFileName specifies input file, and p_filteredFileName specifies
	output file name*/
	void FilterAvrage(std::string p_rawDataFileName, std::string p_filteredFileName, int p_numToAvrage);


	/////////////////////HELP METHODS/////////////////
	/**
	Help method used to avrage numbers based on what rows are sent in. This
	is meant to be called sending in the true and false vectors of lines read
	from the data files. Return value is a single string with all avraged rows*/
	std::string AvrageNumbers(const std::vector<std::string>& p_rows, int p_nrOfdataRowsPerEntry, int p_nrToAvrage);

	/**
	Help Method to MergeDataOntoSameLine. This puts p_numToMerge
	data values on the same row*/
	std::string MergeVectorDataOntoSameLine(const std::vector<std::string>& p_dataLines, int p_numToMerge, int nrOfRowsPerDataEntry);

	/**
	Splits a data set into two vectors, one with only true output and one with
	only false output. Create vectors before calling this method and send pointers
	as parameters*/
	void SplitDataIntoTrueAndFalseVectors(
		const std::vector<std::string>& p_inData, 
		std::vector<std::string>* p_trueVector, 
		std::vector<std::string>* p_falseVector, 
		int p_nrOfDataRowsPerEntry);
	/**
	Help method that returns the file specified as a vector of strings where
	each string is a line fron the read file.*/
	std::vector<std::string> ReadFileIntoLines(std::string p_fileName);

	/**
	Returns the full path to files we might want to open
	Parameter is directory path as given relative to program .exe*/
	std::string GetAbsoluteFilePath(std::string p_directory);


	// Stores full path to directory in which we find raw data
	std::string m_rawDataFilePath;
};

