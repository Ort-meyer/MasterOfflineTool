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

    /* This is how they calculate the movement speed
    groundspeed = 180 * maxMovementSpeed * 0.95 * 0.01 * 1.25 * (0.95 + movementSkill * 0.0002)  
    where maxMovementSpeed = 90 when dexterity is 55 (which it was)
    I set movementSkill to 30 as i dont thing we reached any higher
    The total will then come to 194.3 units/sec
    So 194.3 / frameRate gives max displacement per update
    I'm guessing we had about 60 fps
    This does not take jumping and falling into account...
    */
    m_maxDisplacement = 194.3f / 60.0f; 

    // The death movement speed is 400 units/sec
    m_maxDeadDisplacement = 400.0f / 60.0f;
}


ConfigHandler::~ConfigHandler()
{
}
