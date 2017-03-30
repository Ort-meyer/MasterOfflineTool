#pragma once
#include <vector>
#include <string>
#include "NeuralNetworkFactory.h"
class DataSetBuilder;
namespace FANN
{
    class training_data;
}

enum class NetworkCreationType;

struct DataSet;
struct NetworkSettings;
class FileCombiner
{
public:
    FileCombiner();
    ~FileCombiner();
    static void SaveNetsOfSameSettingToFile(const NeuralNetworkFactory & p_factory, const std::string & p_filePrefixName, const std::string & p_folder);
private:
    void SaveBestNetToFile(const NeuralNetworkFactory& p_factory, const std::string& p_fileName, const std::string& p_folder = "../SavedNetSettings/");
    void CombineFilesInFolder(const std::string& p_folderName, const std::string& p_fileEnding);
    void FeedDataToNeuralNetworkFactory();
    // This function takes the net settings and creates cross validation sets from them.
    void PerformCrossValidationOnNetSetting(NetworkSettings & p_netSetting, const int& p_totalNumberOfPersons, const int& p_combo);
    // This function also reduces the size of the given vector to remove files already drawn out
    std::vector<std::string> GetAllFilesWithStampAndShrinkList(const std::string& p_stamp, std::vector<std::string>& o_files);
    // This function gives back all training data that a number of combos creates
    FANN::training_data CreateTrainingDataFromListOfDataSet(std::vector<DataSet>& p_allCombosOfData);
    // Saves all the saved net with approach to their ID
    void SaveNetworkToFile();
    DataSetBuilder* m_dataSetBuilder;
    int m_validationAmount; // Sets how many of the file combos that should be used as validation set
    int m_stampSize;
    int m_dataSetCombinationsPerPerson;

    NetworkCreationType m_creationType;
    NeuralNetworkFactory m_factory;

    // Read explanation in DataSetBuilder::BuildDataSetFromFiles. Outer vector here is for all different people
    std::vector<std::vector<std::vector<DataSet>>*> m_allCombosOfData;

    // For getting person from validation data. int in vector is person, string is file names. Not very suitable for combos...
    // Size of second vector should be 4
    std::vector<std::vector<std::string>> m_filesPerPerson;

};


