#include "FileCombiner.h"
#include "DataSetBuilder.h"
#include "NeuralNetwork.h"
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <FileHandler.h>
#include <algorithm>
#include <iostream>
#include <sstream>
#include "GnuPlotter.h"
#include <ConfigHandler.h>
static FileCombiner m_this;

FileCombiner::FileCombiner() : m_dataSetBuilder(new DataSetBuilder())
{
    std::string t_stampLayout = "YYYY-MM-DD - hh-mm-ss";
    m_stampSize = t_stampLayout.length();

    ConfigHandler* config = ConfigHandler::Get();

    // This is the last thing we check before we send the setting to the network factory
    m_creationType = config->m_creationType;

    m_factory.SetTrainingVariables(config->m_numberOfEpochs, config->m_reportRate, config->m_errorAcceptance);
    m_factory.SetMaxNetworksInMemory(config->m_maxNetworkThreads);
    m_factory.SetDeleteCompletedNetworks(true);

    std::vector<FANN::activation_function_enum> functions;
    functions.push_back(FANN::activation_function_enum::ELLIOT_SYMMETRIC);
    //functions.push_back(FANN::activation_function_enum::SIGMOID_SYMMETRIC);
    //functions.push_back(FANN::activation_function_enum::COS_SYMMETRIC);
    //functions.push_back(FANN::activation_function_enum::SIN_SYMMETRIC);
    //functions.push_back(FANN::activation_function_enum::GAUSSIAN_SYMMETRIC);
    //functions.push_back(FANN::activation_function_enum::LINEAR_PIECE_SYMMETRIC);
    m_factory.UseTheseHiddenActivationFunctions(functions);

    functions.clear();
    functions.push_back(FANN::activation_function_enum::ELLIOT_SYMMETRIC);
    // functions.push_back(FANN::activation_function_enum::SIGMOID_SYMMETRIC);
    // functions.push_back(FANN::activation_function_enum::COS_SYMMETRIC);
    // functions.push_back(FANN::activation_function_enum::SIN_SYMMETRIC);
    // functions.push_back(FANN::activation_function_enum::GAUSSIAN_SYMMETRIC);
    // functions.push_back(FANN::activation_function_enum::LINEAR_PIECE_SYMMETRIC);
    // functions.push_back(FANN::activation_function_enum::ELLIOT);
    // functions.push_back(FANN::activation_function_enum::SIGMOID);
    // functions.push_back(FANN::activation_function_enum::GAUSSIAN);
    // functions.push_back(FANN::activation_function_enum::LINEAR_PIECE);
    m_factory.UseTheseOutputActivationFunctions(functions);

    m_validationAmount = config->m_numValidationSet;
    CombineFilesInFolder("../filteredData", "filteredData");
    FeedDataToNeuralNetworkFactory();
    
    int a;
    std::cin >> a;
}


FileCombiner::~FileCombiner()
{
    delete m_dataSetBuilder;
}

void FileCombiner::SaveBestNetToFile(const NeuralNetworkFactory& p_factory, const std::string & p_fileName, const std::string & p_folder)
{
    std::string t_folderFullPath = FileHandler::GetAbsoluteFilePath(p_folder);
    std::vector<NetworkSettings> netSettings = p_factory.GetBestNetworks();
    //std::sort(netSettings.begin(), netSettings.end(), NetworkSorting::SortNetworkSettingsByPercentile);
	std::sort(netSettings.begin(), netSettings.end(), NetworkSorting::SortNetwroKSettingsByMSEDiff);
    std::vector<std::string> t_lines;
    size_t length = netSettings.size();
    for (size_t net = 0; net < length; net++)
    {
        t_lines.push_back(FileHandler::SaveNetworkToString(netSettings[net]));        
    }
    FileHandler::WriteToFile(t_lines, t_folderFullPath + p_fileName);
}

