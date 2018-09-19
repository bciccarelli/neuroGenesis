using System;
using System.IO;
using System.Collections.Generic;
namespace AI
{
    class App
    {
        public static List<float[]> input = new List<float[]> { new float[] { 1 }, new float[] { 2 }, new float[] { 3 } };
        public static List<float> output = new List<float> { 2, 4, 6 };

        public static void Main(string[] args)
        {
            neuralData data = new neuralData(input, output);
            neuralNetwork.createNetwork(data);
            neuralNetwork.createNode(1);

            Console.WriteLine(neuralNetwork.estimateAccuracy());
            neuralNetwork.train(200);
            Console.WriteLine("Achieved Accuracy: " + neuralNetwork.estimateAccuracy() + "%");
            logger.logNode(neuralNetwork.nodes[0]);
            logger.logNode(neuralNetwork.nodes[1]);
            logger.logNode(neuralNetwork.nodes[2]);
            Console.WriteLine(neuralNetwork.runNetwork(input[0]));
            Console.WriteLine(neuralNetwork.runNetwork(input[1]));
            Console.WriteLine(neuralNetwork.runNetwork(input[2]));
            Console.ReadLine();
        }

    }

}
