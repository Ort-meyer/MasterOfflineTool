#include "GnuPlotter.h"
#include <FileHandler.h>
#include "DataSetBuilder.h"
#include <algorithm>


using namespace FANN;
using namespace std;
GnuPlotter::GnuPlotter()
{
    CreatePeople("../RawData", "../FilteredData");
}


GnuPlotter::~GnuPlotter()
{
}


////// Shamelessly stolen and put here. We should really refactor shit...////////
std::vector<std::string> GetAllFilesWithStampAndShrinkList(const std::string& p_stamp, std::vector<std::string>& o_files)
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


void GnuPlotter::CreatePeople(std::string p_rawDataPath, std::string p_filteredDataPath)
{

    /// Find raw position file names
    // Get all raw data
    vector<string> allRawFileNames = FileHandler::GetAllFileNames(p_rawDataPath, "debug"); // remember to chance to .log file format!
    vector<string> allPositionFiles;
    // Filter so we only have those with position
    for (size_t i = 0; i < allRawFileNames.size(); i++)
    {
        size_t fileNameStart = allRawFileNames[i].find_last_of("/") + 1;
        if (allRawFileNames[i].substr(fileNameStart, 3) == "Pos")
        {
            // We found a position file!
            allPositionFiles.push_back(allRawFileNames.at(i));
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
        Person thisGuy;
        thisGuy.name = t_stamp;

        // Add relevant data sets
        thisGuy.dataSets;
        DataSetBuilder dsb;
        vector<vector<DataSet>>* t_dataSets = dsb.BuildDataSetFromFiles(t_filesInCombination, t_desiredCombo);
        thisGuy.dataSets = &t_dataSets->at(0);

        // Figure out which raw positions belong to this guy
        

        m_people.push_back(thisGuy);
    }
}
