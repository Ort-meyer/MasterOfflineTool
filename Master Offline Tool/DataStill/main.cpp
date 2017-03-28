#include <iostream>
#include "DataStill.h"
#include "DataStillManager.h"
#include <ConfigHandler.h>
using namespace std;
int main(int argc, char* argv[])
{
    //Set config
    if (argc > 1)
    {
        cout << argv[1] << endl << argv[2] << endl;
        ConfigHandler* cf = ConfigHandler::Get();
        cf->m_entriesToAvrage = std::stoi(std::string(argv[1]));
        cf->m_entriesToMerge = std::stoi(std::string(argv[2]));
    }
    //DataStill still;
    DataStillManager dataStillManager;
    dataStillManager.FiltrateAllFilesInDirectory("../rawdata/", "../FilteredData/", ConfigHandler::Get()->m_fileEndingRawData, ConfigHandler::Get()->m_fileEndingFiltered);
    //cout << "Hello, world!" << endl;
    //int pause;
    //cin >> pause;
}