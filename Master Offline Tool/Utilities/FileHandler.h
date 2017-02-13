#pragma once
#include <string>
#include <vector>
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
};

