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
}


ConfigHandler::~ConfigHandler()
{
}
