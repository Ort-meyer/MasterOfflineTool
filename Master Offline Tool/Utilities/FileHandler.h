#pragma once
#include <string>
#include <vector>
#include "../Master Offline Tool/NeuralNetwork.h"

namespace FileHandler
{
    /**
    Method that returns vector of paths to all files within the specified
    directory. Directory is only local directory, */
    std::vector<std::string> GetAllFileNames(std::string p_directory, std::string p_fileEnding);

    /**
    Returns the full path to files we might want to open
    Parameter is directory path as given relative to program .exe*/
    std::string GetAbsoluteFilePath(std::string p_directory);

    /**
    Help method that returns the file specified as a vector of strings where
    each string is a line fron the read file.*/
    std::vector<std::string>* ReadFileIntoLines(std::string p_fullFilePath);

    /**
    Interprets p_lines as a list of lines to be written into a file.
    Will write into p_fileName*/
    void WriteToFile(const std::vector<std::string>& p_lines, std::string p_fullFilePath);

    /**
    Interprets p_lines as a list of lines to be appended to the end of a file.
    Will write into p_fileName*/
    void AppendToFile(const std::vector<std::string>& p_lines, const std::string & p_fullFilePath);

    /**
    Takes network settings and turns into a string which can then be saved to a
    file if one wants to.*/
    std::string SaveNetworkToString(const NetworkSettings& p_network);

    /**
    Copys given file to folder, if folder does not exist it will be created
    */
    void CopyFileToFolder(const std::string& p_folderFullPath, const std::string& p_fileFullPath);
};

