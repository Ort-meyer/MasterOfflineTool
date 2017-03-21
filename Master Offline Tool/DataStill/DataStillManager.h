#pragma once
#include <string>
#include <vector>

// Whether we want to multithread the still's filtering
#define MULTITHREAD

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
    void ProcessFilesAndSaveToFile(std::vector<std::string> p_files, const std::string& p_writeDirectoryPath
        , const std::string& p_readFileEnding, const std::string& p_writeFileEnding);

    /**
    returns all the files of the same person
    */
    std::vector<std::string> GetAllFilesWithStampAndShrinkList(const std::string & p_stamp, std::vector<std::string>& o_files);
    
    std::string m_keyPressesRawDataBegining;
    std::string m_positionRawDataBegining;
    std::string m_rotationRawDataBegining;
    std::string m_timeofdayRawDataBegining;
    std::vector<std::string> m_fileTypes;
    int m_stampSize;
};

