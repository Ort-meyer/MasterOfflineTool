#include "DataStill.h"
#include <fstream>
#include <iostream>
#include <iterator>
#include <sstream>
#include <GLM\glm.hpp>
#include <FileHandler.h>
#include <Windows.h>

#include "KeyMaskInterpreter.h"
using namespace glm;
using namespace std;
DataStill::DataStill()
{
}


DataStill::~DataStill()
{
}

//////// SPECIFIC MEHODS//////

vector<string>* DataStill::FilterDisplacement(const std::vector<std::string>& p_lines)
{
    // We know this. Says so in comments
    int numLinesPerDataEntry = 3;
    int numInputs = 3;
    // Set start values
    istringstream in(p_lines.at(1));
    vec3 t_currentPos;
    for (size_t j = 0; j < numInputs; j++)
    {
        in >> t_currentPos[j];
    }
    vector<string>* r_lines = new vector<string>();
    // Iterate over all positions and generate displacements
    for (size_t i = numLinesPerDataEntry; i < p_lines.size() - 2; i += numLinesPerDataEntry)
    {

        string index = p_lines.at(i - numLinesPerDataEntry);
        string output = p_lines.at(i + 2 - numLinesPerDataEntry);

        // Read next position into vector
        istringstream in(p_lines.at(i + 1));
        vec3 t_newPos;
        for (size_t j = 0; j < numInputs; j++)
        {
            in >> t_newPos[j];
        }
        vec3 displacement = t_newPos - t_currentPos;
        t_currentPos = t_newPos;
        // Write displacement to file right away
        r_lines->push_back(index);
        stringstream t_stringStream;
        t_stringStream << displacement.x << " " << displacement.y << " " << displacement.x << " ";
        r_lines->push_back(t_stringStream.str());
        r_lines->push_back(output);
    }
    return r_lines;
}

std::vector<std::string>* DataStill::FilterRotations(const std::vector<std::string>& p_lines)
{
    // Start by filtering displacements
    vector<string>* r_lines = new vector<string>();
    r_lines = FilterDisplacement(p_lines);
    // Now remove roll values
    for (size_t i = 1; i < r_lines->size() - 1; i += 3)
    {
        r_lines->at(i) = r_lines->at(i).substr(0, r_lines->at(i).size() - 2);
    }
    return r_lines;
}

/////// GENERAL METODS///////

void DataStill::FlagDataOutput(std::vector<std::string>& o_lines, int p_backtrack, int p_outputToFlagTo)
{
    string t_outputToFlagTo = to_string((long double)p_outputToFlagTo);
    // Start at an output 
    for (size_t outputIter = 2; outputIter < o_lines.size(); outputIter += 3)
    {
        // Check if this output matches what we're looking for
        int res = strcmp(o_lines.at(outputIter).c_str(), t_outputToFlagTo.c_str()); // The cast here feels REALLY risky
        if (res == 0)
        {
            // Walk backwards and change output
            int lastLineToRead = outputIter - (p_backtrack + 1) * 3;
            for (int j = outputIter - 3; j > lastLineToRead && j >= 2; j -= 3)
            {
                o_lines.at(j) = t_outputToFlagTo;
            }
        }
    }
}

std::vector<std::vector<std::string>>* DataStill::NormalizeValues(const std::vector<vector<std::string>>& p_filesInLines)
{
    // Find largest value
    // Needs to be vector in case of multiple data rows per data entry
    float t_maxValue = 0;
    for (size_t i = 0; i < p_filesInLines.size(); i++)// Per file
    {
        // We now have one list of lines
        // Iterate through the list. add +2 to iterator for index and output
        for (size_t j = 0; j < p_filesInLines.at(i).size() - 2; j += 3)
        {
            // We're at the start of one data entry
            // Figure out how many inputs
            string line = p_filesInLines.at(i).at(j + 1);
            istringstream in(line);
            int inputs = std::distance(istream_iterator<string>(istringstream(line) >> ws), istream_iterator<string>());
            // See if there's a bigger number for this kind of value
            for (size_t k = 0; k < inputs; k++)
            {
                float thisValue;
                in >> thisValue;
                thisValue = abs(thisValue);
                if (t_maxValue < thisValue)
                    t_maxValue = thisValue;
            }
        }
    }

    vector<vector<string>>* r_filesInLines = new vector<vector<string>>();
    r_filesInLines->resize(p_filesInLines.size());
    // Now divide every value with this value and write to file
    for (size_t i = 0; i < p_filesInLines.size(); i++)// Per file
    {
        // Iterate through the list. add +2 to iterator for index and output
        for (size_t j = 0; j < p_filesInLines.at(i).size() - 2; j += 3)
        {
            // We're at the start of one data entry
            string index = p_filesInLines.at(i).at(j);
            string output = p_filesInLines.at(i).at(j + 2);
            // Figure out how many inputs
            string line = p_filesInLines.at(i).at(j + 1);
            istringstream in(line);
            int inputs = std::distance(istream_iterator<string>(istringstream(line) >> ws), istream_iterator<string>());
            // See if there's a bigger number for this kind of value
            stringstream t_stringstream;
            for (size_t k = 0; k < inputs; k++)
            {
                float thisValue;
                in >> thisValue;
                thisValue /= t_maxValue;
                t_stringstream << thisValue << " ";
            }
            // Write values
            r_filesInLines->at(i).push_back(index);
            r_filesInLines->at(i).push_back(t_stringstream.str());
            r_filesInLines->at(i).push_back(output);
        }
    }
    return r_filesInLines;
}

