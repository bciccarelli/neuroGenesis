using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
namespace AI
{
    class neuralData {
        public List<decimal[]> inputData;
        public List<decimal> outputData;
        public neuralData(List<decimal[]> input, List<decimal> output)
        {
            inputData = input;
            outputData = output;
        }
    }
    static class neuralNetwork {
        public static decimal constant = 2;
        public static decimal step = (decimal).001;
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
        public static void createNode(int layer) {
            nodes.Insert(1, new node(typeof(decimal), false, layer, "new"));
            for (int i = 1; i < nodes.Count; i++) {
                nodes[i].pastNodes.Add(nodes[i - 1].id);
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].update();
            }
        }
        public static int getNodeByID(string id) {
            for (int i = 0; i < nodes.Count; i++) {
                if (nodes[i].id == id) {
                    return i;
                }
            }
            Console.WriteLine("No such ID exists");
            return -1;
        }
        public static decimal runNetwork(decimal[] input) {
            for (int i = 0; i < topLayer+1; i++) {
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
        public static decimal closeness (decimal data, decimal factor) {
            decimal num = (decimal)(1 - (Math.Abs(data/factor)));
            //if (num < 0) {
            //    num = 0;
            //}
            return num;
        }
        public static decimal estimateAccuracy() {
            decimal accuracy = 0;
            decimal value = 0;
            for (int i = 0; i < data.inputData.Count; i++) {
                value = runNetwork(data.inputData[i]);
                //Console.WriteLine("Current value: " + value + ", Expected value: " + data.outputData[i] + ", Accuracy: " + closeness(value - data.outputData[i], constant));
                //Console.WriteLine(data.outputData.Count);
                accuracy += closeness(value - data.outputData[i], constant)/data.outputData.Count;
                //Console.WriteLine(accuracy);
            }
            return accuracy;
        }
        public static void train(decimal minAccuracy) {
            decimal currentAccuracy = 0;
            while (currentAccuracy < minAccuracy) {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].train(step);
                }
                currentAccuracy = estimateAccuracy();
            }
        }
        public static void train(int numTimes) {
            while (numTimes > 0) {
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
            if (info == "until cap") {
                decimal cap = 1;
                decimal currentAccuracy = 0;
                decimal newAccuracy = 0;
                while (currentAccuracy < cap)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        nodes[i].train(step);
                    }
                    newAccuracy = estimateAccuracy();
                    
                    if (currentAccuracy > newAccuracy)
                    {
                        cap = newAccuracy;
                    }
                    else {
                        currentAccuracy = estimateAccuracy();
                    }
                    

                }
            }
            

        }
    }
    class node {
        
        public List<List<decimal>> add = new List<List<decimal>>();
        public List<List<decimal>> multiply = new List<List<decimal>>();
        public List<decimal> value = new List<decimal>( new decimal[] { 0 } );
        public int layer;
        public string id;
        public List<string> pastNodes = new List<string>();
        
        public node(Type dataType, bool final, int layer, string id)
        {
            this.id = id;
            this.layer = layer;
            add.Add(new List<decimal>( new decimal[] { 0 } ));
            multiply.Add(new List<decimal>( new decimal[] { 2 } ));
        }
        public void update() {
            while (pastNodes.Count > add.Count - 1) {
                add.Add(new List<decimal>(new decimal[] { 1 }));
                multiply.Add(new List<decimal>(new decimal[] { 0 }));
            }
        }
        public void train(decimal step) {
            Random rand = new Random();
            decimal accuracy;
            decimal step1;
            decimal step2;
            decimal accuracyAfterStep;
            for (int i = 0; i < add.Count; i++) {
                for (int j = 0; j < add[i].Count; j++)
                {
                    accuracy = neuralNetwork.estimateAccuracy();
                    step1 = step * (rand.Next(0, 100)-50);
                    step2 = step * (rand.Next(0, 100)-50);
                    multiply[i][j] += step1;
                    add[i][j] += step2;
                    accuracyAfterStep = neuralNetwork.estimateAccuracy();
                    if (accuracyAfterStep <= accuracy)
                    {
                        multiply[i][j] -= step1;
                        add[i][j] -= step2;
                    }
                    else {
                        Console.WriteLine(accuracy);
                    }
                    
                }
                
            }
        }
        public void act() {
            value[0] = 0;

            for (int i = 1; i < add.Count; i++)
            {
                for (int j = 0; j < add[i].Count; j++)
                {
                    value[0] += add[i][j] * neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[j])].value[j];
                    //Console.WriteLine("id: " + this.id + ", past id: " + pastNodes[i - 1] + ", past count " + (neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[0]));

                }
            }
            for (int i = 1; i < add.Count; i++)
            {
                for (int j = 0; j < add[i].Count; j++)
                {
                    value[0] *= 1 + multiply[i][j] * neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[j])].value[j];
                }
            }
            value[0] *= multiply[0][0];


            value[0] += add[0][0];
        }

        public void act(decimal[] input)
        {
            value.Clear();
            foreach (decimal f in input) {
                value.Add(f);
            }
        }
    }
    static class logger
    {
        static string write;
        public static void logNode(node n) {
            write = "Add: ";
            foreach (List<decimal> f in n.add) {
                foreach (decimal fl in f) {
                    write += "[" + fl + "], ";
                }
            }
            write += "... Multiply: ";
            foreach (List<decimal> f in n.multiply)
            {
                foreach (decimal fl in f)
                {
                    write += "[" + fl + "], ";
                }
            }
            Console.WriteLine(write);
        }
    }
}