void FileCombiner::SaveNetsOfSameSettingToFile(const NeuralNetworkFactory& p_factory, const std::string& p_filePrefixName, const std::string& p_folder)
{
    std::string t_folderFullPath = FileHandler::GetAbsoluteFilePath(p_folder);
    
    if (CreateDirectory(t_folderFullPath.c_str(), NULL) || ERROR_ALREADY_EXISTS == GetLastError())
    {
        // We are good, lets go!!!
        std::vector<NetworkSettings> netSettings = p_factory.GetBestNetworks();
        size_t length = netSettings.size();
        for (size_t i = 0; i < length; i++)
        {
            std::string functionFolder = std::to_string(netSettings[i].functionHidden) + std::to_string(netSettings[i].functionOutput);
            std::string folderToUse = t_folderFullPath + "/" + functionFolder + "/";
            if (CreateDirectory(folderToUse.c_str(), NULL) || ERROR_ALREADY_EXISTS == GetLastError())
            {
                std::ostringstream fileNameStream;
                fileNameStream << p_filePrefixName << " ";
                fileNameStream << netSettings[i].functionHidden << " ";
                fileNameStream << netSettings[i].functionOutput << " ";
                fileNameStream << netSettings[i].hiddenLayers << " ";
                for (size_t j = 0; j < netSettings[i].hiddenLayers; j++)
                {
                    fileNameStream << " " << netSettings[i].hiddenCells[j];
                }
                fileNameStream << " ";
                fileNameStream << netSettings[i].functionHidden;
                fileNameStream << " ";
                fileNameStream << netSettings[i].functionOutput;
                fileNameStream << " ";
                fileNameStream << netSettings[i].trainingAlgorithm;
                fileNameStream << " ";
                fileNameStream << netSettings[i].learningRate;
                fileNameStream << " ";
                fileNameStream << netSettings[i].steepnessHidden;
                fileNameStream << " ";
                fileNameStream << netSettings[i].steepnessOutput;
                fileNameStream << " ";
                fileNameStream << netSettings[i].deterministicWeights;
                std::string fileName = fileNameStream.str();
                std::vector<std::string> newFileEntry;
                newFileEntry.push_back(FileHandler::SaveNetworkToString(netSettings[i]));
                FileHandler::AppendToFile(newFileEntry, folderToUse + fileName);
            }
        }
    }
    else
    {
        DWORD errorcode = GetLastError();
        LPVOID lpMsgBuf;

        FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
            NULL, errorcode, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpMsgBuf, 0, NULL);

        MessageBox(NULL, (LPCTSTR)lpMsgBuf, TEXT("Error"), MB_OK);

    }
}

void FileCombiner::CombineFilesInFolder(const std::string& p_folderName, const std::string& p_fileEnding)
{
    std::vector<std::string> t_rawDataFileNames = FileHandler::GetAllFileNames(p_folderName, p_fileEnding);

    // WE know that the stamp is in the end of a file, just before the file ending
    for (size_t i = 0; i < t_rawDataFileNames.size(); )
    {
        int t_stampBegins = t_rawDataFileNames[i].length() - p_fileEnding.length() - 1 - m_stampSize;
        std::string t_stamp = t_rawDataFileNames[i].substr(t_stampBegins, m_stampSize);
        std::vector<std::string> t_filesInCombination = GetAllFilesWithStampAndShrinkList(t_stamp, t_rawDataFileNames);
        // Sort to make sure the files are always sent in in the same order
        std::sort(t_filesInCombination.begin(), t_filesInCombination.end());
        // If we specify a combination sequence the second vectors size will be 1
        m_allCombosOfData.push_back(m_dataSetBuilder->BuildDataSetFromFiles(t_filesInCombination, "PosRot"));
    }
    m_dataSetCombinationsPerPerson = m_allCombosOfData[0]->size();
}