std::vector<std::string>* DataStill::MergeDataOntoSameLine(const std::vector<std::string>& p_lines, int p_numToMerge)
{
    int nrOfRowsPerDataEntry = 3; // Will have to be tweaked!
    int strideToData = 1; //How many rows are skipped before data is read, for each data point
    int nrOfDataPoints = p_lines.size() / nrOfRowsPerDataEntry;

    // Read into two vectors: one with true, one with false. Makes things easier
    vector<string> trueList;
    vector<string> falseList;
    SplitDataIntoTrueAndFalseVectors(p_lines, &trueList, &falseList, nrOfRowsPerDataEntry);

    vector<string>* trueMerged = MergeVectorDataOntoSameLine(trueList, p_numToMerge, nrOfRowsPerDataEntry);
    vector<string>* falseMerged = MergeVectorDataOntoSameLine(falseList, p_numToMerge, nrOfRowsPerDataEntry);
    vector<string>* r_lines = new vector<string>();
    // Merge true and false lists before returning
    r_lines = trueMerged;
    r_lines->insert(r_lines->end(), falseMerged->begin(), falseMerged->end());
    return r_lines;
}

std::vector<std::string>* DataStill::FilterAwayDataRemove(const std::vector<string>& p_lines, int p_increment)
{
    int nrOfRowsPerdataSet = 3; // Will have to be tweaked!
    int size = p_lines.size() / nrOfRowsPerdataSet;
    vector<string>* r_lines = new vector<string>();
    for (size_t i = 1; i < size + 1; i++)
    {
        // Skip every p_increment:th write
        if (i%p_increment != 0)
        {
            int derp = 5;
            for (size_t j = 0; j < nrOfRowsPerdataSet; j++)
            {
                r_lines->push_back(p_lines.at((i - 1)*nrOfRowsPerdataSet + j));
            }
        }
    }
    return r_lines;
}

std::vector<std::string>*  DataStill::FilterAwayDataKeep(const std::vector<string>& p_lines, int p_increment)
{
    int nrOfRowsPerdataSet = 3; // Will have to be tweaked!
    int size = p_lines.size() / nrOfRowsPerdataSet;
    vector<string>* r_lines = new vector<string>();
    for (size_t i = 1; i < size + 1; i++)
    {
        // Skip every p_increment:th write
        if (i%p_increment == 0)
        {
            int derp = 5;
            for (size_t j = 0; j < nrOfRowsPerdataSet; j++)
            {
                r_lines->push_back(p_lines.at((i - 1)*nrOfRowsPerdataSet + j));
            }
        }
    }
    return r_lines;
}

