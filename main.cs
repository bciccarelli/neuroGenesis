using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
namespace AI
{
    class neuralData {
        public List<float[]> inputData;
        public List<float> outputData;
        public neuralData(List<float[]> input, List<float> output)
        {
            inputData = input;
            outputData = output;
        }
    }
    static class neuralNetwork {
        public static float constant = 1;
        public static float step = (float).01;
        public static int topLayer = 2;
        public static bool flexible;
        public static neuralData data;
        public static List<node> nodes = new List<node>();
        
        public static void createNetwork(neuralData Data)
        {
            flexible = true;
            data = Data;
            nodes.Add(new node(typeof(float), false, 0, "start"));
            nodes.Add(new node(typeof(float), true, 2, "end"));
        }
        public static void createNode(int layer) {
            nodes.Insert(1, new node(typeof(float), false, layer, "new"));
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
        public static float runNetwork(float[] input) {
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
            return (float)0;
        }
        public static float closeness (float data, float factor) {
            float num = (float)(1 - (Math.Abs(factor * data)));
            //if (num < 0) {
            //    num = 0;
            //}
            return num;
        }
        public static float estimateAccuracy() {
            float accuracy = 0;
            float value = 0;
            for (int i = 0; i < data.inputData.Count; i++) {
                value = runNetwork(data.inputData[i]);
                //Console.WriteLine("Current value: " + value + ", Expected value: " + data.outputData[i] + ", Accuracy: " + closeness(value - data.outputData[i], constant));
                accuracy += closeness(value - data.outputData[i], constant)/data.inputData[0].Length;
                //Console.WriteLine(accuracy);
            }
            return accuracy;
        }
        public static void train(float minAccuracy) {
            float currentAccuracy = 0;
            while (currentAccuracy < minAccuracy) {
                Console.WriteLine(estimateAccuracy());
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
                numTimes--;
            }
            
        }
        public static void train(string info)
        {
            if (info == "until cap") {
                float cap = 1;
                float currentAccuracy = 0;
                float newCurrentAccuracy = 0;

                while (currentAccuracy < cap)
                {

                    for (int i = 0; i < nodes.Count; i++)
                    {
                        nodes[i].train(step);
                    }
                    newCurrentAccuracy = estimateAccuracy();
                    if (newCurrentAccuracy == currentAccuracy) {
                        cap=currentAccuracy;
                    }
                    currentAccuracy = newCurrentAccuracy;
                }
            }
            

        }
    }
    class node {
        
        public List<List<float>> add = new List<List<float>>();
        public List<List<float>> multiply = new List<List<float>>();
        public List<float> value = new List<float>( new float[] { 0 } );
        public int layer;
        public string id;
        public List<string> pastNodes = new List<string>();
        
        public node(Type dataType, bool final, int layer, string id)
        {
            this.id = id;
            this.layer = layer;
            add.Add(new List<float>( new float[] { 0 } ));
            multiply.Add(new List<float>( new float[] { 1 } ));
        }
        public void update() {
            while (pastNodes.Count > add.Count - 1) {
                add.Add(new List<float>(new float[] { 1 }));
                multiply.Add(new List<float>(new float[] { 1 }));
            }
        }
        public void train(float step) {
            for (int i = 0; i < add.Count; i++) {
                for (int j = 0; j < add[i].Count; j++)
                {
                    float accuracy = neuralNetwork.estimateAccuracy();
                    add[i][j] += step;

                    float accuracyAfterStep = neuralNetwork.estimateAccuracy();

                    //Console.WriteLine("a b: " + accuracy + ", a a: " + accuracyAfterStep);
                    if (i == 1)
                    {
                        Console.WriteLine("before: " + accuracy + ", after: " + accuracyAfterStep);
                    }
                    if (accuracyAfterStep < accuracy)
                    {
                        add[i][j] -= step;
                    }
                    accuracy = neuralNetwork.estimateAccuracy();
                    add[i][j] -= step;
                    accuracyAfterStep = neuralNetwork.estimateAccuracy();

                    //Console.WriteLine("After subtract a b: " + accuracy + ", a a: " + accuracyAfterStep);
                    if (accuracyAfterStep < accuracy)
                    {

                        add[i][j] += step;
                    }

                    accuracy = neuralNetwork.estimateAccuracy();
                    multiply[i][j] += step;
                    accuracyAfterStep = neuralNetwork.estimateAccuracy();
                    if (accuracyAfterStep < accuracy)
                    {
                        multiply[i][j] -= step;
                    }
                    accuracy = neuralNetwork.estimateAccuracy();
                    multiply[i][j] -= step;
                    accuracyAfterStep = neuralNetwork.estimateAccuracy();
                    if (accuracyAfterStep < accuracy)
                    {
                        multiply[i][j] += step;
                    }
                }
                
            }
        }
        public void act() {
            value[0] = 0;
            
            for (int i = 1; i < add.Count; i++) {
                for (int j = 0  ; j < add[i].Count; j++)
                {
                    value[0] += add[i][j] * neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[j];
                    //Console.WriteLine("id: " + this.id + ", past id: " + pastNodes[i - 1] + ", past count " + (neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[0]));

                }
            }
            value[0] += add[0][0];

            for (int i = 1; i < add.Count; i++)
            {
                for (int j = 0; j < add[i].Count; j++)
                {
                    value[0] *= multiply[i][j] * neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[i - 1])].value[j];
                }
            }
            value[0] *= multiply[0][0];
            
        }
        public void act(float[] input)
        {
            value.Clear();
            foreach (float f in input) {
                value.Add(f);
            }
        }
    }
    static class logger
    {
        static string write;
        public static void logNode(node n) {
            write = "Add: ";
            foreach (List<float> f in n.add) {
                foreach (float fl in f) {
                    write += "[" + fl + "], ";
                }
            }
            write += "... Multiply: ";
            foreach (List<float> f in n.multiply)
            {
                foreach (float fl in f)
                {
                    write += "[" + fl + "], ";
                }
            }
            Console.WriteLine(write);
        }
    }
}