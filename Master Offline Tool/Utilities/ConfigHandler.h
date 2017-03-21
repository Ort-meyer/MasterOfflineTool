#pragma once
#include <string>
enum class NetworkCreationType
{
    CreateAllCombinations,
    BuildFromBaseline,
    CreateOneSpecific
};

enum class LogLevel
{
    // Make sure it goes from low log level to high log level so we can do > checks
    Silent,
    Progress,
    Verbose
};

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

    // Training amount info
    int m_numberOfEpochs;
    int m_reportRate;
    float m_errorAcceptance;

    // How we want to train
    NetworkCreationType m_creationType;

    // How much info we want to print out
    LogLevel m_logLevel;

    // How many threads we will run at once
    int m_maxNetworkThreads;

    // How much off the network can be and we still count it as success
    float m_networkPredictionTolerance;

    // Could be hardcoded into DataStillManager. Describes the biggest displacement possible with the settings used during playtest
    float m_maxDisplacement;

private:
    ConfigHandler();
    ~ConfigHandler();

    static ConfigHandler* m_singleton;
};