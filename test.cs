using System;
using System.IO;
using System.Collections.Generic;
namespace AI
{
    class App
    {
        public static List<int[]> input = new List<int[]> { new int[] { 0, 1, 2 }, new int[] { 1, 2, 3 } };
        public static List<int> output = new List<int> { 0, 1, 2 };

        public static void Main(string[] args)
        {
            neuralData data = new neuralData(input, output);
            neuralNetwork net = new neuralNetwork(data, 1000);
            net.createNode(1);
            Console.WriteLine(net.nodes);
            Console.ReadLine();
        }

    }

}