void FileCombiner::FeedDataToNeuralNetworkFactory()
{
    int numberOfPersons = m_allCombosOfData.size();
    // The trainingdata attached to one combo
    std::vector<FANN::training_data> t_allTrainingData;

    size_t numberOfCombos = m_allCombosOfData[0]->size();
    for (size_t combo = 0; combo < numberOfCombos; combo++)
    {
        std::cout << m_dataSetBuilder->GetComboNameFromIndex(combo) << std::endl;
        // Set the net settings from config, if m_creationtype does not need a certain setting it will later be ignored
        NetworkSettings t_netSetting;

        t_netSetting.outputCells = 1;
        t_netSetting.hiddenLayers = 4;
        int hiddenlayers[5] = { 100,100,100, 100, 100 };
        t_netSetting.hiddenCells = hiddenlayers;
        t_netSetting.learningRate = 0.7;
        t_netSetting.steepnessHidden = 0.7;
        t_netSetting.steepnessOutput = 0.7;
        t_netSetting.functionHidden = FANN::activation_function_enum::ELLIOT_SYMMETRIC;
        t_netSetting.functionOutput = FANN::activation_function_enum::ELLIOT_SYMMETRIC;
        t_netSetting.trainingAlgorithm = FANN::training_algorithm_enum::TRAIN_RPROP;
        t_netSetting.deterministicWeights = ConfigHandler::Get()->m_deterministicWeights;

        PerformCrossValidationOnNetSetting(t_netSetting, numberOfPersons, combo);
        // When we get here we have completed one combination of inputs with all combinations of validation and training data between persons
        // Here we save the best net setting to file
        // Make sure all the nets are done
        m_factory.JoinNetworkThreads();

        // SaveBestNetToFile(t_factory, "bestFunctions.netSetting");
        SaveNetsOfSameSettingToFile(m_factory, m_dataSetBuilder->GetComboNameFromIndex(combo), "../SavedNetSettings/" + m_dataSetBuilder->GetComboNameFromIndex(combo));
        // SaveBestNetToFile(m_factory, m_dataSetBuilder->GetComboNameFromIndex(combo) + "." + ConfigHandler::Get()->m_fileEndingNetSettings);

        // Then we clear the best vector before we start the next combo
        m_factory.ClearBestVectors();
    }
}

void FileCombiner::PerformCrossValidationOnNetSetting(NetworkSettings & p_netSetting, const int& p_totalNumberOfPersons, const int& p_combo)
{
    for (size_t validationSet = 0; validationSet < p_totalNumberOfPersons; validationSet++)
    {
        // This is used to make the validationset wrap around the vector
        int low = p_totalNumberOfPersons - validationSet - m_validationAmount;
        int validationSetAmountOverSize = min(low, 0);
        validationSetAmountOverSize = abs(validationSetAmountOverSize);
        std::vector<DataSet> oneCombosTrainingData;
        std::vector<DataSet> oneCombosValidationData;
        // This loop is to add all persons (excpet the validaiton set) to the training data that will be created later
        for (size_t person = 0; person < p_totalNumberOfPersons; person++)
        {
            // If the current preson is inside the validation range
            if (person >= validationSet && person < validationSet + m_validationAmount || person < validationSetAmountOverSize)
            {
                oneCombosValidationData.insert(oneCombosValidationData.end(), m_allCombosOfData[person]->at(p_combo).begin(), m_allCombosOfData[person]->at(p_combo).end());
            }
            else
            {
                // This make it so oneCombo contains all persons data that is combined in a specific way
                oneCombosTrainingData.insert(oneCombosTrainingData.end(), m_allCombosOfData[person]->at(p_combo).begin(), m_allCombosOfData[person]->at(p_combo).end());
            }
        }
        // Now we create the training data from the accumulated training data
        FANN::training_data trainingData = CreateTrainingDataFromListOfDataSet(oneCombosTrainingData);
        FANN::training_data validationData = CreateTrainingDataFromListOfDataSet(oneCombosValidationData);
        //trainingData.save_train("Stupid.txt");
        //trainingData.read_train_from_file("Stupid.txt");
        m_factory.SetNumBestNetworks(100000);

        p_netSetting.trainingData = new FANN::training_data(trainingData);
        // A bit of a ugly hax to make no validation data work...
        if (m_validationAmount != 0)
        {
            p_netSetting.validationData = new FANN::training_data(validationData);
        }
        else
        {
            p_netSetting.validationData = nullptr;
        }
        p_netSetting.inputCells = m_allCombosOfData[0]->at(p_combo).at(0).inputs;

        // Find out what we want to run and run it. Its here we can have special things, like saving down a run net for gnuplotter
        switch (m_creationType)
        {
        case NetworkCreationType::CreateAllCombinations:
            m_factory.CreateNewNeuralNetworkCombinationsFromData(&trainingData, &validationData);
            m_factory.JoinNetworkThreads();
            break;
        case NetworkCreationType::BuildFromBaseline:
            m_factory.CreateAndRunNetworksFromBaseline(p_netSetting, ConfigHandler::Get()->m_settingsToTest);
            break;
        case NetworkCreationType::CreateOneSpecific:
        {
            m_factory.SetDeleteCompletedNetworks(false);
            m_factory.CreateSpecificNeuralNetwork(p_netSetting);
            break;
        }
        default:
            break;
        }

        // A bit of a ugly hax to make it work with no validation
        if (m_validationAmount == 0)
        {
            break;
        }

        // Print progress
        if (ConfigHandler::Get()->m_logLevel >= LogLevel::Progress)
        {
            float percentage = (float)(validationSet + 1) / (float)p_totalNumberOfPersons;
            std::cout << "Cross validation progress: " << percentage * 100.0f << "%" << std::endl;
        }
        // Delete the pointers created to avoid memory leak
        // TODO make sure we dont have any old code in one of the factory functions that does this
        if (p_netSetting.trainingData != nullptr)
        {
            delete p_netSetting.trainingData;
        }
        if (p_netSetting.validationData != nullptr)
        {
            delete p_netSetting.validationData;
        }
    }
}

