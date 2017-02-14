#pragma once
#include <string>
#include <vector>
class DataStill
{
public:
    DataStill();
    ~DataStill();


    ///////////////////Specific methods///////////////
    /**
    Transforms world positions into displacement vectors.
    Formatting is important. Must be as follows:
    Index
    Values[3] // MUST be three input vectors
    Output
    In other words, multiple rows of input will not work.
    Output will be structured in the same way, but with displacement
    vectors instead of world positions.
    Returns pointer to the filtered data as a vector of strings*/
    std::vector<std::string>* FilterDisplacement(const std::vector<std::string>& p_lines);

    /**
    Transforms rotations into displacement vectors. Input is the yaw,
    pitch and roll of the camera of that frame, in radians.
    Output values will two values on each row (we skip roll since it's
    always 0) where each value is now the "radian displacement" a.k.a the
    delta-angle.*/
    std::vector<std::string>* FilterRotations(const std::vector<std::string>& p_lines);


    //////////////////General methods////////////////



    /**
    Searchs through all data entries for p_outputToFlagTo output, then backtracks
    lines and changes output of p_backtrack earlier data entires to p_outputToFlagTo.*/
    void FlagDataOutput(std::vector<std::string>& p_lines, int p_backtrack, int p_outputToFlagTo);

    /**
    Reads through all files specified and normalizes values within and outputs
    results into respective p_filtereFileNames name. Make sure the two parameters
    are mapped. All values will be between -1 and 1
    WARNING! This assume that there is only one data row per file. CBA with scalability.*/
    std::vector<std::vector<std::string>>* NormalizeValues(const std::vector<std::vector<std::string>>& p_filesInLines);

    /**
    Takes p_numToMerge data sets and put them onto the same line.
    Index will be that of the first of the data sets.*/
    std::vector<std::string>* MergeDataOntoSameLine(const std::vector<std::string>& p_lines, int p_numToMerge);

    /**
    General filter method to remove every p_increment:th point of data
    For instance, setting p_increment to 3 will remove every 3rd data point.
    p_rawDataFileName specifies input file, and p_filteredFileName specifies
    output file name*/
    std::vector<std::string>* FilterAwayDataRemove(const std::vector<std::string>& p_lines, int p_increment);

    /**
    General filter method to keep every p_increment:th point of data
    For instance, setting p_increment to 3 will keep every 3rd data point,
    every other data point will be removed
    p_rawDataFileName specifies input file, and p_filteredFileName specifies
    output file name*/
    std::vector<std::string>*  FilterAwayDataKeep(const std::vector<std::string>& p_lines, int p_increment);

    /**
    General filter method that takes p_nuToAvrage numer of values and stores
    as the avrage of those values.
    p_rawDataFileName specifies input file, and p_filteredFileName specifies
    output file name*/
    std::vector<std::string>*  FilterAvrage(const std::vector<std::string>& p_lines, int p_numToAvrage);

    /**
    General filter method that takes p_nuToAvrage numer of values and stores
    as the avrage of those values.
    p_rawDataFileName specifies input file, and p_filteredFileName specifies
    output file name*/
    std::vector<std::string>*  FilterAvrage2(const std::vector<std::string>& p_lines, int p_numToAvrage);

    /**
    General filter for adding a given number of rows of input data together.
    TODO Fix so that only data adjacent to eachother in time is added together.
    */
    std::vector<std::string>* FilterAdd(const std::vector<std::string>& p_lines, const int& p_numToAdd);

    /**
    General filter for adding a given number of rows of input data together.
    TODO Fix so that only data adjacent to eachother in time is added together.
    */
    std::vector<std::string>* FilterAdd2(const std::vector<std::string>& p_lines, const int& p_numToAdd);

private:
    /////////////////////HELP METHODS/////////////////

     /**
     Adds p_increment rows together and returns vector of added values.
     Index and output will be that of the first data row.
     WARNING! Make sure different outputs are NOT mixed when using this
     method.*/
    std::vector<std::string>* AddDataTogether(const std::vector<std::string>& p_lines, int p_increment);

    /**
    Help method used to avrage numbers based on what rows are sent in. This
    is meant to be called sending in the true and false vectors of lines read
    from the data files. Return value is a single string with all avraged rows*/
    std::vector<std::string>* AvrageNumbers(const std::vector<std::string>& p_rows, int p_nrOfdataRowsPerEntry, int p_nrToAvrage);

    /**
    Help Method to MergeDataOntoSameLine. This puts p_numToMerge
    data values on the same row*/
    std::vector<std::string>* MergeVectorDataOntoSameLine(const std::vector<std::string>& p_dataLines, int p_numToMerge, int nrOfRowsPerDataEntry);

    /**
    Splits a data set into two vectors, one with only true output and one with
    only false output. Create vectors before calling this method and send pointers
    as parameters*/
    void SplitDataIntoTrueAndFalseVectors(
        const std::vector<std::string>& p_inData,
        std::vector<std::string>* p_trueVector,
        std::vector<std::string>* p_falseVector,
        int p_nrOfDataRowsPerEntry);

    /**
    Takes all vectors of strings from input parameter and puts together into the
    same vector of strings and returns*/
    std::vector<std::string>* MergeSetsOfLinesIntoSameSet(std::vector<std::vector<std::string>*> p_setsOfLines);

    // Stores full path to directory in which we find raw data
    std::string m_rawDataFilePath;
    std::string m_filteredDataFilePath;
};

