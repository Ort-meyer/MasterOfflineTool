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
    Loads the neural network specified and runs it on all filtered data. Its results
    are then used to create a massive text file which gnuplot can use to create a heatmap.
    It also uses the raw positions to get the golden answer for comparisson.
    p_annFilePath is absolut file path to the ann file*/
    //void RunNetworkAndPrepForGnuPlot(std::string p_annFilePath);

private:

    void CreatePeople(std::string p_rawDataPath, std::string p_filteredDataPath);

    struct Person
    {
        std::vector<std::string>* rawPosData;
        std::vector<DataSet>* dataSets;
        std::string name;
    };

    std::vector<Person> m_people;

};

