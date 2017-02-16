#include "DataStillManager.h"
#include "DataStill.h"
#include "KeyMaskInterpreter.h"
#include <Windows.h>
#include <FileHandler.h>
#include <algorithm>
#include <iostream>
#include <thread>
using namespace std;
DataStillManager::DataStillManager()
{
    m_keyPressesRawDataBegining = "Keypresses";
    m_positionRawDataBegining = "Positions";
    m_rotationRawDataBegining = "Rotations";
    m_timeofdayRawDataBegining = "TimeOfDay";
    m_fileTypes.push_back(m_keyPressesRawDataBegining);
    m_fileTypes.push_back(m_positionRawDataBegining);
    m_fileTypes.push_back(m_rotationRawDataBegining);
    m_fileTypes.push_back(m_timeofdayRawDataBegining);
}


DataStillManager::~DataStillManager()
{
}

void DataStillManager::FiltrateAllFilesInDirectory(const std::string& p_readDirectoryPath, const std::string& p_writeDirectoryPath
    , const std::string& p_readFileEnding, const std::string& p_writeFileEnding)
{
    std::string directoryPath = FileHandler::GetAbsoluteFilePath(p_readDirectoryPath);

    std::vector<std::string> filesInDirectory = FileHandler::GetAllFileNames(p_readDirectoryPath, p_readFileEnding);
    std::sort(filesInDirectory.begin(), filesInDirectory.end());

    vector<thread> t_threads;
    while (filesInDirectory.size() > 0)
    {
        // Contains files of the same type, like all position files for example
        std::vector<std::string> t_allFilesOfType;
        std::string t_firstFileType;
        // Loop through all the file types to find which typ this is
        for (size_t i = 0; i < m_fileTypes.size(); i++)
        {
            if (filesInDirectory[0].find(m_fileTypes[i]) != std::string::npos)
            {
                t_firstFileType = m_fileTypes[i];
                break;
            }
        }

        for (size_t currentFile = 0; currentFile < filesInDirectory.size(); currentFile++)
        {
            // This means that this file is of the same typ as the once we are trying to find
            if (filesInDirectory[currentFile].find(t_firstFileType) != std::string::npos)
            {
                t_allFilesOfType.push_back(filesInDirectory[currentFile]);
                // Remove the file from the unchecked once, we dont want to add it again...
                filesInDirectory.erase(filesInDirectory.begin() + currentFile);
                currentFile--;
            }
        }
        // When we get here all files of the same type should be in t_allfilesoftype. Send it of to processing
#ifdef MULTITHREAD
        t_threads.push_back(thread(&DataStillManager::ProcessFilesAndSaveToFile, this, t_allFilesOfType, p_writeDirectoryPath, p_readFileEnding, p_writeFileEnding));
#else 
        ProcessFilesAndSaveToFile(t_allFilesOfType, p_writeDirectoryPath, p_readFileEnding, p_writeFileEnding);
#endif // MULTITHREAD
    }
    for (size_t i = 0; i < t_threads.size(); i++)
    {
        t_threads.at(i).join();
    }
}

void derp(int t, int f)
{
    int q = t + f;
}

void merp()
{
    std::thread thread(derp, 1, 2);
}

void DataStillManager::ProcessFilesAndSaveToFile(std::vector<std::string> p_files, const std::string& p_writeDirectoryPath
    , const std::string& p_readFileEnding, const std::string& p_writeFileEnding)
{
    cout << "starting process" << endl;
    DataStill still;
    KeyMaskInterpreter keyMaskInterpeter;
    std::string outputPath = FileHandler::GetAbsoluteFilePath(p_writeDirectoryPath);

    size_t length = p_files.size();

    // We need to read all files first, to apply normalize filter
    std::vector<std::vector<std::string>>* t_allFileContent = new std::vector<std::vector<std::string>>();
    for (size_t currentFile = 0; currentFile < length; currentFile++)
    {
        t_allFileContent->push_back(*FileHandler::ReadFileIntoLines(p_files.at(currentFile)));
    }

    bool normalize = true;

    for (size_t currentFile = 0; currentFile < length; currentFile++)
    {
        std::vector<std::string>* fileContent = &t_allFileContent->at(currentFile);
        //still.FlagDataOutput(*fileContent, 60 * 5, 1);

        // In this if, and the following elses, we take the beginning of the file name, as long as the identifyer we want to compare with
        // and then we simply compare that substring to the wanted string. So basically we take the word in the beginning in a file name
        // and see what it is
        ////////////////// KEY PRESSES //////////////////////
        if (p_files[currentFile].find(m_keyPressesRawDataBegining) != std::string::npos)
        {
            // It's a keypresses file, perform special thingies here!
            keyMaskInterpeter.ReinterpretRawKeyData(fileContent);
            // TODO doesn't this introduce a memory leak, since we change the pointer of fileContent to a new one but doesn't remove the old one
            fileContent = still.FilterAdd2(*fileContent, 60);
        }
        ////////////////// POSITIONS //////////////////////
        else if (p_files[currentFile].find(m_positionRawDataBegining) != std::string::npos)
        {
            // It's a positions file, perform special thingies here!
            fileContent = still.FilterDisplacement(*fileContent);
            fileContent = still.FilterAvrage2(*fileContent, 60);
        }
        ////////////////// ROTATIONS //////////////////////
        else if (p_files[currentFile].find(m_rotationRawDataBegining) != std::string::npos)
        {
            // It's a rotations file, perform special thingies here!
            fileContent = still.FilterRotations(*fileContent);
            fileContent = still.FilterAvrage2(*fileContent, 60);
        }
        ////////////////// TimeOfDay //////////////////////
        else if (p_files[currentFile].find(m_timeofdayRawDataBegining) != std::string::npos)
        {
            // It's a rotations file, perform special thingies here!
            fileContent = still.FilterAvrage2(*fileContent, 60);
            normalize = false;
        }
        // Perform general things, same for each file
        fileContent = still.MergeDataOntoSameLine2(*fileContent, 5);

        // Finally we save the data in the new pointer to the container
        t_allFileContent->at(currentFile) = *fileContent;
        delete fileContent;
    }
    if (normalize)
        t_allFileContent = still.NormalizeValues(*t_allFileContent);



    // Write to file
    for (size_t i = 0; i < length; i++)
    {
        // here we seperate the file name from the absolute path
        size_t fileNameBegins = p_files.at(i).find_last_of("//");
        std::string fileName = p_files.at(i).substr(fileNameBegins + 1);

        std::string outputFilename = outputPath + fileName;
        outputFilename.erase(outputFilename.end() - p_readFileEnding.length(), outputFilename.end());
        outputFilename += p_writeFileEnding;

        FileHandler::WriteToFile(t_allFileContent->at(i), outputFilename);
    }
    delete t_allFileContent;
}

void dostuff()
{

}