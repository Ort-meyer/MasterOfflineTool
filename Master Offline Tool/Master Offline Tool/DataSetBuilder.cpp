#include "DataSetBuilder.h"
#include "NeuralNetworkFactory.h"
#include <FileHandler.h>
#include <fstream>
#include <iostream>
#include <iterator>
#include <sstream>
using namespace FANN;
using namespace std;
DataSetBuilder::DataSetBuilder()
{
	//// "Global" varialbes
	//m_dataFileEnding = "filteredData";
	//m_dataFilePath = "../FilteredData";

 //   // DEBUG stuff
 //   BuildDataSetFromFolder(m_dataFilePath);
 //   // End debug stuff

	//// Lots of trash we'll need to clean up
 //   m_factory = new NeuralNetworkFactory();
 //   m_factory->SetVariables(1, 20, 19,0.7f,1.0f,1.0f,10);
 //   // We let the network be responsible for deleting the training data after it is done
 //   FANN::training_data* data = new FANN::training_data();
 //   data->read_train_from_file("CalcTrainingData.data");
 //   //m_factory->CreateNewNeuralNetworkCombinationsFromData(data);

	//m_factory = new NeuralNetworkFactory();


}

DataSetBuilder::~DataSetBuilder()
{
}

void DataSetBuilder::BuildDataSetFromFolder(string p_directory)
{
	// Get the names of all our raw data files
	vector<string> t_rawDataFileNames = FileHandler::GetAllFileNames(p_directory, m_dataFileEnding);
	// Convert each file of raw data into multiple data sets
	vector<vector<DataSet>> t_rawDataFiles;
	for (size_t i = 0; i < t_rawDataFileNames.size(); i++)
	{
		t_rawDataFiles.push_back(ConvertRawdataToDataSets(t_rawDataFileNames.at(i)));
	}
	/*
	mergedData is a vector of vectors of data sets. Each "outer" vector is a
	kombination of the data set we want to merge. In other words, if we have
	three different types of input, mergedData.size() will always be 7, as that
	is how many combinations that exist.
	Each "inner" vector is the data sets for that particular kombination of 
	inputs. That is to say, each inner vector contains the actual values that
	the neural network will make use of.*/
	vector<vector<DataSet>> mergedData; // This is what we'll feed networks
	// Find which big data sets we want to merge
	int n = t_rawDataFiles.size();
	int cap = 1 << n;
	for (int i = 1; i < cap; ++i)
	{
		vector<vector<DataSet>> t_dataFilesToMerge;
		for (int j = 0; j < n; ++j)
		{
			if (i & (1 << j))
			{
				t_dataFilesToMerge.push_back(t_rawDataFiles.at(j));
				//cout << vec[j] << " ";
			}
		}
		mergedData.push_back(MergeDataFiles(t_dataFilesToMerge));
	}

	// Now to actually make it work
	int dataSetSize = mergedData.at(0).size();
	
	// Run through all merged data and feed to factory
	for (size_t i = 0; i < mergedData.size(); i++)
	{
		
		// Build mergedData into something useful
		float** inputs;
		inputs = (float**)malloc(sizeof(float*) * dataSetSize);
		float** outputs;
		outputs = (float**)malloc(sizeof(float*) * dataSetSize);

		int numInputs = mergedData.at(i).at(0).inputs;
		// Take this combination and put all inputs and outputs into appropriate arrays
		for (size_t j = 0; j < dataSetSize; j++)
		{
			inputs[j] = (float*)malloc(sizeof(float) * numInputs);
			inputs[j] = &mergedData.at(i).at(j).values[0];
			outputs[j] = (float*)malloc(sizeof(float));
			outputs[j] = &mergedData.at(i).at(j).output;
		}


		training_data data;       
        data.set_train_data(dataSetSize, mergedData.at(i).at(0).inputs, inputs, 1, outputs); 

		NeuralNetworkFactory netFac;
        //netFac.CreateSpecificNeuralNetwork(&data, )
        int* myInt = new int(20);
		//netFac.SetVariables(1, 3, 1, 0.3, 0.3, 0.3, 1);
        netFac.CreateSpecificNeuralNetwork(&data, 1, myInt, FANN::activation_function_enum::SIGMOID_SYMMETRIC, FANN::activation_function_enum::SIGMOID_SYMMETRIC,
            0.7f, 1.0f, 1.0f, true, 10000, 1000, 0.0001f,&data);
		//netFac.CreateNewNeuralNetworkCombinationsFromData(&data);
        delete myInt;
	}


}

