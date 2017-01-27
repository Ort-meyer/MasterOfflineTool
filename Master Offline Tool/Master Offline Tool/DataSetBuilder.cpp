#include "DataSetBuilder.h"
#include "NeuralNetworkFactory.h"


DataSetBuilder::DataSetBuilder()
{
    m_factory = new NeuralNetworkFactory();
    m_factory->SetVariables(3, 3, 1,0.5f,0.5f,0.5f,10);
    // We let the network be responsible for deleting the training data after it is done
    FANN::training_data* data = new FANN::training_data();
    data->read_train_from_file("CalcTrainingData.data");
    m_factory->CreateNewNeuralNetworkCombinationsFromData(data);
}


DataSetBuilder::~DataSetBuilder()
{
}

void DataSetBuilder::BuildDataSetFromFiles(std::vector<std::string> p_fileNames)
{
}
