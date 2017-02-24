#pragma once
class ConfigHandler
{
public:
    static ConfigHandler* Get();

    int m_entriesToAvrage;
    int m_entriesToMerge;

private:
    ConfigHandler();
    ~ConfigHandler();

    static ConfigHandler* m_singleton;
};