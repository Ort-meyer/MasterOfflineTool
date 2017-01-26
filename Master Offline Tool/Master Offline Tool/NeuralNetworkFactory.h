#pragma once
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <vector>
class NeuralNetwork;
struct NetworkSettings;
class NeuralNetworkFactory
{
public:
    NeuralNetworkFactory();
    ~NeuralNetworkFactory();
    
    /**
    Sets up different settings the factory needs to know how many different combinations to try
    */
    void SetVariables(const int& p_maxNumberOfHiddenLayers, const int& p_maxNumberOfHiddenCellsPerLayer, const int& p_numberOfHiddenCellIncrease)
    {
        m_maxNumberOfHiddenLayers = p_maxNumberOfHiddenLayers;
        m_maxNumberOfHiddenCellsPerLayer = p_maxNumberOfHiddenCellsPerLayer;
        m_numberOfHiddenCellIncrease = p_numberOfHiddenCellIncrease;
    }
    /**
    Uses the data to form a lot of neural networks with different settings, train them and validate them.
    Validation along with other info about the network is printed to file
    */
    void CreateNewNeuralNetworkCombinationsFromData(FANN::training_data p_trainingData);

    void testFunc();
private:
    void CreateNeuralNetworkCombinations(NetworkSettings* p_netWorkSettings, const int& p_depth);

    std::vector<NeuralNetwork*> m_networks;
    int m_maxNumberOfHiddenLayers;
    int m_maxNumberOfHiddenCellsPerLayer;
    // States how much the hidden cell per layer will increase per loop run
    int m_numberOfHiddenCellIncrease;
};

