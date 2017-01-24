#pragma once
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>
#include <string>
class Calculator
{
public:
    enum CalculationMethod {Addition, Subtraction, Multiplication, Division};
    Calculator();
    ~Calculator();

    void CreateTrainingFile();
    void SetUpNet();
    void ReadDataFromFile();
    void TrainOnData(const int& p_numberOfEpochs, const int& p_reportFrequency, const float& p_errorAccaptance, const bool& p_random);
    void ValidateOnFile();
    void ValidateOnNumber(const float& p_number1, const float& p_number2, CalculationMethod p_method);
    void SetFileName(const std::string p_fileName) { m_fileName = p_fileName; };
    void SetRandomSeed(const unsigned int& p_seed) { srand(p_seed); };

private:
    FANN::neural_net m_net;
    FANN::training_data m_data;
    std::string m_fileName;
    
};

