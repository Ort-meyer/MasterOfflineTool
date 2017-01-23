#include <iostream>
using namespace std;

#include <FANN\header\floatfann.h>
#include <FANN\header\fann_cpp.h>

#include <fstream>

// Not sure what these do...
#include <ios>
#include <iomanip>


int print_callback(FANN::neural_net &net, FANN::training_data &train,
	unsigned int max_epochs, unsigned int epochs_between_reports,
	float desired_error, unsigned int epochs, void *user_data)
{
	cout << "Epochs     " << setw(8) << epochs << ". "
		<< "Current Error: " << left << net.get_MSE() << right << endl;
	return 0;
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



	int pause;
	cin >> pause;
}