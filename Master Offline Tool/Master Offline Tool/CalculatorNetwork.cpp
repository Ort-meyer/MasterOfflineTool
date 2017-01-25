#include "CalculatorNetwork.h"

#include <fstream>
using namespace std;

CalculatorNetwork::CalculatorNetwork()
{
   {
      m_trainingDataFilename = "CalcTrainingDataNEW.data";
      m_validationDataFilename = "CalcTrainingDataNEW.data";
      m_networkSettings.inputCells = 3;
      m_networkSettings.outputCells = 1;
      m_networkSettings.hiddenCells = new int(20); // Yup, it's an array
      m_networkSettings.hiddenLayers = 1;

      m_networkSettings.learningRate = 1;
      m_networkSettings.steepnessOutput = 1;
      m_networkSettings.steepnessHidden = 1;
      m_networkSettings.functionOutput = FANN::SIGMOID_SYMMETRIC;
      m_networkSettings.functionHidden = FANN::SIGMOID_SYMMETRIC;

      m_networkSettings.randomSeed = true;

      m_methodToUse = ALL;

      CreateTrainingFile();
      NeuralNetwork::SetupNetwork();
      NeuralNetwork::TrainOnData(200, 20, 0.0001f);
   }
}


CalculatorNetwork::~CalculatorNetwork()
{
}

void CalculatorNetwork::CreateTrainingFile()
{
   float min = -.5f;
   float max = .5f; // Answere may not be above 1 and 0.5+0.5 is 1
   int numTestCases = 1000;
   ofstream file;
   file.open(m_trainingDataFilename);
   /*
   Print first line of data:
   First is number of test cases.
   next is number of inputs
   finally is number of outputs*/
   file << numTestCases << " " << "3" << " " << "1" << endl;
   // Loop through our test cases
   CalculationMethod method;
   for (int i = 0; i < numTestCases; i++)
   {
      // Create random numbers
      float x = min + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX) / (max - min));
      float y = min + static_cast<float>(rand()) / (static_cast<float>(RAND_MAX) / (max - min));
      if (m_methodToUse == CalculationMethod::ALL)
      {
         method = static_cast<CalculationMethod>(rand() % 4);
      }
      switch (method)
      {
      case CalculatorNetwork::Addition:
         file << x << " " << y << " " << (float)method / 4 << endl << x + y << endl;
         break;
      case CalculatorNetwork::Subtraction:
         file << x << " " << y << " " << (float)method / 4 << endl << x - y << endl;
         break;
      case CalculatorNetwork::Multiplication:
         file << x << " " << y << " " << (float)method / 4 << endl << x * y << endl;
         break;
      case CalculatorNetwork::Division:
         file << x << " " << y << " " << (float)method / 4 << endl << (abs(x) < abs(y) ? x / y : y / x) << endl;
         break;
      default:
         break;
      }
   }
   file.close();
}