vector<vector<DataSet>>* DataSetBuilder::BuildDataSetFromFiles(const std::vector<std::string>& p_fileNames)
{
    // Convert each file of raw data into multiple data sets
    vector<vector<DataSet>> t_filteredDataFiles;
    for (size_t i = 0; i < p_fileNames.size(); i++)
    {
        t_filteredDataFiles.push_back(ConvertRawdataToDataSets(p_fileNames.at(i)));
    }
    /*
    mergedData is a vector of vectors of data sets. Each "outer" vector is a
    kombination of the data set we want to merge. In other words, if we have
    three different types of input, mergedData.size() will always be 7, as that
    is how many combinations that exist.
    Each "inner" vector is the data sets for that particular kombination of
    inputs. That is to say, each inner vector contains the actual values that
    the neural network will make use of.*/
    // Outer vector contains combination while inner vector contains data
    vector<vector<DataSet>>* mergedData = new vector<vector<DataSet>>(); // This is what we'll feed networks
                                        // Find which big data sets we want to merge
    int n = p_fileNames.size();
    int cap = 1 << n;
    for (int i = 1; i < cap; ++i)
    {
        vector<vector<DataSet>> t_dataFilesToMerge;
        for (int j = 0; j < n; ++j)
        {
            if (i & (1 << j))
            {
                t_dataFilesToMerge.push_back(t_filteredDataFiles.at(j));
                //cout << vec[j] << " ";
            }
        }
        mergedData->push_back(MergeDataFiles(t_dataFilesToMerge));
    }
    return mergedData;
}

vector<DataSet> DataSetBuilder::ConvertRawdataToDataSets(std::string p_fileName)
{
	vector<DataSet> r_dataSets;
	ifstream file(p_fileName); // Most certainly needs tweaking
	if (file)
	{
		stringstream buffer;
		buffer << file.rdbuf();
		file.close();

		for (string line; getline(buffer, line); )
		{
			istringstream in(line);
			// Read index
			int index;
			in >> index;

			// Read values
			vector<float> theseValues;
			getline(buffer, line);
			istringstream in1(line);
			// Counts number of inputs by getting number of spaces. Or something
			int inputs = distance(istream_iterator<string>(istringstream(line) >> ws), istream_iterator<string>());
			for (size_t i = 0; i < inputs; i++)
			{
				float thisValue;
				in1 >> thisValue;
				theseValues.push_back(thisValue);
			}

			// Read output
			getline(buffer, line);
			istringstream in2(line);
			int output;
			in2 >> output;

			// Create the data set
			DataSet thisSet;
			thisSet.index = index;
			thisSet.inputs = theseValues.size();
			thisSet.values = theseValues; // Yup, let's copy!
			//thisSet.values = (float*)malloc(sizeof(float) * theseValues.size());
			//memcpy(thisSet.values, &theseValues.at(0), theseValues.size());
			thisSet.output = output;
			r_dataSets.push_back(thisSet);
		}


	}
	return r_dataSets;
}
std::vector<DataSet> DataSetBuilder::MergeDataFiles(vector<vector<DataSet>> p_dataFiles)
{

	vector<DataSet> finalDataSets;
	// Iterate through first data file
	for (size_t i = 0; i < p_dataFiles.at(0).size(); i++)
	{
		vector<DataSet> theseSets;
		// Find a data set
		DataSet thisSet = p_dataFiles.at(0).at(i);
		theseSets.push_back(thisSet);
		int thisIndex = thisSet.index;
		// Find the data sets in all other data files
		for (size_t j = 1; j < p_dataFiles.size(); j++)
		{
			// Find the data set with the right index from this file
			for (size_t k = 0; k < p_dataFiles.at(j).size(); k++)
			{
				if (p_dataFiles.at(j).at(k).index == thisIndex)
				{
					theseSets.push_back(p_dataFiles.at(j).at(k));
					break;
				}
			}
		}
		// We've found the sets to merge. Lego merge
		DataSet finalSet;
		// Copy index and output
		finalSet.index = theseSets.at(0).index;
		finalSet.output = theseSets.at(0).output;
		// Add together inputs
		int inputs = 0;
		for (size_t l = 0; l < theseSets.size(); l++)
		{
			inputs += theseSets.at(l).inputs;
		}
		finalSet.inputs = inputs;
		// Copy over data sets into final data set
		for (size_t l = 0; l < theseSets.size(); l++)
		{
			finalSet.values.insert(finalSet.values.end(), theseSets.at(l).values.begin(), theseSets.at(l).values.end());
		}
		finalDataSets.push_back(finalSet);
		
	}
	return finalDataSets;
}

