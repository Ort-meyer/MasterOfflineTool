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

    m_numValidationSet = 1;

    m_fileEndingFiltered = "filteredData";
    m_fileEndingRawData = "rawData";
    m_fileEndingNetSettings = "netSettings";
    m_fileEndingNeuralNet = "ann";

    m_numberOfEpochs = 1000;
    m_reportRate = 100;
    m_errorAcceptance = 0.000001f;

    m_creationType = NetworkCreationType::CreateOneSpecific;

    int settingToTest = (int)FANNSettingToTest::TestHiddenCells + (int)FANNSettingToTest::TestOutputSteepness
        + (int)FANNSettingToTest::TestHiddenLayers + (int)FANNSettingToTest::TestHiddenSteepness;
    m_settingsToTest = (FANNSettingToTest)settingToTest;

    m_logLevel = LogLevel::Verbose;

    m_maxNetworkThreads = 10;

    m_networkPredictionTolerance = 0.2f;

    m_deterministicWeights = true;
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

    m_performeRetraining = false;

    ////// SETTINGS FOR TRAINING MANY NETWORKS ////////
    m_maxNumberOfHiddenLayers = 5;
    m_numberOfHiddenLayersStart = 5;
    m_maxNumberOfHiddenCellsPerLayer = 100;
    // States how much the hidden cell per layer will increase per loop run
    m_numberOfHiddenCellIncrement = 100;
    // States how fast the learningrate will increment, by how much, each loop. Lower numbers creates more combinations
    // learningrate goes from 0-1
    m_learningRateIncrement = 0.2;
    // States how fast the hiddenSteepness will increment, by how much, each loop. Lower numbers creates more combinations
    // hiddenSteepness goes from 0-1
    m_hiddenSteepnessIncrement = 0.3;
    // States how fast the outputSteepness will increment, by how much, each loop. Lower numbers creates more combinations
    // outputSteepness goes from 0-1
    m_outputSteepnessIncrement = 0.3;


}


ConfigHandler::~ConfigHandler()
{
}
