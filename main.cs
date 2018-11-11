using System;
using System.IO;
using System.Collections.Generic;

#pragma warning disable 0649
namespace AI
{
    class App
    {
        public static List<decimal[]> inputs = new List<decimal[]>();
        
        public static decimal[] outputs = new decimal[] { 1, 0, 0, 1, 1, 0, 0, 1 };

        public static void Main(string[] args)
        {
            inputs.Add(new decimal[] { 0, 1, 0 });
            inputs.Add(new decimal[] { 1, 1, 0 });
            inputs.Add(new decimal[] { 1, 1, 1 });
            inputs.Add(new decimal[] { 0, 1, 1 });
            inputs.Add(new decimal[] { 0, 0, 1 });
            inputs.Add(new decimal[] { 1, 0, 1 });
            inputs.Add(new decimal[] { 1, 0, 0 });
            inputs.Add(new decimal[] { 0, 0, 0 });
            network.applyWeights(3);
            List<decimal> os = new List<decimal>();
            os.Add(network.run(inputs[0]));
            os.Add(network.run(inputs[1]));
            os.Add(network.run(inputs[2]));
            os.Add(network.run(inputs[3]));
            Console.WriteLine("Output1: " + os[0]);
            Console.WriteLine("Output2: " + os[1]);
            Console.WriteLine("Output3: " + os[2]);
            Console.WriteLine("Output4: " + os[3]);
            network.train( inputs,  outputs, 100000);

            Console.WriteLine("--------- After Training -------------");
            os = new List<decimal>();
            os.Add(network.run(inputs[0]));
            os.Add(network.run(inputs[1]));
            os.Add(network.run(inputs[2]));
            os.Add(network.run(inputs[3]));

            Console.WriteLine("Output1: " + os[0]);
            Console.WriteLine("Output2: " + os[1]);
            Console.WriteLine("Output3: " + os[2]);
            Console.WriteLine("Output4: " + os[3]);
            decimal x = 0;
            for (int o = 0; o < os.Count; o++) {
                x += os[o];
            }
            x /= 4;
            for (int o = 0; o < os.Count; o++)
            {
                if (os[o] > x)
                {
                    os[o] = 1;
                }
                else {
                    os[o] = 0;
                }
            }

            Console.WriteLine("Equivalent To: "+os[0]);
            Console.WriteLine("Equivalent To: " + os[1]);
            Console.WriteLine("Equivalent To: " + os[2]);
            Console.WriteLine("Equivalent To: " + os[3]);
            Console.Read();
        }
        
    }
    static class network {
        public static List<decimal> weights0 = new List<decimal>();
        public static List<decimal> weights1 = new List<decimal>();
        public static List<decimal> weights2 = new List<decimal>();
        public static List<decimal> dweights0 = new List<decimal>();
        public static List<decimal> dweights1 = new List<decimal>();
        public static List<decimal> dweights2 = new List<decimal>();
        public static List<decimal> _outputs;
        public static List<decimal> _error;
        public static List<decimal> _dot;
        public static decimal a, b, _a, _b;

        public static void train(List<decimal[]> inputs, decimal[] outputs, int iterations)
        {
            _outputs = new List<decimal>();
            _error = new List<decimal>();
            _dot = new List<decimal>();
            for (int j = 0; j < iterations; j++) {

                for (int q = 0; q < inputs.Count-1; q++)
                {
                    _outputs.Add(run(inputs[q]));
                }
                for (int q = 0; q < outputs.Length-1; q++)
                {
                    _error.Add(_outputs[q] - outputs[q]);
                }
                for (int q = 0; q < outputs.Length - 1; q++)
                {
                    _error[q] *= d_sigmoid(outputs[q]);
                }
                _b = b;
                for (int q = 0; q < outputs.Length - 1; q++)
                {
                    _b*=_error[q];
                }
                _a = a;
                for (int q = 0; q < outputs.Length - 1; q++)
                {
                    _a*=_error[q];
                }
                weights2[0] += _a*100000000;
                weights2[1] += _b*100000000;
                
                /*
                decimal ca = weights2[0];
                decimal cb = weights2[1];

                for (int q = 0; q < outputs.Length - 1; q++)
                {
                    ca *= _error[q];
                    cb *= _error[q];
                }
                for (int q = 0; q < outputs.Length - 1; q++)
                {
                    weights0[0] += ca * d_sigmoid(outputs[q]);
                }
                for (int q = 0; q < outputs.Length - 1; q++)
                {
                    weights0[1] += ca * d_sigmoid(outputs[q]);
                }
                for (int q = 0; q < outputs.Length - 1; q++)
                {
                    weights0[2] += ca * d_sigmoid(outputs[q]);
                }
                */

            }

        }
        public static decimal run(decimal[] inputs)
        {
            decimal value = 0;
            for (int i = 0; i < inputs.Length-1; i++) {
                value += weights0[i] * inputs[i];
            }
            a= (value);

            value = 0;
            for (int i = 0; i < inputs.Length - 1; i++)
            {
                value += weights1[i] * inputs[i];
            }
            b = (value);

            value = 0;
            value += weights2[0] * a;
            value += weights2[1] * b;
            return sigmoid(value);
        }
        public static void applyWeights(int num) {
            Random r = new Random();
            for (int i = 0; i < 3; i++) {
                weights0.Add((decimal)r.NextDouble()*2-1);
            }
            for (int i = 0; i < 3; i++)
            {
                weights1.Add((decimal)r.NextDouble() * 2 - 1);
            }
            for (int i = 0; i < 2; i++)
            {
                weights2.Add((decimal)r.NextDouble() * 2 - 1);
            }
        }
        
        public static decimal sigmoid(decimal x)
        {
            return (decimal)(1 / (1 + Math.Pow(Math.E, (double)-x)));
        }
        public static decimal d_sigmoid(decimal x)
        {
            return sigmoid(x) * (1 - sigmoid(x));
        }
    }

}
