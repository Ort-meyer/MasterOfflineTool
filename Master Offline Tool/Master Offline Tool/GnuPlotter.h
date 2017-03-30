#pragma once
#include <string>
#include <vector>

#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include "DataSetBuilder.h"

class GnuPlotter
{
public:
    GnuPlotter();
    ~GnuPlotter();

    
    /**
    Lookes through the lines for the given index and fins relevant positions and builds 
    a list which is then returned.*/
    std::vector<std::string>* ReverseEngineerPositions(const std::vector<std::string>& p_lines, const int& p_index);

    void PrepGoldenDataForGnuPlot();

    /**
    Loads the neural network specified and runs it on all filtered data. Its results
    are then used to create a massive text file which gnuplot can use to create a heatmap.
    It also uses the raw positions to get the golden answer for comparisson.
    p_annFilePath is absolut file path to the ann file*/
    void RunNetworkAndPrepForGnuPlot(std::string p_annFilePath);

private:

    // Shamelessly nicked method from konrad. Does what it says it does
    std::vector<std::string> GetAllFilesWithStampAndShrinkList(const std::string & p_stamp, std::vector<std::string>& o_files);
    /**
    Searches through all raw position files to find position data for the tag specified*/
    std::vector<std::string>* GetRawPositions(const std::vector<std::string> p_posFileNames, const std::string & p_tag);

    void CreatePeople(std::string p_rawDataPath, std::string p_filteredDataPath);

    struct Person
    {
        std::vector<std::string>* rawPosData;
        std::vector<DataSet>* dataSets;
        std::string name;
    };

    std::vector<Person> m_people;

    int currentValidationSet;

};

