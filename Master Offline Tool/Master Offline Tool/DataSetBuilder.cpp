#include "DataSetBuilder.h"
#include "NeuralNetworkFactory.h"


using namespace std;
DataSetBuilder::DataSetBuilder()
{
    m_factory = new NeuralNetworkFactory();
    m_factory->SetVariables(3, 3, 1,0.5f,0.5f,0.5f,10);
    // We let the network be responsible for deleting the training data after it is done
    FANN::training_data* data = new FANN::training_data();
    data->read_train_from_file("CalcTrainingData.data");
    m_factory->CreateNewNeuralNetworkCombinationsFromData(data);
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
	vector<string> t_rawDataFileNames = GetAllFileNames(p_directory);

}

void DataSetBuilder::BuildDataSetFromFiles(std::vector<std::string> p_fileNames)
{
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
		return;
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
