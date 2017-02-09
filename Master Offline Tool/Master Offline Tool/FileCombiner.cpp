#include "FileCombiner.h"
#include "DataSetBuilder.h"
#include "NeuralNetworkFactory.h"
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <FileHandler.h>
#include <algorithm>
#include <iostream>
static FileCombiner m_this;

FileCombiner::FileCombiner(): m_dataSetBuilder(new DataSetBuilder())
{
    std::string t_stampLayout = "YYYY-MM-DD - hh-mm-ss";
    m_stampSize = t_stampLayout.length();
    m_validationAmount = 1;
    CombineFilesInFolder();
    FeedDataToNeuralNetworkFactory();
    int a;
    std::cin >> a;
}


FileCombiner::~FileCombiner()
{
    delete m_dataSetBuilder;
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
        m_allCombosOfData.push_back(m_dataSetBuilder->BuildDataSetFromFiles(t_filesInCombination));
    }
    m_dataSetCombinationsPerPerson = m_allCombosOfData[0]->size();
}

void FileCombiner::FeedDataToNeuralNetworkFactory()
{
    NeuralNetworkFactory t_factory;
    int numberOfPersons = m_allCombosOfData.size();
    // The trainingdata attached to one combo
    std::vector<FANN::training_data> t_allTrainingData;
    
    size_t numberOfCombos = m_allCombosOfData[0]->size();

    for (size_t combo = 0; combo < numberOfCombos; combo++)
    {
        std::cout << m_dataSetBuilder->GetComboNameFromIndex(combo) << std::endl;
        // This loop is for cross validation
        for (size_t validationSet = 0; validationSet < numberOfPersons; validationSet++)
        {
            // This is used to make the validationset wrap around the vector
            int low = numberOfPersons - validationSet - m_validationAmount;
            int validationSetAmountOverSize = min(low, 0);
            validationSetAmountOverSize = abs(validationSetAmountOverSize);
            std::vector<DataSet> oneCombosTrainingData;
            std::vector<DataSet> oneCombosValidationData;
            // This loop is to add all persons (excpet the validaiton set) to the training data that will be created later
            for (size_t person = 0; person < numberOfPersons; person++)
            {
                // If the current preson is inside the validation range
                if (person >= validationSet && person < validationSet + m_validationAmount || person<validationSetAmountOverSize)
                {
                    oneCombosValidationData.insert(oneCombosValidationData.end(), m_allCombosOfData[person]->at(combo).begin(), m_allCombosOfData[person]->at(combo).end());
                }
                else
                {
                    // This make it so oneCombo contains all persons data that is combined in a specific way
                    oneCombosTrainingData.insert(oneCombosTrainingData.end(), m_allCombosOfData[person]->at(combo).begin(), m_allCombosOfData[person]->at(combo).end());
                }
            }
            //netFac.CreateSpecificNeuralNetwork(&data, )
            int* myInt = new int(20);
            //netFac.SetVariables(1, 3, 1, 0.3, 0.3, 0.3, 1);
            FANN::training_data trainingData = CreateTrainingDataFromListOfDataSet(oneCombosTrainingData);
            FANN::training_data validationData = CreateTrainingDataFromListOfDataSet(oneCombosValidationData);

            t_factory.CreateSpecificNeuralNetwork(&trainingData, 1, myInt, FANN::activation_function_enum::SIGMOID_SYMMETRIC, FANN::activation_function_enum::SIGMOID_SYMMETRIC,
                0.7f, 1.0f, 1.0f, true, 10000, 1000, 0.0001f, &validationData);
            //netFac.CreateNewNeuralNetworkCombinationsFromData(&data);
            delete myInt;
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

    // Build mergedData into something useful
    float** inputs;
    inputs = (float**)malloc(sizeof(float*) * dataSetSize);
    float** outputs;
    outputs = (float**)malloc(sizeof(float*) * dataSetSize);

    int numInputs = p_allCombosOfData[0].inputs;
    // Take this combination and put all inputs and outputs into appropriate arrays
    for (size_t j = 0; j < dataSetSize; j++)
    {
        
        inputs[j] = (float*)malloc(sizeof(float) * numInputs);
        inputs[j] = &p_allCombosOfData.at(j).values[0];
        outputs[j] = (float*)malloc(sizeof(float));
        outputs[j] = &p_allCombosOfData.at(j).output;
    }


    FANN::training_data data;
    data.set_train_data(dataSetSize, p_allCombosOfData.at(0).inputs, inputs, 1, outputs);

    return data;
    
}

