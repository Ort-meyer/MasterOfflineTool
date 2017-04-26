#include "FileCombiner.h"
#include "GnuPlotter.h"

#include <ConfigHandler.h>
#include <iostream>
int main(int argc, char* argv[])
{
    ConfigHandler* cf = ConfigHandler::Get();
    if (argc > 1)
    {
        cf->m_combinationToUse = argv[1];

        cf->m_hiddenLayers = std::stoi(std::string(argv[2]));
        for (size_t i = 0; i < 7; i++)
        {
            cf->m_hiddenCells[i] = std::stoi(std::string(argv[3 + i]));
        }

        cf->m_entriesToAvrage = std::stoi(std::string(argv[10]));
        cf->m_entriesToMerge = std::stoi(std::string(argv[11]));

    }

    FileCombiner fc;
    //GnuPlotter plotter;
}