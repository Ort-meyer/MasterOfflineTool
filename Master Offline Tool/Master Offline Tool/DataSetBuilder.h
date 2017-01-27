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
	int output;
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
	Help method that returns a string of all file names in the given
	directory. WARNING! Only looks for .rawdata format right now*/
	std::vector<std::string> GetAllFileNames(std::string p_directory);
	/**
	Loads file from provided filename and converts data into our own
	dataset file format*/
	std::vector<DataSet> ConvertRawdataToDataSets(std::string p_fileName);

	std::vector<DataSet> MergeDataFiles(std::vector<std::vector<DataSet>> p_dataFiles);

	int m_bytes;
	int m_kbytes;
};

