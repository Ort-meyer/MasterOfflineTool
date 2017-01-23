#include <iostream>
#include <fstream>

// Not sure what these do...
#include <ios>
#include <iomanip>
using namespace std;
#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>

int print_callback(FANN::neural_net &net, FANN::training_data &train,
    unsigned int max_epochs, unsigned int epochs_between_reports,
    float desired_error, unsigned int epochs, void *user_data)
{
    cout << "Epochs     " << epochs << ". "
        << "Current Error: " << net.get_MSE() << endl;
    return 0;
}

void TrainNeuralNetwork(FANN::neural_net* io_net)
{
    io_net->train_on_file("PlusTest.data",200,50,0.0001f);
}

void SetUpNeuralNet(FANN::neural_net* io_net)
{
    // set up the network
    io_net->create_standard(2,2,1);
    io_net->set_callback(print_callback, NULL);
}

void RunNeuralNetwork(FANN::neural_net* io_net, const float& p_input1, const float& p_input2)
{
    FANN::training_data data;
    float* input = (float*)malloc(sizeof(float) * 2);
    *input = p_input1;
    input[1] = p_input2;
    float* hej = io_net->run(input);
    if (*hej != input[0] + input[1])
    {
        std::cout << "Not correct! Should be: " << input[0] + input[1] << " But was " << *hej << endl;
    }
    else
    {
        std::cout << "Correct!!";
    }
}



void CreatetestFile()
{
	int min = 0;
	int max = 100;
	int numTestCases = 50;
	ofstream file;
	file.open("PlusTest.data");
	/* 
	Print first line of data:
	First is number of test cases.
	next is number of inputs
	finally is number of outputs*/
	file << numTestCases << " " << "2" << " " << "1" << endl;
	// Loop through our test cases
	for (int i = 0; i < numTestCases; i++)
	{
		// Create random numbers
		int x = rand() % (max - min + 1) + min;
		int y = rand() % (max - min + 1) + min;
		if (x == y)
			cout << "fuck";
		// Print the numbers as inputs and their sum as output
		file << x << " " << y << endl << x+y << endl;
	}
	file.close();

}

int main()
{
	cout << "Hello, world!" << endl;
	CreatetestFile();
	const float learning_rate = 0.7f;
	const unsigned int num_layers = 3;
	const unsigned int num_input = 2;
	const unsigned int num_hidden = 3;
	const unsigned int num_output = 1;
	const float desired_error = 0.001f;
	const unsigned int max_iterations = 300000;
	const unsigned int iterations_between_reports = 1000;




    FANN::neural_net* net = new FANN::neural_net();
    SetUpNeuralNet(net);
    TrainNeuralNetwork(net);
    RunNeuralNetwork(net, 2, 3);

	int pause;
	cin >> pause;
}