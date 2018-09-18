using System;
using System.IO;
using System.Collections.Generic;
namespace AI
{
    class App
    {
        public static List<float[]> input = new List<float[]> { new float[] { 2, 3, 4 }, new float[] { 1, 2, 3 } };
        public static List<float> output = new List<float> { 0, 1, 2 };

        public static void Main(string[] args)
        {
            neuralData data = new neuralData(input, output);
            neuralNetwork.createNetwork(data);
            neuralNetwork.createNode(1);

            Console.WriteLine(neuralNetwork.estimateAccuracy());
            neuralNetwork.train((float).9);
            Console.WriteLine("Required Accuracy: 90%, Achieved Accuracy: " + neuralNetwork.estimateAccuracy() + "%");

            Console.ReadLine();
        }

    }

}
