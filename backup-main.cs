using System;
using System.IO;
using System.Collections.Generic;
namespace AI
{
    class App
    {
        public static List<decimal[]> inputs = new List<decimal[]>();
        
        public static decimal[] outputs = new decimal[] { 1, 0, 0, 1 };

        public static void Main(string[] args)
        {

            inputs.Add(new decimal[] { 0, 0, 1 });
            inputs.Add(new decimal[] { 1, 0, 1 });
            inputs.Add(new decimal[] { 1, 0, 0 });
            inputs.Add(new decimal[] { 0, 0, 0 });
            network.applyWeights(3);
            network.train( inputs, outputs, 100 );
            Console.WriteLine(network.run(inputs[0]));
            Console.WriteLine(network.run(inputs[1]));
            Console.WriteLine(network.run(inputs[2]));
            Console.WriteLine(network.run(inputs[3]));
            Console.Read();
        }
        
    }
    static class network {
        public static List<decimal> weights = new List<decimal>();
        public static List<decimal> _outputs = new List<decimal>();
        public static List<decimal> _error = new List<decimal>();
        public static List<decimal> _dot = new List<decimal>();

        public static void train(List<decimal[]> inputs, decimal[] outputs, int iterations)
        {

            for (int j = 0; j < iterations; j++) {
                for (int b = 0; b < inputs.Count; b++)
                {
                    _outputs.Add(run(inputs[b]));
                }
                for (int b = 0; b < outputs.Length-1; b++)
                {
                    _error.Add(_outputs[b] - outputs[b]);
                }
                for (int b = 0; b < outputs.Length - 1; b++)
                {
                    _error[b] *= d_sigmoid(outputs[b]);
                }
                for (int b = 0; b < outputs.Length - 1; b++)
                {
                    _dot.Add(0);
                    for (int i = 0; i < inputs.Count - 1; i++) {
                        _dot[b] += _error[b]*inputs[b][i];
                    }
                }
                for (int b = 0; b < outputs.Length - 1; b++)
                {
                    weights[b]+=_dot[b];
                }
            }

        }
        public static decimal run(decimal[] inputs)
        {
            decimal value = 0;
            for (int i = 0; i < inputs.Length-1; i++) {
                value += weights[i] * inputs[i];
            }
            return sigmoid(value);
        }
        public static void applyWeights(int num) {
            Random r = new Random();
            for (int i = 0; i < num; i++) {
                weights.Add((decimal)r.NextDouble());
            }
        }
        
        public static decimal sigmoid(decimal x)
        {
            return (decimal)(1 / (1 + Math.Pow(Math.E, (double)-x)));
        }
        public static decimal d_sigmoid(decimal x)
        {
            return (x) * (1-x);
        }
    }

}
