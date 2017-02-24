#pragma once
class ConfigHandler
{
public:
    static ConfigHandler* Get();

private:
    ConfigHandler();
    ~ConfigHandler();

    static ConfigHandler* m_singleton;
};