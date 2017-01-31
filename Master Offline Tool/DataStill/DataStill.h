#pragma once
#include <string>
#include <vector>
class DataStill
{
public:
	DataStill();
	~DataStill();

private:
	/**
	Returns the full path to files we might want to open
	Parameter is directory path as given relative to program .exe*/
	std::string GetAbsoluteFilePath(std::string p_directory);

	/**
	Filters player's movements from p_rawDataFileName and stores in 
	a new file as specified by p_filteredFilename*/
	void FilterDisplacement(std::string p_rawDataFileName, std::string p_filteredFileName);

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

	// Stores full path to directory in which we find raw data
	std::string m_rawDataFilePath;
};

