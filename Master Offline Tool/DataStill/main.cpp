#include <iostream>
#include "DataStill.h"
#include "DataStillManager.h"
#include <ConfigHandler.h>
using namespace std;
int main()
{
    //DataStill still;
    DataStillManager dataStillManager;
    dataStillManager.FiltrateAllFilesInDirectory("../rawdata/", "../FilteredData/", ConfigHandler::Get()->m_fileEndingRawData, ConfigHandler::Get()->m_fileEndingFiltered);
    cout << "Hello, world!" << endl;
    int pause;
    cin >> pause;
}