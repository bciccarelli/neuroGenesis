using System;
using System.IO;
using System.Collections.Generic;
namespace AI
{
    class App
    {
        public static List<decimal[]> input = new List<decimal[]> {a(1), a(2), a(3), a(4), a(10), a(20), a(50), a(100) };
        public static List<decimal> output = new List<decimal> { 2, 4, 6, 8, 20, 40, 100, 200 };
        public static decimal[] a(double b) {
            return new decimal[] { (decimal)b };
        }
        public static void Main(string[] args)
        {
            neuralData data = new neuralData(input, output);
            neuralNetwork.createNetwork(data);
            neuralNetwork.createNode(1);

            Console.WriteLine(neuralNetwork.estimateAccuracy()*100);
            neuralNetwork.train((decimal).9998);
            Console.WriteLine("Achieved Accuracy: " + neuralNetwork.estimateAccuracy()*100 + "%");
            logger.logNode(neuralNetwork.nodes[0]);
            logger.logNode(neuralNetwork.nodes[1]);
            logger.logNode(neuralNetwork.nodes[2]);
            Console.WriteLine("Input: " + input[0][0] + ", " + neuralNetwork.runNetwork(input[0]));
            Console.WriteLine("Input: " + input[1][0] + ", " +  neuralNetwork.runNetwork(input[1]));
            Console.WriteLine("Input: " + input[2][0] + ", " + neuralNetwork.runNetwork(input[2]));

            Console.WriteLine(neuralNetwork.runNetwork(new decimal[] { 1000 }));
            Console.ReadLine();
        }

    }

}
