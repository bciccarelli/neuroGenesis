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
        public static float constant = 10;
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
                            //Console.WriteLine("Final node value: " + nodes[j].add[0]);
                            
                            return nodes[j].value[0];
                        }
                        if (nodes[j].layer == 0)
                        {
                            nodes[j].act(input);
                        }
                        else
                        {
                            nodes[j].act();
                        }
                        //Console.WriteLine(n.value[0]);
                    }
                }
            }
            return (float)0;
        }
        public static float closeness (float data, float factor) {
            return (float)(1-(Math.Abs(Math.Atan(factor*data))/(Math.PI/2)));
        }
        public static float estimateAccuracy() {
            float accuracy = 0;
            float value = 0;
            for (int i = 0; i < data.inputData[0].Length; i++) {
                value = runNetwork(data.inputData[0]);
                //Console.WriteLine("Current value: " + value + ", Expected value: " + data.outputData[i]);
                accuracy += closeness(value - data.outputData[i], constant);// data.inputData[0].Length;
                //Console.WriteLine(accuracy);
            }
            return accuracy;
        }
        public static void train(float minAccuracy) {
            float currentAccuracy = 0;
            while (currentAccuracy < minAccuracy) {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].train(step);
                    if (nodes[i].layer == topLayer)
                    {
                        Console.WriteLine("Top layer. Add Length: " + nodes[i].add.Count + ", Add[0]: " + nodes[i].add[0]);
                    }
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
                        Console.WriteLine("Top layer. Add Length: " + nodes[i].add.Count + ", Add[0]: "+ nodes[i].add[0]);
                    }
                }
                numTimes--;
            }
            
        }
    }
    class node {
        public Type input;
        public Type output;
        public List<float> add = new List<float>();
        public List<float> multiply = new List<float>();
        public List<float> value = new List<float>( new float[] { 0 } );
        public int layer;
        public string id;
        public List<string> pastNodes = new List<string>();
        
        
        public node(Type dataType, bool final, int layer, string id)
        {
            this.id = id;
            this.layer = layer;
            this.input = dataType;
            this.output = dataType;
        }
        public void update() {
            while (pastNodes.Count > add.Count) {
                add.Add(5);
                multiply.Add(0);
            }
        }
        public void train(float step) {
            for (int i = 0; i < add.Count; i++) {
                float accuracy = neuralNetwork.estimateAccuracy();

                add[i] += step;
                //Console.WriteLine(add[i]);
                float accuracyAfterStep = neuralNetwork.estimateAccuracy();
                //Console.WriteLine("Add value " + add[i] + ", Accuracy before step : " + accuracy + ", Accuracy after step : " + accuracyAfterStep);
                if (accuracyAfterStep < accuracy)
                {
                    add[i] -= step;
                    //Console.WriteLine("Negated step");
                }
                add[i] -= step;
                accuracyAfterStep = neuralNetwork.estimateAccuracy();
                //Console.WriteLine("Add value " + add[i] + ", Accuracy before step : " + accuracy + ", Accuracy after step : " + accuracyAfterStep);
                if (accuracyAfterStep < accuracy)
                {
                    add[i] += step;
                    //Console.WriteLine("Negated step");
                }
                //Console.WriteLine("Settled Add: " + add[i]);
            }
        }
        public void act() {

            value[0] = 0;
            foreach (float a in add) {
                //Console.WriteLine(a);
                value[0] += a;
            }
        }
        public void act(float[] input)
        {
            value.Clear();
            foreach (float f in input) {
                value.Add(f);
            }
        }
    }
}