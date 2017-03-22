#include "DataStillManager.h"
#include "DataStill.h"
#include "KeyMaskInterpreter.h"
#include <Windows.h>
#include <FileHandler.h>
#include <algorithm>
#include <iostream>
#include <sstream>
#include <iterator>
#include <fstream>
#include <thread>
#include <ConfigHandler.h>

using namespace std;
DataStillManager::DataStillManager()
{
    std::string t_stampLayout = "YYYY-MM-DD - hh-mm-ss";
    m_stampSize = t_stampLayout.length();

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
    int i = 0;
    while (filesInDirectory.size() > 0)
    {
        // Get all files of one person
        std::vector<std::string> t_allFilesFromPerson;

        int t_stampBegins = filesInDirectory[0].length() - p_readFileEnding.length() - 1 - m_stampSize;
        std::string t_stamp = filesInDirectory[0].substr(t_stampBegins, m_stampSize);
        t_allFilesFromPerson = GetAllFilesWithStampAndShrinkList(t_stamp, filesInDirectory);
        // Sort to make sure the files are always sent in in the same order
        std::sort(t_allFilesFromPerson.begin(), t_allFilesFromPerson.end());
      
        // When we get here all files of the same type should be in t_allfilesoftype. Send it of to processing
#ifdef MULTITHREAD
        t_threads.push_back(thread(&DataStillManager::ProcessFilesAndSaveToFile, this, t_allFilesFromPerson, p_writeDirectoryPath, p_readFileEnding, p_writeFileEnding));
#else 
        ProcessFilesAndSaveToFile(t_allFilesFromPerson, p_writeDirectoryPath, p_readFileEnding, p_writeFileEnding);
#endif // MULTITHREAD
        i++;
    }
    for (size_t i = 0; i < t_threads.size(); i++)
    {
        t_threads.at(i).join();
    }
}

void CheckThatAllEntiresHaveSameNrOfWords(std::vector<std::string>& p_lines)
{
	string line = p_lines.at(1);
	istringstream in(line);
	int inputs = std::distance(istream_iterator<string>(istringstream(line) >> ws), istream_iterator<string>());
	for (size_t i = 1; i < p_lines.size()-1; i+=3)
	{
		line = p_lines.at(i);
		istringstream in(line);
		int theseInputs = std::distance(istream_iterator<string>(istringstream(line) >> ws), istream_iterator<string>());
		if (theseInputs != inputs)
		{
			int derp = 2;
		}
	}
}


