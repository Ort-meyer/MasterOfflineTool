#include "NeuralNetwork.h"
#include <iostream>
#include <vector>
#include <FileHandler.h>
#include <ConfigHandler.h>
using namespace std;


int training_callback(FANN::neural_net &net, FANN::training_data &train,
    unsigned int max_epochs, unsigned int epochs_between_reports,
    float desired_error, unsigned int epochs, void *user_data)
{
    BestEpoch* bestEpoch = static_cast<BestEpoch*>(user_data);
	// Add mse into total list
	bestEpoch->mseList.push_back(net.get_MSE());
	// Check for new top and bottom and update as necessary
	if (net.get_MSE() > bestEpoch->top)
		bestEpoch->top = net.get_MSE();
	if (net.get_MSE() < bestEpoch->bottom)
		bestEpoch->bottom = net.get_MSE();
	//Update difference to always be first 
	//bestEpoch->difference = bestEpoch->mseList.at(0) - bestEpoch->mseList.at(bestEpoch->mseList.size() - 1);
	bestEpoch->difference = bestEpoch->top - bestEpoch->bottom;
	
    if (bestEpoch->bestMSE > net.get_MSE())
    {
        bestEpoch->bestMSE = net.get_MSE();
        bestEpoch->bestEpoch = epochs;
    }
    if (ConfigHandler::Get()->m_logLevel == LogLevel::Verbose)
    {
        cout << "Epochs     " << epochs << ". "
            << "Current Error: " << net.get_MSE() <<
            " Hidden Activation function: " << net.get_activation_function(1, 0)
            << " Output Activation function: " << net.get_activation_function(net.get_num_layers() - 1, 0) << endl;
    }
    return 0;
}

NeuralNetwork::NeuralNetwork()
{
    m_bestEpoch.bestMSE = 1000;
    m_bestEpoch.bestEpoch = -1;
	m_bestEpoch.top = -10000;
	m_bestEpoch.bottom = 10000;
    m_MSEExcaptableDifference = 0.001f;
}


NeuralNetwork::~NeuralNetwork()
{
    delete m_networkSettings.trainingData;
    delete m_networkSettings.validationData;
    // TODO we need to delete hidden cells somewhere...
}

void NeuralNetwork::ValidateOnFile()
{
    //m_data.read_train_from_file(m_trainingDataFilename);
    //m_net.test_data(m_data);
    //std::cout << "Mean Square Error according to FANN: " << m_net.get_MSE() << endl;
    //float fullError = 0;
    //float* input = *m_data.get_input();
    //float* output = *m_data.get_output();
    //int length = m_data.length_train_data();
    //for (size_t i = 0; i < length; i++)
    //{
  //     // Needs to make this work for several output cells too
    //	float* netOutput = m_net.run(&input[i * m_networkSettings.inputCells]);
    //	std::cout << "Net: " << *netOutput << " Acctual: " << output[i] << endl;
    //	fullError += abs(abs(*netOutput) - abs(output[i]));
    //}
    //std::cout << "mean error: " << fullError / static_cast<float>(length) << endl;
}

void NeuralNetwork::ValidateNetwork()
{
    if (m_networkSettings.validationData != nullptr)
    {
        m_net.test_data(*m_networkSettings.validationData);
        if (ConfigHandler::Get()->m_logLevel == LogLevel::Verbose)
        {
            std::cout << "Mean Square Error according to FANN: " << m_net.get_MSE() << endl;
        }
        m_networkSettings.mse = m_net.get_MSE();
        float fullError = 0;
        float* input = *m_networkSettings.validationData->get_input();
        float* output = *m_networkSettings.validationData->get_output();
        int length = m_networkSettings.validationData->length_train_data();
        int successful = 0;
        for (size_t i = 0; i < length; i++)
        {
            // Needs to make this work for several output cells too, Or do WE?
            float* netOutput = m_net.run(&input[i * m_networkSettings.inputCells]);
            float diff = abs(*netOutput - output[i]);
            float tolerance = 0.1;
            if (diff < tolerance)
                successful++;
            //std::cout << "Net: " << *netOutput << " Acctual: " << output[i] << endl;
            fullError += abs(abs(*netOutput) - abs(output[i]));
        }
        if (ConfigHandler::Get()->m_logLevel == LogLevel::Verbose)
        {
            cout << "number of successful: " << successful << " Out of a total of: " << length << endl;
            cout << "Successrate: " << ((float)successful / (float)length) * 100 << "%" << endl;
            std::cout << "mean error: " << fullError / static_cast<float>(length) << endl;
        }
        m_networkSettings.correctPercentile = ((float)successful / (float)length) * 100;
        m_networkSettings.meanError = fullError / static_cast<float>(length);
    }
}

