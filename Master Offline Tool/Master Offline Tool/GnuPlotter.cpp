#include "GnuPlotter.h"
#include <FileHandler.h>
#include "DataSetBuilder.h"
#include <algorithm>

#define ROWSTOAVRAGE 60
#define ROWSONSAMEROW 5

using namespace FANN;
using namespace std;
GnuPlotter::GnuPlotter()
{
    CreatePeople("../RawData", "../FilteredData");
    RunNetworkAndPrepForGnuPlot("../SavedNetwork/mynet.ann");
    PrepGoldenDataForGnuPlot();
}


GnuPlotter::~GnuPlotter()
{
}


std::vector<std::string>* GnuPlotter::ReverseEngineerPositions(const std::vector<std::string>& p_lines, const int& p_index)
{
    vector<string>* r_lines = new vector<string>();
    size_t t_numLines = p_lines.size();
    for (size_t i_dataSetStart = 0; i_dataSetStart< t_numLines; i_dataSetStart+=3)
    {
        // Check if we've found the right index
        if (p_lines.at(i_dataSetStart) == to_string(p_index))
        {
            // We can assume that the next x * y entries from an index are the positions we're looking for
            size_t t_numRows = ROWSTOAVRAGE * ROWSONSAMEROW * 3; // times 3 since each entry is 3 rows
            // Loop over relevant positions and put in return list
            for (size_t i_pos = i_dataSetStart + 1; i_pos < t_numRows + i_dataSetStart + 1; i_pos += 3)
            {
                r_lines->push_back(p_lines.at(i_pos));
            }
            break;
        }
    }
    return r_lines;
}

void GnuPlotter::PrepGoldenDataForGnuPlot()
{
    size_t t_numPeople = m_people.size();
    for (size_t i_person = 0; i_person < t_numPeople; i_person++)
    {
        Person* t_thisGuy = &m_people.at(i_person);
        vector<string>* t_lines = new vector<string>();
        size_t t_numDataSets = t_thisGuy->dataSets->size();
        for (size_t i_dataSet = 0; i_dataSet < t_numDataSets; i_dataSet++)
        {
            DataSet* t_thisDataSet = &t_thisGuy->dataSets->at(i_dataSet);
            if (t_thisDataSet->output >= 0.99999f) // Not sure if epsilon is needed
            {
               vector<string>* t_newLines = ReverseEngineerPositions(*t_thisGuy->rawPosData, t_thisDataSet->index);
               // Merge with previous positions
               t_lines->insert(t_lines->end(), t_newLines->begin(), t_newLines->end());
            }
            else ////// ELSE SHOULD BE REMOVED COMPLETELY ///////////
            {
                vector<string>* t_newLines = ReverseEngineerPositions(*t_thisGuy->rawPosData, t_thisDataSet->index);
                // Merge with previous positions
                t_lines->insert(t_lines->end(), t_newLines->begin(), t_newLines->end());
            }
        }

        // Done with data sets. Time to save results to file
        string t_fileName = "../heatmaps/Golden/";
        t_fileName += "Golden";
        t_fileName += t_thisGuy->name;
        t_fileName += "heatmap.heatmap";
        FileHandler::WriteToFile(*t_lines, t_fileName);
    }
}

void GnuPlotter::RunNetworkAndPrepForGnuPlot(std::string p_annFilePath)
{
    // Load network
    neural_net t_net;
    t_net.create_from_file(p_annFilePath);
    // Run through all people (no, not like that...)
    size_t t_numPeople = m_people.size();
    for (size_t i_person = 0; i_person< t_numPeople; i_person++)
    {
        Person* t_thisGuy = &m_people.at(i_person);
        vector<string>* t_lines = new vector<string>();

        // Run each data set through the network
        size_t t_numDataSets = t_thisGuy->dataSets->size();
        for (size_t i_dataSet = 0; i_dataSet< t_numDataSets; i_dataSet++)
        {
            ////////////////////////// COMMENT STUFF BACK IN ////////////////////////
            DataSet* t_thisDataSet = &t_thisGuy->dataSets->at(i_dataSet);
            // Run this data set
            //float* output = t_net.run(&t_thisDataSet->values[0]);
            // Cast so we get 0 or 1
            //*output += 0.5f; // Tweak as necessary
            //bool lost = (int)(*output);
            if (rand()%3 == 0)//lost)
            {
                // Reverse engineer positions and write to file
                vector<string>* t_newLines = ReverseEngineerPositions(*t_thisGuy->rawPosData, t_thisDataSet->index);
                // Merge with previous positions
                t_lines->insert(t_lines->end(), t_newLines->begin(), t_newLines->end());
            }
        }

        // Done with data sets. Time to save results to file
        string t_fileName = "../heatmaps/Calculated/";
        t_fileName += "Calculated";
        t_fileName += t_thisGuy->name;
        t_fileName += "heatmap.heatmap";
        FileHandler::WriteToFile(*t_lines, t_fileName); // put in calculated folder since... well, it's calculated
    }
}

