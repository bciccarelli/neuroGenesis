using System;
using System.IO;
using System.Collections.Generic;
namespace aiNameSpace
{
    class App
    {
        public static void Main(string[] args)
        {
            neuralData data = new neuralData();
            neuralNetork net = new neuralNetwork(data);

            Console.ReadLine();
        }

    }

}
