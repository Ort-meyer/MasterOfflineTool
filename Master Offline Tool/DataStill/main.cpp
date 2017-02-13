#include <iostream>
#include "DataStill.h"
#include "DataStillManager.h"
using namespace std;
int main()
{
    //DataStill still;
    DataStillManager dataStillManager;
    dataStillManager.FiltrateAllFilesInDirectory("../rawdata/", "../FilteredData/", "debug", "filteredData");
    cout << "Hello, world!" << endl;
    int pause;
    cin >> pause;
}