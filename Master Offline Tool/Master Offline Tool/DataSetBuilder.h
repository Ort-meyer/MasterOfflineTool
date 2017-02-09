#pragma once
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <vector>
#include <string>
class NeuralNetworkFactory;
/**
Struct containing all the data to be fed to a neural network.
The DatasetBuiler will build these sets from the raw data it
reads.*/
struct DataSet
{	
    std::string name;
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
    std::vector<std::vector<DataSet>>* BuildDataSetFromFiles(const std::vector<std::string>& p_fileNames);

    std::string GetComboNameFromIndex(const int& p_index);
private:
    NeuralNetworkFactory* m_factory;

	/**
	Loads file from provided path and converts data into our own
	dataset file format*/
	std::vector<DataSet> ConvertRawdataToDataSets(std::string p_fullPath);

	std::vector<DataSet> MergeDataFiles(std::vector<std::vector<DataSet>> p_dataFiles);

    // Will keep track of what combo is what
    std::vector<std::string> m_comboIndexToString;

	std::string m_dataFilePath;
	std::string m_dataFileEnding;
};

