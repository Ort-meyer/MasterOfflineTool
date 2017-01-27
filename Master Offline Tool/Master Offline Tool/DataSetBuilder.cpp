#include "DataSetBuilder.h"
#include "NeuralNetworkFactory.h"
#include <fstream>
#include <iostream>
#include <iterator>
#include <sstream>
using namespace std;
DataSetBuilder::DataSetBuilder()
{
    m_factory = new NeuralNetworkFactory();
    m_factory->SetVariables(3, 3, 1,0.5f,0.5f,0.5f,10);
    // We let the network be responsible for deleting the training data after it is done
    FANN::training_data* data = new FANN::training_data();
    data->read_train_from_file("CalcTrainingData.data");
    m_factory->CreateNewNeuralNetworkCombinationsFromData(data);
	m_bytes = 0;
	m_kbytes = 0;
	// DEBUG stuff
	BuildDataSetFromFolder("DEBUGData");
	// End debug stuff
	m_factory = new NeuralNetworkFactory();
	m_factory->SetVariables(3, 5, 1);
	m_factory->testFunc();


}


DataSetBuilder::~DataSetBuilder()
{
}

void DataSetBuilder::BuildDataSetFromFolder(string p_directory)
{
	// Get the names of all our raw data files
	vector<string> t_rawDataFileNames = GetAllFileNames(p_directory);
	// Convert each file of raw data into multiple data sets
	vector<vector<DataSet>> t_rawDataFiles;
	for (size_t i = 0; i < t_rawDataFileNames.size(); i++)
	{
		t_rawDataFiles.push_back(ConvertRawdataToDataSets(t_rawDataFileNames.at(i)));
	}
	vector<vector<DataSet>> mergedData; // This is what we'll feed networks
	// Find which big data sets we want to merge
	// DEBUG
	MergeDataFiles(t_rawDataFiles);
	// END DEBUG
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
}

void DataSetBuilder::BuildDataSetFromFiles(std::vector<std::string> p_fileNames)
{

}

vector<DataSet> DataSetBuilder::ConvertRawdataToDataSets(std::string p_fileName)
{
	vector<DataSet> r_dataSets;
	// Read file into a stringstream
	string path = "../Debug/DEBUGData/";
	path += p_fileName;
	ifstream file(path); // Most certainly needs tweaking
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
			thisSet.values = (float*)malloc(sizeof(float) * theseValues.size());
			memcpy(thisSet.values, &theseValues.at(0), theseValues.size());
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
		m_kbytes++;
		cout << m_kbytes << endl;
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
		// Allocate memory for values
		//m_bytes += inputs * sizeof(float);
		//if (m_bytes > 1000)
		//{
		//	m_kbytes += (m_bytes/1000)+1;
		//	m_bytes = 0;
		//}
		float* big = (float*)malloc(inputs * sizeof(float)); // REALLY redudant with double memory, but won't work otherwise
		//finalSet.values = nullptr;
		finalSet.values = big;
		//finalSet.values = (float*)malloc(inputs * sizeof(float) * 2); // REALLY redudant with double memory, but won't work otherwise
		// Copy values
		for (size_t l = 0; l < theseSets.size(); l++)
		{
			int stride = 0;
			if (l > 0)
				stride += theseSets.at(l - 1).inputs * sizeof(float);
			memcpy(finalSet.values+stride, theseSets.at(l).values, theseSets.at(l).inputs*sizeof(float));
		}
		finalDataSets.push_back(finalSet);
		
	}
	return finalDataSets;
}
std::vector<std::string> DataSetBuilder::GetAllFileNames(std::string p_directory)
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
	fullPath += "/*.rawdata";
	vector<string> t_rawDataFileNames;

	WIN32_FIND_DATA FindFileData;
	HANDLE hFind = FindFirstFile(fullPath.c_str(), &FindFileData);
	if (hFind == INVALID_HANDLE_VALUE)
	{
		printf("FindFirstFile failed (%d)\n", GetLastError());
		return t_rawDataFileNames;
	}
	//FindNextFile(hFind, &FindFileData); // Do I need this?
	std::string t_string = FindFileData.cFileName;
	t_rawDataFileNames.push_back(t_string);
	while (FindNextFile(hFind, &FindFileData))
	{
		std::string t_string = FindFileData.cFileName;
		t_rawDataFileNames.push_back(t_string);
	}
	FindClose(hFind);
	return t_rawDataFileNames;
}