std::vector<std::string>*  DataStill::FilterAvrage(const std::vector<string>& p_lines, int p_numToAvrage)
{

    int nrOfRowsPerDataEntry = 3; // Will have to be tweaked!
    int strideToData = 1; //How many rows are skipped before data is read, for each data point
    int nrOfDataPoints = p_lines.size() / nrOfRowsPerDataEntry;
    // Read into two vectors: one with true, one with false. Makes things easier
    vector<string> trueList;
    vector<string> falseList;
    SplitDataIntoTrueAndFalseVectors(p_lines, &trueList, &falseList, nrOfRowsPerDataEntry);
    // Use our amazing help method
    vector<string>* trueAvrages = AvrageNumbers(trueList, nrOfRowsPerDataEntry, p_numToAvrage);
    vector<string>* falseAvrages = AvrageNumbers(falseList, nrOfRowsPerDataEntry, p_numToAvrage);
    vector<string>* r_lines = new vector<string>();
    vector<vector<string>*> t_setsOfLines;
    t_setsOfLines.push_back(trueAvrages);
    t_setsOfLines.push_back(falseAvrages);
    // Merge true and false lists before returning
    r_lines = MergeSetsOfLinesIntoSameSet(t_setsOfLines);
    r_lines->insert(r_lines->end(), falseAvrages->begin(), falseAvrages->end());
    return r_lines;
}

std::vector<std::string>* DataStill::FilterAvrage2(const std::vector<std::string>& p_lines, int p_numToAvrage)
{
    vector<string>* r_lines = new vector<string>();
    // Bool that tracks if we're currently searching for new data entries of same output to avrage
    bool t_searching = false;
    // Keeps track of how many values each data row has
    const int numValues = std::distance(istream_iterator<string>(istringstream(p_lines.at(1)) >> ws), istream_iterator<string>());
    // Stores the current values to be avraged
    vector<float> t_currentValues;
    t_currentValues.resize(numValues);
    // Counts how many to avrage we've found
    int t_numToAvrageCount = 0;
    // Start looping over all data entries
    for (size_t dataEntryStart = 0; dataEntryStart < p_lines.size(); dataEntryStart += 3)
    {
        string t_targetIndex;
        string t_targetOutput;
        // Check if we're looking for new values
        if (!t_searching)
        {
            // We weren't! Set a target
            t_targetIndex = p_lines.at(dataEntryStart);
            t_targetOutput = p_lines.at(dataEntryStart + 2);
            t_searching = true;
        }
        // Check if next output is the same as what we're looking for
        string t_currentOutput = p_lines.at(dataEntryStart + 2);
        if (t_targetOutput.compare(t_currentOutput) == 0)
        {
            // Next input is the same. Add together values
            istringstream in(p_lines.at(dataEntryStart + 1));
            for (size_t j = 0; j < numValues; j++)
            {
                float thisValue;
                in >> thisValue;
                t_currentValues.at(j) += thisValue;
            }
            t_numToAvrageCount++;

            // Check if we've found our p_numToAvrage data entries
            if (t_numToAvrageCount == p_numToAvrage)
            {
                // Divide all values to get avrage
                stringstream ss;
                for (size_t j = 0; j < numValues; j++)
                {
                    ss << t_currentValues.at(j) / p_numToAvrage << " ";
                }
                // Store output data entry, now with avraged number
                r_lines->push_back(t_targetIndex);
                r_lines->push_back(ss.str());
                r_lines->push_back(t_targetOutput);

                // Cleanup for next round
                t_currentValues.clear();
                t_currentValues.resize(numValues);
                t_numToAvrageCount = 0;
                t_searching = false;
            }
        }
        else
        {
            t_currentValues.clear();
            t_currentValues.resize(numValues);
            t_searching = false;
        }
    }
    return r_lines;
}

std::vector<std::string>* DataStill::FilterAdd(const std::vector<std::string>& p_lines, const int & p_numToAdd)
{
    int nrOfRowsPerDataEntry = 3; // Will have to be tweaked!
    int strideToData = 1; //How many rows are skipped before data is read, for each data point
    int nrOfDataPoints = p_lines.size() / nrOfRowsPerDataEntry;
    // Read into two vectors: one with true, one with false. Makes things easier
    vector<string> trueList;
    vector<string> falseList;
    SplitDataIntoTrueAndFalseVectors(p_lines, &trueList, &falseList, nrOfRowsPerDataEntry);
    // Use our amazing help method
    vector<string>* trueAdded = AddDataTogether(trueList, p_numToAdd);
    vector<string>* falseAdded = AddDataTogether(falseList, p_numToAdd);
    vector<string>* r_lines = new vector<string>();
    // Merge true and false lists before returning
    r_lines = trueAdded;
    r_lines->insert(r_lines->end(), falseAdded->begin(), falseAdded->end());
    return r_lines;
}

/////// HELP METHODS///////

