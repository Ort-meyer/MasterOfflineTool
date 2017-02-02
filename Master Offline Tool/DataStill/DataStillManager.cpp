#include "DataStillManager.h"
#include "DataStill.h"
#include "KeyMaskInterpreter.h"
#include <Windows.h>



DataStillManager::DataStillManager()
{
    m_keyPressesRawDataBegining = "Keypresses";
    m_positionRawDataBegining = "Positions";
    m_rotationRawDataBegining = "Rotations";
}


DataStillManager::~DataStillManager()
{
}

void DataStillManager::FiltrateAllFilesInDirectory(std::string p_directoryPath)
{
    DataStill still;
    KeyMaskInterpreter keyMaskInterpeter;
    std::string directoryPath = GetAbsoluteFilePath(p_directoryPath);
    std::vector<std::string> filesInDirectory = GetAllFileNames(p_directoryPath, "debug");
    size_t numberOfFilesInDirectory = filesInDirectory.size();
    for (size_t currentFile = 0; currentFile < numberOfFilesInDirectory; currentFile++)
    {
        std::vector<std::string>* fileContent = still.ReadFileIntoLines(directoryPath + filesInDirectory[currentFile]);
        if (filesInDirectory[currentFile].compare(m_keyPressesRawDataBegining) == m_keyPressesRawDataBegining.length())
        {
            // It's a keypresses file, perform special thingies here!
            keyMaskInterpeter.ReinterpretRawKeyData(fileContent);
        }
        else if (filesInDirectory[currentFile].compare(m_positionRawDataBegining) == m_positionRawDataBegining.length())
        {
            // It's a positions file, perform special thingies here!
        }
        else if (filesInDirectory[currentFile].compare(m_rotationRawDataBegining) == m_rotationRawDataBegining.length())
        {
            // It's a rotations file, perform special thingies here!
        }
        delete fileContent;
    }
}

std::vector<std::string> DataStillManager::GetAllFileNames(std::string p_directory, const std::string& p_fileEnding)
{
    std::string fullPath;
    // Get current working directory and add the specific directory we want after
    char buffer[MAX_PATH];
    GetModuleFileName(NULL, buffer, MAX_PATH);
    std::string::size_type pos = std::string(buffer).find_last_of("\\/");
    fullPath = std::string(buffer).substr(0, pos);
    // Add in directory and file ending of stuff we want
    fullPath += "\\";
    fullPath += p_directory;
    fullPath += "/*." + p_fileEnding;
    std::vector<std::string> t_rawDataFileNames;

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

std::string DataStillManager::GetAbsoluteFilePath(std::string p_directory)
{
    std::string fullPath;
    // Get current working directory and add the specific directory we want after
    char buffer[MAX_PATH];
    GetModuleFileName(NULL, buffer, MAX_PATH);
    std::string::size_type pos = std::string(buffer).find_last_of("\\/");
    fullPath = std::string(buffer).substr(0, pos);
    // Add in directory and file ending of stuff we want
    fullPath += "\\";
    fullPath += p_directory;
    return fullPath;
}