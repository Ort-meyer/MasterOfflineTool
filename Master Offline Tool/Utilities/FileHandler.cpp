#include "FileHandler.h"

#include <fstream>
#include <iostream>
#include <iterator>
#include <sstream>
#include <Windows.h>
using namespace std;
std::vector<std::string> FileHandler::GetAllFileNames(std::string p_directory, std::string p_fileEnding)
{

    string folderPath = GetAbsoluteFilePath(p_directory);
    string fullPath = folderPath;
    fullPath += "/*.";
    fullPath += p_fileEnding;
    vector<string> t_rawDataFileNames;

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
    vector<string> t_fullPathFiles;
    for (size_t i = 0; i < t_rawDataFileNames.size(); i++)
    {
        string fullFilePath = folderPath;
        fullFilePath += "/";
        fullFilePath += t_rawDataFileNames.at(i);
        t_fullPathFiles.push_back(fullFilePath);
    }
    FindClose(hFind);
    return t_fullPathFiles;
}

std::string FileHandler::GetAbsoluteFilePath(std::string p_directory)
{
    string fullPath;
    // Get current working directory and add the specific directory we want after
    char buffer[MAX_PATH];
    GetModuleFileName(NULL, buffer, MAX_PATH);
    string::size_type pos = string(buffer).find_last_of("\\/");
    fullPath = string(buffer).substr(0, pos);
    // Add in directory and file ending of stuff we want
    fullPath += "\\";
    fullPath += p_directory;
    return fullPath;
}


std::vector<std::string>* FileHandler::ReadFileIntoLines(std::string p_fullFilePath)
{
    // Read all lines of file into vector of strings
    vector<string>* lines = new vector<string>();
    ifstream inFile(p_fullFilePath);
    string line;
    while (std::getline(inFile, line))
    {
        lines->push_back(line);
    }
    inFile.close();
    return lines;
}

void FileHandler::WriteToFile(const std::vector<std::string>& p_lines, std::string p_fullFilePath)
{
    // Open and write
    ofstream outFile;
    outFile.open(p_fullFilePath);
    for (size_t i = 0; i < p_lines.size(); i++)
    {
        outFile << p_lines.at(i) << endl;
    }
    outFile.close();
}

void FileHandler::AppendToFile(const std::vector<std::string>& p_lines, const std::string& p_fullFilePath)
{  
  std::ofstream outfile;

  outfile.open(p_fullFilePath, std::ios_base::app);
  for (size_t i = 0; i < p_lines.size(); i++)
  {
      outfile << p_lines.at(i) << endl;
  }
  outfile.close();
}

string FileHandler::SaveNetworkToString(const NetworkSettings & p_network)
{
    std::ostringstream newEntry;
    // Add all the important network info
    newEntry << "ID: " << p_network.idString;
    newEntry << " MSE: " << p_network.mse;
    newEntry << " Percentile correct: " << p_network.correctPercentile;
    newEntry << " Mean error: " << p_network.meanError;
    newEntry << " Guessed lost when found: " << p_network.lostWrongGuesses;
    newEntry << " Did retraining: " << (p_network.didRetrain ? "true" : "false");
    if (p_network.didRetrain)
    {
        newEntry << " Retraining was good: " << (p_network.retrainingWasGood ? "true" : "false");
        newEntry << " Number of Epochs trainined: " << p_network.bestEpoch.bestEpoch;
    }
    newEntry << std::endl << "---Network settings---" << std::endl;
    for (size_t i = 0; i < p_network.hiddenLayers; i++)
    {
        newEntry << " hidden layers no: " << i + 1 << " cells: " << p_network.hiddenCells[i] << std::endl;
    }
    newEntry << " Function hidden: ";
    newEntry << p_network.functionHidden;
    newEntry << " Function output: ";
    newEntry << p_network.functionOutput;
    newEntry << " Training algorithm: ";
    newEntry << p_network.trainingAlgorithm;
    newEntry << " Learning rate: ";
    newEntry << p_network.learningRate;
    newEntry << " Steepness hidden: ";
    newEntry << p_network.steepnessHidden;
    newEntry << " Steepness output: ";
    newEntry << p_network.steepnessOutput;
    newEntry << " Deterministic weights: ";
    newEntry << p_network.deterministicWeights;
    newEntry << std::endl;
    newEntry << "Biggest difference : ";
    newEntry << p_network.bestEpoch.difference;
    newEntry << std::endl;
    newEntry << "MSE values: ";
    for (size_t i = 0; i < p_network.bestEpoch.mseList.size(); i++)
    {
        newEntry << p_network.bestEpoch.mseList.at(i) << " ";
    }
    newEntry << std::endl;
    // Add the new entry to all the lines that should be writen
    return newEntry.str();
}

void FileHandler::CopyFileToFolder(const std::string & p_folderFullPath, const std::string & p_fileFullPath)
{
    if (CreateDirectory(p_folderFullPath.c_str(), NULL) || ERROR_ALREADY_EXISTS == GetLastError())
    {
        // Folder creted or already existed.
        std::string fileName = p_fileFullPath.substr(p_fileFullPath.find_last_of('/'));
        CopyFile(p_fileFullPath.c_str(), (p_folderFullPath + fileName).c_str(), false);
    }
}

void FileHandler::CreateFolder(const std::string& p_folderToCreate)
{
    CreateDirectory(p_folderToCreate.c_str(), NULL);
}
