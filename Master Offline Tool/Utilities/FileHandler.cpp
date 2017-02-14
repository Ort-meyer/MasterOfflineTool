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