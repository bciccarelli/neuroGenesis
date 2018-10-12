using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
namespace AI
{
    class neuralData
    {
        public List<decimal[]> inputData;
        public List<decimal> outputData;
        public neuralData(List<decimal[]> input, List<decimal> output)
        {
            inputData = input;
            outputData = output;
        }
    }
    static class neuralNetwork
    {
        public static int plateau = 100000;
        public static decimal constant = 5;
        public static decimal step = (decimal).0001;
        public static int topLayer = 2;
        public static bool flexible;
        public static neuralData data;
        public static List<node> nodes = new List<node>();

        public static void createNetwork(neuralData Data)
        {
            flexible = true;
            data = Data;
            nodes.Add(new node(typeof(decimal), false, 0, "start"));
            nodes.Add(new node(typeof(decimal), true, 2, "end"));
        }
        public static void createNode(int layer, int num)
        {
            for (int k = 0; k < num; k++)
            {
                nodes.Insert(1, new node(typeof(decimal), false, layer, "node"+nodes.Count));
                for (int j = 0; j < layer; j++)
                {
                    for (int i = 1; i < nodes.Count; i++)
                    {
                        if (!nodes[i].pastNodes.Contains(nodes[i - 1].id))
                        {
                            nodes[i].pastNodes.Add(nodes[i - 1].id);
                        };
                    }
                }
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].update();
                }
            }
        }
        public static int getNodeByID(string id)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].id == id)
                {
                    return i;
                }
            }
            Console.WriteLine("No such ID exists");
            return -1;
        }
        public static decimal runNetwork(decimal[] input)
        {
            for (int i = 0; i < topLayer + 1; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (nodes[j].layer == i)
                    {
                        if (nodes[j].layer == topLayer)
                        {
                            nodes[j].act();

                            //Console.WriteLine(nodes[j].add.Count);
                            //Console.WriteLine(nodes[j].add[1].Count);

                            //Console.WriteLine("Final node value: " + nodes[j].add[1][0]);

                            return nodes[j].value[0];
                        }
                        if (nodes[j].layer == 0)
                        {
                            nodes[j].act(input);

                        }
                        else
                        {
                            nodes[j].act();
                            //Console.WriteLine(nodes[j].id + " node value: " + nodes[j].value[0]);

                        }
                        //Console.WriteLine(n.value[0]);
                    }
                }
            }
            return (decimal)0;
        }
        public static decimal closeness(decimal data, decimal factor)
        {
            decimal num = (decimal)(1 - (Math.Abs(data / factor)));
            //if (num < 0) {
            //    num = 0;
            //}
            return num;
        }
        public static decimal estimateAccuracy()
        {
            decimal accuracy = 0;
            decimal value = 0;
            for (int i = 0; i < data.inputData.Count; i++)
            {
                value = runNetwork(data.inputData[i]);
                //Console.WriteLine("Current value: " + value + ", Expected value: " + data.outputData[i] + ", Accuracy: " + closeness(value - data.outputData[i], constant));
                //Console.WriteLine(data.outputData.Count);
                accuracy += closeness(value - data.outputData[i], constant) / data.outputData.Count;
                //Console.WriteLine(accuracy);
            }
            return accuracy;
        }
        public static void train(decimal minAccuracy)
        {
            decimal currentAccuracy = 0;
            while (currentAccuracy < minAccuracy)
            {
                for (int i = 1; i < nodes.Count; i++)
                {
                    nodes[i].train(step);
                }
                currentAccuracy = estimateAccuracy();
            }
        }
        public static void train(int numTimes)
        {
            while (numTimes > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].train(step);
                    if (nodes[i].layer == topLayer)
                    {
                        //Console.WriteLine("Top layer. Add Length: " + nodes[i].add.Count + ", Add[0]: " + nodes[i].add[0] + ", Accuracy: " + estimateAccuracy());
                    }
                }
                if (numTimes == 1000) { Console.WriteLine(1000); }
                numTimes--;
            }

        }
        public static void train(string info)
        {
            switch (info)
            {
                case "until cap":
                    decimal best = (decimal)0;
                    for (int i = 0; i < plateau; i++)
                    {
                        for (int j = 0; j < nodes.Count; j++)
                        {
                            nodes[j].train(step);
                        }
                        decimal currentAcc = estimateAccuracy();
                        if (currentAcc > best)
                        {
                            best = currentAcc;
                            i = 0;
                        }
                    }
                    break;
            }
        }
        public static void train(string info, decimal min)
        {
            switch (info)
            {
                case "until cap":
                    decimal best = (decimal)0;
                    for (int i = 0; i < plateau; i++)
                    {
                        for (int j = 0; j < nodes.Count; j++)
                        {
                            nodes[j].train(step);
                        }
                        decimal currentAcc = estimateAccuracy();
                        if (currentAcc > best)
                        {
                            best = currentAcc;
                            i = 0;
                        }
                        if (best < min) {
                            i = 0;
                        }
                    }
                    break;
            }
        }
    }
    class node
    {

        public List<List<decimal>> variables = new List<List<decimal>>();
        public List<decimal> value = new List<decimal>(new decimal[] { 0 });
        public int layer;
        public List<Action> functions;
        public string id;
        public List<string> pastNodes = new List<string>();
        public void mult()
        {
            value[0] *= variables[0][0];
            for (int i = 1; i < variables[0].Count; i++)
            {
                value[0] += variables[0][i] * neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[0];
            }
        }
        public void add()
        {
            value[0] += variables[1][0];
            for (int i = 1; i < variables[1].Count; i++)
            {
                value[0] += variables[1][i] * neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[0];
            }
        }

        public void addEnd()
        {
            value[0] += variables[3][0];
            for (int i = 1; i < variables[3].Count; i++)
            {
                value[0] += variables[3][i] * neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[0];
            }
        }
        public void exp()
        {
            for (int i = 1; i < variables[2].Count; i++)
            {
                double v = (double)neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[0];
                if (v >= 0)
                {
                    value[0] += (decimal)Math.Pow(v, (double)variables[2][i]);
                }
            }
        }
        public node(Type dataType, bool final, int layer, string id)
        {
            this.id = id;
            this.layer = layer;
            functions = new List<Action>(new Action[] { add, mult, exp, addEnd });
            variables.Add(new List<decimal>());
            variables.Add(new List<decimal>());
            variables.Add(new List<decimal>());
            variables.Add(new List<decimal>());
            variables[0].Add(1);
            variables[1].Add(1);
            variables[2].Add(1);
            variables[3].Add(1);
        }
        public void update()
        {
            while (pastNodes.Count > variables[0].Count - 1)
            {
                variables[0].Add(1);
                variables[1].Add(1);
                variables[2].Add(1);
                variables[3].Add(1);
            }
        }
        public void train(decimal step)
        {
            Random rand = new Random();
            decimal accuracy;
            List<decimal> steps = new List<decimal>();
            //decimal step2;
            decimal accuracyAfterStep;
            accuracy = neuralNetwork.estimateAccuracy();
            for (int a = 0; a < variables.Count; a++)
            {
                for (int b = 0; b < variables[a].Count; b++)
                {
                    steps.Add(step * (rand.Next(0, 100) - 50));
                    //Console.WriteLine(steps[d]);
                    variables[a][b] += steps[a];

                }
            }
            accuracyAfterStep = neuralNetwork.estimateAccuracy();
            if (accuracyAfterStep <= accuracy)
            {
                for (int a = 0; a < variables.Count; a++)
                {
                    for (int b = 0; b < variables[a].Count; b++)
                    {
                        //Console.WriteLine(steps[d]);
                        variables[a][b] -= steps[a];
                    }
                }
            }
            else
            {
                Console.WriteLine(accuracy);
            }
        }
        public void act()
        {
            value[0] = 0;
            /*for (int i = 1; i < add.Count; i++)
            {
                for (int j = 0; j < add[i].Count; j++)
                {
                    value[0] += add[i][j] * neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[j])].value[j];
                    //Console.WriteLine("id: " + this.id + ", past id: " + pastNodes[i - 1] + ", past count " + (neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[0]));

                }
            }*/
            foreach (Action a in functions)
            {
                a();
            }

            value[0] += variables[1][0];
        }

        public void act(decimal[] input)
        {
            value.Clear();
            foreach (decimal f in input)
            {
                value.Add(f);
            }
        }
    }
    static class logger
    {
        static string write;
        public static void logNode(node n)
        {
            write = "Add: ";
            foreach (decimal d in n.variables[1])
            {
                write += "[" + d + "], ";
            }
            write += "... Multiply: ";
            foreach (decimal d in n.variables[0])
            {
                write += "[" + d + "], ";
            }
            Console.WriteLine(write);
        }
    }
}