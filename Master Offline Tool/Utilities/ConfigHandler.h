#pragma once
#include <string>
class ConfigHandler
{
public:
    static ConfigHandler* Get();

    // Filtering attributes
    // Number of entries we want to avrage across
    int m_entriesToAvrage;
    // Number of entries we want merged on the same row
    int m_entriesToMerge;

    // Training configs
    // Whether we want to do the entire permutation thing or not
    bool m_trainAllNetworks;

    // AllnetworkTraining configs
    // How many sets we want to validate on each run
    int m_numValidationSet;

    // File endings
    // Unfiltered data from the game
    std::string m_fileEndingRawData;

    // FilteredData from after our data still has done its work
    std::string m_fileEndingFiltered;

    // Network settings that we save down to know nr of hidden cells and stuff
    std::string m_fileEndingNetSettings;

    // Our actual saved neural network
    std::string m_fileEndingNeuralNet;

private:
    ConfigHandler();
    ~ConfigHandler();

    static ConfigHandler* m_singleton;
};