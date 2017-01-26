#pragma once
#include <vector>
#include <string>
class NeuralNetworkFactory;
class DataSetBuilder
{
public:
    DataSetBuilder();
    ~DataSetBuilder();

    void BuildDataSetFromFiles(std::vector<std::string> p_fileNames);
private:
    NeuralNetworkFactory* m_factory;
};

