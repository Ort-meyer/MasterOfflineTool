#pragma once
#include <string>
#include <vector>

class DataStillManager
{
public:
    DataStillManager();
    ~DataStillManager();
    /**
    Filtrates all the files in the given directory path, path should be local path, not absolute
    */
    void FiltrateAllFilesInDirectory(std::string p_directoryPath);

    
private:
    /**
    Returns the full path to files we might want to open
    Parameter is directory path as given relative to program .exe*/
    std::string GetAbsoluteFilePath(std::string p_directory);

    std::vector<std::string> GetAllFileNames(std::string p_directory, const std::string& p_fileEnding);

    std::string m_positionRawDataBegining;
    std::string m_rotationRawDataBegining;
    std::string m_keyPressesRawDataBegining;
};