std::vector<std::string> GnuPlotter::GetAllFilesWithStampAndShrinkList(const std::string& p_stamp, std::vector<std::string>& o_files)
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

/**
Searches through all raw position files to find position data for the tag specified*/
std::vector<std::string>* GnuPlotter::GetRawPositions(const std::vector<std::string>p_posFileNames, const std::string& p_tag)
{
    // Find the correct file name
    string t_thisFile;
    size_t length = p_posFileNames.size();
    for (size_t fileToLookAt = 0; fileToLookAt < length; fileToLookAt++)
    {
        // This should find if stamp is in file name
        if (p_posFileNames[fileToLookAt].find(p_tag) != std::string::npos) {
            {
                t_thisFile = p_posFileNames[fileToLookAt];
                break;
            }
        }
    }

    // Open file and return
    return FileHandler::ReadFileIntoLines(t_thisFile);
}

void GnuPlotter::CreatePeople(std::string p_rawDataPath, std::string p_filteredDataPath)
{

    /// Find raw position file names
    // Get all raw data
    vector<string> t_allRawFileNames = FileHandler::GetAllFileNames(p_rawDataPath, "debug"); // remember to chance to .log file format!
    vector<string> t_allPositionFiles;
    // Filter so we only have those with position
    for (size_t i = 0; i < t_allRawFileNames.size(); i++)
    {
        size_t fileNameStart = t_allRawFileNames[i].find_last_of("/") + 1;
        if (t_allRawFileNames[i].substr(fileNameStart, 3) == "Pos")
        {
            // We found a position file!
            t_allPositionFiles.push_back(t_allRawFileNames.at(i));
        }
    }

    /// Build people
    string t_desiredCombo = "PosRot"; // Hard coded so far
    std::vector<std::string> t_rawDataFileNames = FileHandler::GetAllFileNames(p_filteredDataPath, "filteredData");

    int stampSize = string("YYYY-MM-DD - hh-mm-ss").length();
    int fileEndingSize = string("filteredData").length();

    // We know that the stamp is in the end of a file, just before the file ending
    for (size_t i = 0; i < t_rawDataFileNames.size(); )
    {
        int t_stampBegins = t_rawDataFileNames[i].length() - fileEndingSize - 1 - stampSize;
        std::string t_stamp = t_rawDataFileNames[i].substr(t_stampBegins, stampSize);
        std::vector<std::string> t_filesInCombination = GetAllFilesWithStampAndShrinkList(t_stamp, t_rawDataFileNames);
        // Sort to make sure the files are always sent in in the same order
        sort(t_filesInCombination.begin(), t_filesInCombination.end());

        // Create this guy
        Person t_thisGuy;
        t_thisGuy.name = t_stamp;

        // Add relevant data sets
        t_thisGuy.dataSets;
        DataSetBuilder dsb;
        vector<vector<DataSet>>* t_dataSets = dsb.BuildDataSetFromFiles(t_filesInCombination, t_desiredCombo);
        t_thisGuy.dataSets = &t_dataSets->at(0);

        // Figure out which raw positions belong to this guy
        t_thisGuy.rawPosData = GetRawPositions(t_allPositionFiles, t_stamp);

        m_people.push_back(t_thisGuy);
    }
}
