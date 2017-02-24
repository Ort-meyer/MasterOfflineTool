#pragma once
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

private:
    ConfigHandler();
    ~ConfigHandler();

    static ConfigHandler* m_singleton;
};