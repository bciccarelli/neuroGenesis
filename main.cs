using System;
using System.IO;
using System.Collections.Generic;

#pragma warning disable 0649
namespace AI
{
    class App
    {
        public static List<decimal[]> inputs = new List<decimal[]>();
        
        public static decimal[] outputs = new decimal[] { 0, 1, 1, 0, 0, 1, 1, 0 };

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
            List<decimal> osa = new List<decimal>();
            os.Add(network.run(inputs[0]));
            os.Add(network.run(inputs[1]));
            os.Add(network.run(inputs[2]));
            os.Add(network.run(inputs[3]));
            osa.Add(-os[0]);
            osa.Add(-os[1]);
            osa.Add(-os[2]);
            osa.Add(-os[3]);
            Console.WriteLine("Output1: " + os[0]);
            Console.WriteLine("Output2: " + os[1]);
            Console.WriteLine("Output3: " + os[2]);
            Console.WriteLine("Output4: " + os[3]);
            network.train( inputs,  outputs, 10000);

            Console.WriteLine("--------- After Training -------------");
            os = new List<decimal>();
            os.Add(network.run(inputs[0]));
            os.Add(network.run(inputs[1]));
            os.Add(network.run(inputs[2]));
            os.Add(network.run(inputs[3]));
            osa[0] += os[0];
            osa[1] += os[1];
            osa[2] += os[2];
            osa[3] += os[3];

            Console.WriteLine("Output1: " + os[0]);
            Console.WriteLine("Output2: " + os[1]);
            Console.WriteLine("Output3: " + os[2]);
            Console.WriteLine("Output4: " + os[3]);
            Console.WriteLine("Output Change 1: " + osa[0]);
            Console.WriteLine("Output Change 2: " + osa[1]);
            Console.WriteLine("Output Change 3: " + osa[2]);
            Console.WriteLine("Output Change 4: " + osa[3]);


            Console.WriteLine("Equivalent To: "+ Math.Round(os[0]));
            Console.WriteLine("Equivalent To: " + Math.Round(os[1]));
            Console.WriteLine("Equivalent To: " + Math.Round(os[2]));
            Console.WriteLine("Equivalent To: " + Math.Round(os[3]));
            while(true){
                Console.Read();
                Console.Clear();
                network.train(inputs, outputs, 100);
                os = new List<decimal>();
                os.Add(network.run(inputs[0]));
                os.Add(network.run(inputs[1]));
                os.Add(network.run(inputs[2]));
                os.Add(network.run(inputs[3]));
                Console.WriteLine("Output1: " + os[0]);
                Console.WriteLine("Output2: " + os[1]);
                Console.WriteLine("Output3: " + os[2]);
                Console.WriteLine("Output4: " + os[3]);
            }

        }
        
    }
    static class network {
        public static List<decimal> weights0 = new List<decimal>();
        public static List<decimal> weights1 = new List<decimal>();
        public static List<decimal> weights2 = new List<decimal>();
        public static List<decimal> _outputs;
        public static List<decimal> _error;
        public static List<decimal> _weight_error;
        public static List<decimal> _a;
        public static List<decimal> _b;
        public static decimal step = (decimal)100000;
        public static decimal a, b;

        public static void train(List<decimal[]> inputs, decimal[] outputs, int iterations)
        {
            for (int j = 0; j < iterations; j++) {
                _outputs = new List<decimal>();
                _error = new List<decimal>();
                _a = new List<decimal>();
                _b = new List<decimal>();
                for (int q = 0; q < inputs.Count; q++)
                {
                    _outputs.Add(run(inputs[q]));

                    _a.Add(a);
                    _b.Add(b);
                    //Console.WriteLine("a: "+a+", b: "+b);
                }
                for (int q = 0; q < outputs.Length; q++)
                {
                    _error.Add(outputs[q]-_outputs[q]);
                    //Console.WriteLine(_error[q]);
                }
                
                for (int q = 0; q < outputs.Length; q++)
                {
                    _error[q] *= d_sigmoid(_outputs[q]);

                    weights2[0] += _a[q] * _error[q];
                    weights2[1] += _b[q] * _error[q];

                    weights0[0] += inputs[q][0] * weights2[0] * _error[q];
                    weights0[1] += inputs[q][1] * weights2[0] * _error[q];
                    weights0[2] += inputs[q][2] * weights2[0] * _error[q];

                    weights1[0] += inputs[q][0] * weights2[1] * _error[q];
                    weights1[1] += inputs[q][1] * weights2[1] * _error[q];
                    weights1[2] += inputs[q][2] * weights2[1] * _error[q];
                }
                
                //Console.WriteLine("After: "+weights0[0]);
            }

        }
        public static decimal run(decimal[] inputs)
        {
            decimal value = 0;
            for (int i = 0; i < inputs.Length; i++) {
                value += weights0[i] * inputs[i];
            }
            a = value;

            value = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                value += weights1[i] * inputs[i];
            }
            b = value;

            value = 0;
            value += weights2[0] * a;
            value += weights2[1] * b;
            return sigmoid(value);
        }
        public static void applyWeights(int num) {
            Random r = new Random();
            for (int i = 0; i < 3; i++) {
                weights0.Add((decimal)r.NextDouble()*2-1);
                //weights0.Add((decimal).2);
            }
            for (int i = 0; i < 3; i++)
            {
                weights1.Add((decimal)r.NextDouble() * 2 - 1);
                //weights1.Add((decimal).8);
            }
            for (int i = 0; i < 2; i++)
            {
                //weights2.Add((decimal)r.NextDouble() * 2 - 1);
                weights2.Add((decimal)1-i);
            }
        }
        
        public static decimal sigmoid(decimal x)
        {
            return (decimal)(1 / (1 + Math.Pow(Math.E, (double)-x)));
        }
        public static decimal r_sigmoid(decimal x)
        {
            return (decimal)(Math.Log((double)(1/((1/x)-1))));
        }
        public static decimal d_sigmoid(decimal x)
        {
            return (x) * (1 - (x));
        }
    }

}
