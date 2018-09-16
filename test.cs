using System;
using System.IO;
using System.Collections.Generic;
namespace AI
{
    class App
    {
        public static List<float[]> input = new List<float[]> { new float[] { 0, 1, 2 }, new float[] { 1, 2, 3 } };
        public static List<float> output = new List<float> { 0, 1, 2 };

        public static void Main(string[] args)
        {
            neuralData data = new neuralData(input, output);
            neuralNetwork.createNetwork(data);
            neuralNetwork.createNode(1);
            Console.WriteLine(neuralNetwork.runNetwork(new float[] { (float)1 }));
            Console.WriteLine(neuralNetwork.estimateAccuracy());
            Console.ReadLine();
        }

    }

}
