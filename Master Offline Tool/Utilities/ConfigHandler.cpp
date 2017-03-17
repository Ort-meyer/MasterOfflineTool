#include "ConfigHandler.h"

ConfigHandler* ConfigHandler::m_singleton = nullptr;


ConfigHandler* ConfigHandler::Get()
{
    if (m_singleton == nullptr)
        m_singleton = new ConfigHandler();
    return m_singleton;
}

ConfigHandler::ConfigHandler()
{
    m_entriesToAvrage = 60;
    m_entriesToMerge = 5;

    m_trainAllNetworks = false;

    m_numValidationSet = 1;

    m_fileEndingFiltered = "filteredData";
    m_fileEndingRawData = "rawData";
    m_fileEndingNetSettings = "netSettings";
    m_fileEndingNeuralNet = "ann";

    m_numberOfEpochs = 1000;
    m_reportRate = 100;
    m_errorAcceptance = 0.0001f;

    m_creationType = NetworkCreationType::CreateOneSpecific;

    m_logLevel = LogLevel::Verbose;

    m_maxNetworkThreads = 1;

    m_networkPredictionTolerance = 0.2f;
}


ConfigHandler::~ConfigHandler()
{
}
