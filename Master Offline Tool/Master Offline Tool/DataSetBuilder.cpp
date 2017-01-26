#include "DataSetBuilder.h"
#include "NeuralNetworkFactory.h"


DataSetBuilder::DataSetBuilder()
{
    m_factory = new NeuralNetworkFactory();
    m_factory->SetVariables(3, 5, 1);
    m_factory->testFunc();
}


DataSetBuilder::~DataSetBuilder()
{
}

void DataSetBuilder::BuildDataSetFromFiles(std::vector<std::string> p_fileNames)
{
}