std::vector<std::string> FileCombiner::GetAllFilesWithStampAndShrinkList(const std::string& p_stamp, std::vector<std::string>& o_files)
{
    std::vector<std::string> r_indices;
    size_t length = o_files.size();
    for (size_t fileToLookAt = 0; fileToLookAt < length; fileToLookAt++)
    {
        // This should find if stamp is in file name. Just make sure the same stamp is not present in more than the files that should count
        if (o_files[fileToLookAt].find(p_stamp) != std::string::npos) {
            r_indices.push_back(o_files[fileToLookAt]);
            // We remove that entry from the list
            o_files.erase(o_files.begin() + fileToLookAt);
            // Need to get length again
            length = o_files.size();
            // This is so that we consider every file entry
            fileToLookAt--;
        }
    }
    return r_indices;
}

FANN::training_data FileCombiner::CreateTrainingDataFromListOfDataSet(std::vector<DataSet>& p_allCombosOfData)
{
    int dataSetSize = p_allCombosOfData.size();
    if (dataSetSize == 0)
    {
        FANN::training_data dummyReturn;
        return dummyReturn;
    }
    // Build mergedData into something useful
    float** inputs;
    inputs = (float**)malloc(sizeof(float*) * dataSetSize);
    float** outputs;
    outputs = (float**)malloc(sizeof(float*) * dataSetSize);

    int numInputs = p_allCombosOfData[0].inputs;
    // Take this combination and put all inputs and outputs into appropriate arrays
    for (size_t j = 0; j < dataSetSize; j++)
    {

        //inputs[j] = (float*)malloc(sizeof(float) * numInputs);
        inputs[j] = &p_allCombosOfData.at(j).values[0];
        //outputs[j] = (float*)malloc(sizeof(float));
        outputs[j] = &p_allCombosOfData.at(j).output;
    }


    FANN::training_data data;// = new FANN::training_data();
    data.set_train_data(dataSetSize, p_allCombosOfData.at(0).inputs, inputs, 1, outputs);
    free(inputs);
    free(outputs);
    return data;

}