std::vector<std::string>* DataStill::AddDataTogether(const std::vector<std::string>& p_lines, int p_increment)
{
    vector<string>* r_lines = new vector<string>();
    int numInputs = std::distance(istream_iterator<string>(istringstream(p_lines.at(1)) >> ws), istream_iterator<string>());
    for (size_t batchIter = 1; batchIter < p_lines.size(); batchIter += 3 * p_increment)
    {
        // Check if next batch will go out of scope
        if (batchIter + p_increment * 3 > p_lines.size())
            break;
        string index = p_lines.at(batchIter - 1);
        string output = p_lines.at(batchIter + 1);
        vector<float> thisRow;
        thisRow.resize(numInputs);
        // Go through the rows of this batch
        for (size_t rowIter = batchIter; rowIter < batchIter + p_increment * 3; rowIter += 3)
        {
            // Add together values for each input cell
            istringstream in(p_lines.at(rowIter));
            for (size_t dataValueIter = 0; dataValueIter < numInputs; dataValueIter++)
            {
                float thisValue;
                in >> thisValue;
                thisRow.at(dataValueIter) += thisValue;
            }
        }
        // Write stuff back
        r_lines->push_back(index);
        stringstream ss;
        for (size_t floats = 0; floats < thisRow.size(); floats++)
        {
            ss << thisRow.at(floats) << " ";
        }
        r_lines->push_back(ss.str());
        r_lines->push_back(output);

    }
    return r_lines;
}

std::vector<std::string>* DataStill::AvrageNumbers(const std::vector<string>& p_rows, int p_nrOfdataRowsPerEntry, int p_nrToAvrage)
{
    int nrOfValueRows = p_nrOfdataRowsPerEntry - 2; // Hard-coded. Index is one row, output is one row
    // Figure out how many inputs each data row stores
    vector<int> nrOfInputs;
    // We use only the first data entry, since all data entries have to be foramtted the same
    for (size_t i = 0; i < nrOfValueRows && i < p_rows.size(); i++)
    {
        int inputs = std::distance(istream_iterator<string>(istringstream(p_rows.at(i + 1)) >> ws), istream_iterator<string>());
        nrOfInputs.push_back(inputs);
    }

    vector<string>* r_lines = new vector<string>();
    // Now we avrage the values in each list
    for (size_t i = 0; i < p_rows.size() / p_nrToAvrage; i++)
    {
        // Per batch of data entries
        /*
        Inner vector: contains values of one row
        Middle vector: contains all rows with values within one data entry
        Outer vector: contains all data entries that will be avraged*/
        string index;
        string output;
        bool readIndexAndOutput = false;
        vector<vector<vector<float>>> allDataEntries;
        for (size_t j = 0; j < p_nrToAvrage; j++)
        {
            vector<vector<float>> dataEntriesOfthisSet;
            for (size_t k = 0; k < nrOfValueRows; k++)
            {
                // Number of inputs for this row
                int inputs = nrOfInputs.at(k);
                vector<float> valuesOfThisRow;
                int rowStartEntry = i*p_nrOfdataRowsPerEntry*p_nrToAvrage + j*p_nrOfdataRowsPerEntry + k;
                if (rowStartEntry >= p_rows.size())
                {
                    break;
                }
                // Ugly way of doing this, but it works
                if (!readIndexAndOutput)
                {
                    index = p_rows.at(rowStartEntry);
                    output = p_rows.at(rowStartEntry + p_nrOfdataRowsPerEntry - 1);
                    readIndexAndOutput = true;
                }
                istringstream in(p_rows.at(rowStartEntry + 1));
                for (size_t l = 0; l < inputs; l++)
                {
                    float thisValue;
                    in >> thisValue;
                    valuesOfThisRow.push_back(thisValue);
                }
                dataEntriesOfthisSet.push_back(valuesOfThisRow);
            }
            if (dataEntriesOfthisSet.size() > 0)
                allDataEntries.push_back(dataEntriesOfthisSet);
        }



        // Now we avrage the numbers
        /**
        Each inner vector is one row of values.
        Each outer vector contains one data set*/
        // Check if there's numbers to avrage. I hate these ugly checks..
        if (allDataEntries.size() > 0 && allDataEntries.size() % p_nrToAvrage == 0)
        {
            // Make room for our values
            vector<vector<float>> avrages;
            if (allDataEntries.size() > 0)
            {
                avrages.resize(allDataEntries.at(0).size());
                for (size_t j = 0; j < allDataEntries.at(0).size(); j++)
                {
                    avrages.at(j).resize(allDataEntries.at(0).at(j).size());
                }
            }

            // Get totals
            for (size_t j = 0; j < allDataEntries.size(); j++)
            {
                for (size_t k = 0; k < allDataEntries.at(j).size(); k++)
                {
                    for (size_t l = 0; l < allDataEntries.at(j).at(k).size(); l++)
                    {
                        avrages.at(k).at(l) += allDataEntries.at(j).at(k).at(l);
                    }
                }
            }

            // Write results to return string
            r_lines->push_back(index);
            // Now actually avrage
            for (size_t j = 0; j < avrages.size(); j++)
            {
                stringstream t_stringStream;
                for (size_t k = 0; k < avrages.at(j).size(); k++)
                {
                    avrages.at(j).at(k) /= p_nrToAvrage;
                    // Write value
                    t_stringStream << avrages.at(j).at(k) << " ";
                }
                r_lines->push_back(t_stringStream.str());
            }
            r_lines->push_back(output);
        }
    }
    return r_lines;
}

