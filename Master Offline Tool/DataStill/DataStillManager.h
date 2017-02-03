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
    void FiltrateAllFilesInDirectory(const std::string& p_readDirectoryPath, const std::string& p_writeDirectoryPath, const std::string& p_readFileEnding, const std::string& p_writeFileEnding);

    
private:
    std::string m_positionRawDataBegining;
    std::string m_rotationRawDataBegining;
    std::string m_keyPressesRawDataBegining;
};