void NeuralNetwork::TrainAndValidateNetwork(const int& p_epochs, const int& p_reportRate, const float& p_errorAcceptance)
{
    // We try one train to ensure that we dont get NAN on the MSE
    m_net.train_epoch(*m_networkSettings.trainingData);
    // If we get NAN it means something is probably wrong with the net settings
    if (isnan( m_net.get_MSE()))
    {
        return; // No need to do more, things are fucked up...
    }
    m_networkSettings.didRetrain = false;
    m_firstTrainError = MAXINT;
    // Start by training with one amount of epochs
    TrainOnData(p_epochs, p_reportRate, p_errorAcceptance);
    // If we, after training, find out that the best MSE according to FANN is that much lower than the one we got we retrain
    if (m_bestEpoch.bestMSE < m_net.get_MSE() - m_MSEExcaptableDifference)
    {
        // We still need to validate before we retrain to get the validation error
        ValidateNetwork();
        m_firstTrainError = m_networkSettings.meanError;
        InitializeWeights();
        TrainOnData(m_bestEpoch.bestEpoch, 0, p_errorAcceptance);
        m_networkSettings.didRetrain = true;
    }
    // Validate the trained network if we have any validation data
    // Then we validate again and see if the validation error is good or bad...
    ValidateNetwork();
    if (m_firstTrainError > m_networkSettings.meanError)
    {
        // Apparently the retraining was good
        m_networkSettings.retrainingWasGood = true;
    }
    else
    {
        // The retraining was not good. TODO figure out if we should retrain the network again with the original settings or just ignore
        m_networkSettings.retrainingWasGood = false;
    }
    m_networkSettings.bestEpoch = m_bestEpoch;

}

void NeuralNetwork::SaveNetworkToFile(const std::string & p_fileName)
{
    string t_config;
    m_net.save(p_fileName);
   //// Hax so we can use our file writer
   //vector<string> t_lines;
   //t_lines.push_back(t_config);
   //FileHandler::WriteToFile(t_lines, p_fileName);
    
}

void NeuralNetwork::InitializeWeights()
{
    srand(1337);
    // Set starting weights
    if (!m_networkSettings.deterministicWeights)
    {
        m_net.init_weights(*m_networkSettings.trainingData);
    }
    else
    {
        FANN::connection* networkConnections = (FANN::connection*) malloc(sizeof(FANN::connection) * m_net.get_total_connections());
        m_net.get_connection_array(networkConnections);
        for (size_t i = 0; i < m_net.get_total_connections(); i++)
        {
            networkConnections[i].weight = -0.5f + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX) / (0.5f - (-0.5f)));;
        }
        m_net.set_weight_array(networkConnections, m_net.get_total_connections());
    }
}

void NeuralNetwork::TrainOnData(const int& p_numberOfEpochs, const int& p_reportFrequency, const float& p_errorAccaptance)
{
    m_net.train_on_data(*m_networkSettings.trainingData, p_numberOfEpochs, p_reportFrequency, p_errorAccaptance);
    if (m_net.get_errno() != 0)
    {
        int a = 9;
    }
}

void NeuralNetwork::SetupNetwork()
{
    // Cases for how many layers we want. Yup, this is silly...
    switch (m_networkSettings.hiddenLayers)
    {
    case 0:
        m_net.create_standard(2,
            m_networkSettings.inputCells,
            m_networkSettings.outputCells);
        break;
    case 1:
        m_net.create_standard(3,
            m_networkSettings.inputCells,
            m_networkSettings.hiddenCells[0],
            m_networkSettings.outputCells);
        break;
    case 2:
        m_net.create_standard(4,
            m_networkSettings.inputCells,
            m_networkSettings.hiddenCells[0],
            m_networkSettings.hiddenCells[1],
            m_networkSettings.outputCells);
        break;
    case 3:
        m_net.create_standard(5,
            m_networkSettings.inputCells,
            m_networkSettings.hiddenCells[0],
            m_networkSettings.hiddenCells[1],
            m_networkSettings.hiddenCells[2],
            m_networkSettings.outputCells);
        break;
    case 4:
        m_net.create_standard(6,
            m_networkSettings.inputCells,
            m_networkSettings.hiddenCells[0],
            m_networkSettings.hiddenCells[1],
            m_networkSettings.hiddenCells[2],
            m_networkSettings.hiddenCells[3],
            m_networkSettings.outputCells);
        break;
    case 5:
        m_net.create_standard(7,
            m_networkSettings.inputCells,
            m_networkSettings.hiddenCells[0],
            m_networkSettings.hiddenCells[1],
            m_networkSettings.hiddenCells[2],
            m_networkSettings.hiddenCells[3],
            m_networkSettings.hiddenCells[4],
            m_networkSettings.outputCells);
        break;
    }
    // Set learning rate, activation function and steepness
    m_net.set_learning_rate(m_networkSettings.learningRate);
    m_net.set_activation_steepness_output(m_networkSettings.steepnessOutput);
    m_net.set_activation_function_output(m_networkSettings.functionOutput);
    // Hidden settings possibly should be inside an if-condition?
    m_net.set_activation_steepness_hidden(m_networkSettings.steepnessHidden);
    m_net.set_activation_function_hidden(m_networkSettings.functionHidden);
    m_net.set_callback(training_callback, &m_bestEpoch);

    InitializeWeights();

}