void DataStillManager::ProcessFilesAndSaveToFile(std::vector<std::string> p_files, const std::string& p_writeDirectoryPath
    , const std::string& p_readFileEnding, const std::string& p_writeFileEnding)
{
    if (p_files[1].find("Position") == std::string::npos)
    {
        // We should really not get here!!
        std::cout << "WRONG SORT ON LIST!!!";
        return;
    }
    cout << "starting process" << endl;
    DataStill still;
    KeyMaskInterpreter keyMaskInterpeter;
    std::string outputPath = FileHandler::GetAbsoluteFilePath(p_writeDirectoryPath);

    size_t length = p_files.size();

    // We need to read all files first, to apply remove dead filter
    std::vector<std::vector<std::string>>* t_allFileContent = new std::vector<std::vector<std::string>>();
    for (size_t currentFile = 0; currentFile < length; currentFile++)
    {
        t_allFileContent->push_back(*FileHandler::ReadFileIntoLines(p_files.at(currentFile)));
    }

    std::vector<std::vector<std::string>>* toRemove = t_allFileContent;
    // add 0.5f to threshold to avoid float errors...
    t_allFileContent = still.RemoveDeadData(t_allFileContent->at(1), *t_allFileContent, ConfigHandler::Get()->m_maxDisplacement + 0.5f);
    delete toRemove;

    bool normalize = false;

    for (size_t currentFile = 0; currentFile < length; currentFile++)
    {
        std::vector<std::string>* fileContent = &t_allFileContent->at(currentFile);
        // In this if, and the following elses, we take the beginning of the file name, as long as the identifyer we want to compare with
        // and then we simply compare that substring to the wanted string. So basically we take the word in the beginning in a file name
        // and see what it is
        ////////////////// KEY PRESSES //////////////////////
        if (p_files[currentFile].find(m_keyPressesRawDataBegining) != std::string::npos)
        {
            // It's a keypresses file, perform special thingies here! We might need to remove an extra data point to fit with displacements
            keyMaskInterpeter.ReinterpretRawKeyData(fileContent);
            // TODO doesn't this introduce a memory leak, since we change the pointer of fileContent to a new one but doesn't remove the old one
            fileContent = still.FilterAdd2(*fileContent, ConfigHandler::Get()->m_entriesToAvrage);
            fileContent = still.NormalizeValuesUsingNumber(*fileContent, ConfigHandler::Get()->m_entriesToAvrage, p_files[currentFile]);
			CheckThatAllEntiresHaveSameNrOfWords(*fileContent);

        }
        ////////////////// POSITIONS //////////////////////
        else if (p_files[currentFile].find(m_positionRawDataBegining) != std::string::npos)
        {
            // It's a positions file, perform special thingies here!
            fileContent = still.FilterDisplacement(*fileContent);
            fileContent = still.FilterAvrage2(*fileContent, ConfigHandler::Get()->m_entriesToAvrage);
            // +0.5 since that is what we used for removing dead thingies
            fileContent = still.NormalizeValuesUsingNumber(*fileContent, ConfigHandler::Get()->m_maxDisplacement + 0.5f, p_files[currentFile]);
			CheckThatAllEntiresHaveSameNrOfWords(*fileContent);        
        }
        ////////////////// ROTATIONS //////////////////////
        else if (p_files[currentFile].find(m_rotationRawDataBegining) != std::string::npos)
        {
            // It's a rotations file, perform special thingies here!
            fileContent = still.FilterRotations(*fileContent);
            fileContent = still.FilterAvrage2(*fileContent, ConfigHandler::Get()->m_entriesToAvrage);
            fileContent = still.NormalizeValuesUsingNumber(*fileContent, 3.14f, p_files[currentFile]);
			CheckThatAllEntiresHaveSameNrOfWords(*fileContent);
        }
        ////////////////// TimeOfDay //////////////////////
        else if (p_files[currentFile].find(m_timeofdayRawDataBegining) != std::string::npos)
        {
            // It's a time of day file, perform special thingies here!
            still.RemoveNonSequentialIndex(fileContent);
            fileContent = still.FilterAvrage2(*fileContent, ConfigHandler::Get()->m_entriesToAvrage);
			CheckThatAllEntiresHaveSameNrOfWords(*fileContent);
            normalize = false;
        }
		if (currentFile == 2)
		{
			int derp = 2;
		}
        // Perform general things, same for each file
        fileContent = still.MergeDataOntoSameLine2(*fileContent, ConfigHandler::Get()->m_entriesToMerge);
		CheckThatAllEntiresHaveSameNrOfWords(*fileContent);

        // Finally we save the data in the new pointer to the container
        t_allFileContent->at(currentFile) = *fileContent;
        delete fileContent;
    }
    if (normalize)
    {
        // This should not be called since we now take all the files of one player and not all files of one type..
        toRemove = t_allFileContent;
        t_allFileContent = still.NormalizeValuesWithHighestFound(*t_allFileContent); 
        delete toRemove;
    }

    // Debug make sure every entry have the same amount of rows
    int rows = t_allFileContent->at(0).size();
    for (size_t i = 1; i < t_allFileContent->size(); i++)
    {
        if (rows != t_allFileContent->at(i).size())
        {
            int hej = 1;
            std::cout << "WRONG!!";
        }
    }

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

std::vector<std::string> DataStillManager::GetAllFilesWithStampAndShrinkList(const std::string& p_stamp, std::vector<std::string>& o_files)
{
    std::vector<std::string> r_indices;
    size_t length = o_files.size();
    for (size_t fileToLookAt = 0; fileToLookAt < length; fileToLookAt++)
    {
        // This should find if stamp is in file name. Just make sure the same stamp is not present in more than the files that should count
        if (o_files[fileToLookAt].find(p_stamp) != std::string::npos) {
            r_indices.push_back(o_files[fileToLookAt]);
            // We remove that entry from the list
            o_files.erase(o_files.begin() + fileToLookAt);
            // Need to get length again
            length = o_files.size();
            // This is so that we consider every file entry
            fileToLookAt--;
        }
    }
    return r_indices;
}