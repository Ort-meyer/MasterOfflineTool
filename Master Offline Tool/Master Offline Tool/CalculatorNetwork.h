#pragma once
#include "NeuralNetwork.h"
#include <string>
class CalculatorNetwork:public NeuralNetwork
{
public:
   enum CalculationMethod { Addition, Subtraction, Multiplication, Division, ALL };

   CalculatorNetwork();
   ~CalculatorNetwork();
   void CreateTrainingFile();

   CalculationMethod m_methodToUse;
   std::string m_fileName;
};

