#include "Calculator.h"

#include <iostream>
#include <fstream>

// Not sure what these do...
#include <ios>
#include <iomanip>
using namespace std;

int calculator_callback(FANN::neural_net &net, FANN::training_data &train,
    unsigned int max_epochs, unsigned int epochs_between_reports,
    float desired_error, unsigned int epochs, void *user_data)
{
    cout << "Epochs     " << epochs << ". "
        << "Current Error: " << net.get_MSE() << endl;
    return 0;
}


Calculator::Calculator()
{
}


Calculator::~Calculator()
{
}

void Calculator::CreateTrainingFile(const int& p_numberOfTestcases, CalculationMethod p_methodToCreateDataOn)
{
    float min = -.5f;
    float max = .5f; // Answere may not be above 1 and 0.5+0.5 is 1
    int numTestCases = p_numberOfTestcases;
    CalculationMethod method = p_methodToCreateDataOn;
    ofstream file;
    file.open(m_fileName);
    /*
    Print first line of data:
    First is number of test cases.
    next is number of inputs
    finally is number of outputs*/
    file << numTestCases << " " << "3" << " " << "1" << endl;
    // Loop through our test cases
    for (int i = 0; i < numTestCases; i++)
    {
        // Create random numbers
        float x = min + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX)/(max-min));
        float y = min + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX)/(max-min));
        if (p_methodToCreateDataOn == CalculationMethod::ALL)
        {
            method = static_cast<CalculationMethod>(rand() % 4);
        }      
        switch (method)
        {
        case Calculator::Addition:
            file << x << " " << y << " " << (float)method / 4 << endl << x + y << endl;
            break;
        case Calculator::Subtraction:
            file << x << " " << y << " " << (float)method / 4 << endl << x - y << endl;
            break;
        case Calculator::Multiplication:
            file << x << " " << y << " " << (float)method / 4 << endl << x * y << endl;
            break;
        case Calculator::Division:
            file << x << " " << y << " " << (float)method / 4 << endl << (abs(x) < abs(y) ? x / y: y / x ) << endl;
            break;
        default:
            break;
        }
    }
    file.close();
}

void Calculator::SetUpNet()
{
    // set up the network
    m_net.create_standard(3, 3,20, 1);
    m_net.set_callback(calculator_callback, NULL);

    m_net.set_learning_rate(1.0f);
    m_net.set_activation_steepness_output(1.0f);
    m_net.set_activation_steepness_hidden(1.0f);
    m_net.set_activation_function_hidden(FANN::SIGMOID_SYMMETRIC);
    m_net.set_activation_function_output(FANN::SIGMOID_SYMMETRIC);
}

void Calculator::ReadDataFromFile()
{
    m_data.read_train_from_file(m_fileName);
}

void Calculator::TrainOnData(const int & p_numberOfEpochs, const int & p_reportFrequency, const float & p_errorAccaptance, const bool& p_random)
{
    if (p_random)
    {
        m_net.init_weights(m_data);
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

    m_net.train_on_data(m_data, p_numberOfEpochs, p_reportFrequency, p_errorAccaptance);
}

void Calculator::ValidateOnFile()
{
    m_data.read_train_from_file(m_fileName);
    m_net.test_data(m_data);
    std::cout << "Mean Square Error according to FANN: " << m_net.get_MSE() << endl;
    float fullError = 0;
    float* input = *m_data.get_input();
    float* output = *m_data.get_output();
    int length = m_data.length_train_data();
    for (size_t i = 0; i < length; i++)
    {
        float* netOutput = m_net.run(&input[i*3]);
        std::cout << "Net: " << *netOutput << " Acctual: " << output[i] << endl;
        fullError += abs(abs(*netOutput) - abs(output[i]));
    }
    std::cout << "mean error: " << fullError / static_cast<float>(length)<<endl;
}

void Calculator::ValidateOnNumber(const float & p_number1, const float & p_number2, CalculationMethod p_method)
{
    float Answere;
    std::string calcMethod;

    switch (p_method)
    {
    case Calculator::Addition:
        Answere = p_number1 + p_number2;
        calcMethod = "Addition";
        break;
    case Calculator::Subtraction:
        Answere = p_number1 - p_number2;
        calcMethod = "Subtraction";
        break;
    case Calculator::Multiplication:
        Answere = p_number1 * p_number2;
        calcMethod = "Multiplication";
        break;
    case Calculator::Division:
        Answere = abs(p_number1) < abs(p_number2) ? p_number1/p_number2 : p_number2/p_number1 ;
        calcMethod = "Division";
        break;
    default:
        break;
    }
    float* input = (float*)malloc(sizeof(float) * 3);
    input[0] = p_number1;
    input[1] = p_number2;
    input[2] = static_cast<float>(p_method) / 4.0f;
    float netAnswere = *m_net.run(input);

    std::cout << "Calculation method " << calcMethod << " Network: " << netAnswere << " Real: " << Answere << endl;
}
