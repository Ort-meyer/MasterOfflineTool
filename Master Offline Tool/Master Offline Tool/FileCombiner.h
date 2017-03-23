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
private:
    void SaveBestNetToFile(const NeuralNetworkFactory& p_factory, const std::string& p_fileName, const std::string& p_folder = "../SavedNetSettings/");
    void SaveNetsOfSameSettingToFile(const NeuralNetworkFactory & p_factory, const std::string & p_filePrefixName, const std::string & p_folder);
    void CombineFilesInFolder(const std::string& p_folderName, const std::string& p_fileEnding);
    void FeedDataToNeuralNetworkFactory();
    // This function takes the net settings and creates cross validation sets from them.
    void PerformCrossValidationOnNetSetting(NetworkSettings & p_netSetting, const int& p_totalNumberOfPersons, const int& p_combo);
    // This function also reduces the size of the given vector to remove files already drawn out
    std::vector<std::string> GetAllFilesWithStampAndShrinkList(const std::string& p_stamp, std::vector<std::string>& o_files);
    // This function gives back all training data that a number of combos creates
    FANN::training_data CreateTrainingDataFromListOfDataSet(std::vector<DataSet>& p_allCombosOfData);
    DataSetBuilder* m_dataSetBuilder;
    int m_validationAmount; // Sets how many of the file combos that should be used as validation set
    int m_stampSize;
    int m_dataSetCombinationsPerPerson;

    NetworkCreationType m_creationType;
    NeuralNetworkFactory m_factory;

    // Read explanation in DataSetBuilder::BuildDataSetFromFiles. Outer vector here is for all different people
    std::vector<std::vector<std::vector<DataSet>>*> m_allCombosOfData;

};


