#pragma once
#include <vector>
#include <string>
class NeuralNetworkFactory;
/**
Struct containing all the data to be fed to a neural network.
The DatasetBuiler will build these sets from the raw data it
reads.*/
struct DataSet
{	
	int index;
	int inputs;
	std::vector<float> values;
	float output;
};

class DataSetBuilder
{
public:
    DataSetBuilder();
    ~DataSetBuilder();

	/**
	Reads through a directory and generates combined data sets
	of all combinations of raw data sets*/
	void BuildDataSetFromFolder(std::string p_directory);
    void BuildDataSetFromFiles(std::vector<std::string> p_fileNames);
private:
    NeuralNetworkFactory* m_factory;

	/**
	Method that returns vector of paths to all files within the specified
	directory. Directory is only local directory, */
	std::vector<std::string> GetAllFileNames(std::string p_directory, std::string p_fileEnding);

	/**
	Returns the full path to files we might want to open
	Parameter is directory path as given relative to program .exe*/
	std::string GetAbsoluteFilePath(std::string p_directory);
	/**
	Loads file from provided path and converts data into our own
	dataset file format*/
	std::vector<DataSet> ConvertRawdataToDataSets(std::string p_fullPath);

	std::vector<DataSet> MergeDataFiles(std::vector<std::vector<DataSet>> p_dataFiles);

	std::string m_dataFilePath;
	std::string m_dataFileEnding;

	int m_bytes;
	int m_kbytes;
};

