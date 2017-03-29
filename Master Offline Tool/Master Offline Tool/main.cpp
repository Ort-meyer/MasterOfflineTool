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
        for (size_t i = 0; i < 5; i++)
        {
            cf->m_hiddenCells[i] = std::stoi(std::string(argv[3 + i]));
        }
    }

    FileCombiner fc;
    GnuPlotter plotter;
}