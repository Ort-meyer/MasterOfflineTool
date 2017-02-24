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

}


ConfigHandler::~ConfigHandler()
{
}
