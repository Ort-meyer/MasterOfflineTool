#include "DataStillManager.h"
#include "DataStill.h"
#include "KeyMaskInterpreter.h"
#include <Windows.h>
#include <FileHandler.h>


DataStillManager::DataStillManager()
{
    m_keyPressesRawDataBegining = "Keypresses";
    m_positionRawDataBegining = "Positions";
    m_rotationRawDataBegining = "Rotations";
}


DataStillManager::~DataStillManager()
{
}

void DataStillManager::FiltrateAllFilesInDirectory(const std::string& p_readDirectoryPath, const std::string& p_writeDirectoryPath
    , const std::string& p_readFileEnding, const std::string& p_writeFileEnding)
{
    DataStill still;
    KeyMaskInterpreter keyMaskInterpeter;
    std::string directoryPath = FileHandler::GetAbsoluteFilePath(p_readDirectoryPath);
    std::string outputPath = FileHandler::GetAbsoluteFilePath(p_writeDirectoryPath);
    std::vector<std::string> filesInDirectory = FileHandler::GetAllFileNames(p_readDirectoryPath, p_readFileEnding);
    size_t numberOfFilesInDirectory = filesInDirectory.size();
    for (size_t currentFile = 0; currentFile < numberOfFilesInDirectory; currentFile++)
    {
        std::vector<std::string>* fileContent = FileHandler::ReadFileIntoLines(filesInDirectory[currentFile]);
        still.FlagDataOutput(*fileContent, 60 * 5, 1);
        // here we seperate the file name from the absolute path
        size_t fileNameBegins = filesInDirectory[currentFile].find_last_of("//");
        std::string fileName = filesInDirectory[currentFile].substr(fileNameBegins+1);

        // In this if, and the following elses, we take the beginning of the file name, as long as the identifyer we want to compare with
        // and then we simply compare that substring to the wanted string. So basically we take the word in the beginning in a file name
        // and see what it is
        if (fileName.substr(0, m_keyPressesRawDataBegining.length()).compare(m_keyPressesRawDataBegining) == 0)
        {
            // It's a keypresses file, perform special thingies here!
            keyMaskInterpeter.ReinterpretRawKeyData(fileContent);
            fileContent = still.FilterAdd(*fileContent, 60);
        }
        else if (fileName.substr(0, m_positionRawDataBegining.length()).compare(m_positionRawDataBegining) == 0)
        {
            // It's a positions file, perform special thingies here!
            fileContent = still.FilterDisplacement(*fileContent);
            fileContent = still.FilterAvrage(*fileContent, 60);
        }
        else if (fileName.substr(0, m_rotationRawDataBegining.length()).compare(m_rotationRawDataBegining) == 0)
        {
            // It's a rotations file, perform special thingies here!
            fileContent = still.FilterAvrage(*fileContent, 60);
        }
        // Perform general things, same for each file
        fileContent = still.MergeDataOntoSameLine(*fileContent, 5);
		std::string outputFilename = outputPath + fileName;
        outputFilename.erase(outputFilename.end() - p_readFileEnding.length(), outputFilename.end());
		outputFilename += p_writeFileEnding;

		FileHandler::WriteToFile(*fileContent, outputFilename);
        delete fileContent;
    }
}