std::vector<std::string>* DataStill::MergeVectorDataOntoSameLine(const std::vector<string>& p_dataLines, int p_numToMerge, int nrOfRowsPerDataEntry)
{
    vector<string>* r_lines = new vector<string>();
    int t_nrOfdataRows = nrOfRowsPerDataEntry - 2;
    // Iterate over each chunk of data entries we want merged
    for (size_t i = 0; i < p_dataLines.size(); i += p_numToMerge*nrOfRowsPerDataEntry)
    {
        string index = p_dataLines.at(i);
        // Stride past data rows to find output
        string output = p_dataLines.at(i + t_nrOfdataRows + 1);

        vector<string> t_mergedDataRows;
        t_mergedDataRows.resize(t_nrOfdataRows);
        bool outOfScope = false;
        // Iterate over each data entry
        for (size_t k = 0; k < p_numToMerge; k++)
        {
            int startRow = i + 1 + k * nrOfRowsPerDataEntry;
            // Check if building a merged data entry would put us out of scope
            if (startRow + t_nrOfdataRows >= p_dataLines.size())
            {
                outOfScope = true;
            }
            else
            {
                // Iterate over the data rows of each entry
                for (size_t j = 0; j < t_nrOfdataRows; j++)
                {
                    string line = p_dataLines.at(startRow + j);
                    t_mergedDataRows.at(j).append(line);
                }
            }
        }
        // Write the merged data entry into return string
        if (!outOfScope)
        {
            r_lines->push_back(index);
            for (size_t j = 0; j < t_nrOfdataRows; j++)
            {
                r_lines->push_back(t_mergedDataRows.at(j));
            }
            //cout << output << endl;
            r_lines->push_back(output);
        }
    }
    return r_lines;
}

void DataStill::SplitDataIntoTrueAndFalseVectors(
    const std::vector<std::string>& p_inData,
    std::vector<std::string>* p_trueVector,
    std::vector<std::string>* p_falseVector,
    int p_nrOfDataRowsPerEntry)
{
    int nrOfDataPoints = p_inData.size() / p_nrOfDataRowsPerEntry;
    for (size_t i = 0; i < nrOfDataPoints; i++)
    {
        // This is haxy. It relies on the output for a raw data set to be the last row in a data entry
        if (!strcmp(p_inData.at((i + 1)*p_nrOfDataRowsPerEntry - 1).c_str(), "0"))
        {
            // Write the rows for this data entry
            for (size_t j = 0; j < p_nrOfDataRowsPerEntry; j++)
            {
                p_falseVector->push_back(p_inData.at(i*p_nrOfDataRowsPerEntry + j));
            }
        }
        else
        {
            for (size_t j = 0; j < p_nrOfDataRowsPerEntry; j++)
            {
                p_trueVector->push_back(p_inData.at(i*p_nrOfDataRowsPerEntry + j));
            }
        }
    }
}

std::vector<std::string>* DataStill::MergeSetsOfLinesIntoSameSet(std::vector<std::vector<std::string>*> p_setsOfLines)
{
    vector<string>* r_lines = new vector<string>();
    for (size_t i = 0; i < p_setsOfLines.size(); i++)
    {
        r_lines->insert(r_lines->end(), p_setsOfLines.at(i)->begin(), p_setsOfLines.at(i)->end());
    }
    return r_lines;
}

