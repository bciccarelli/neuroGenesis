using System;
using System.IO;
using System.Collections.Generic;

using System.Threading;
namespace AI
{

    class neuralData {

        public Type objectData;
        public List<float[]> inputData;
        public List<float> outputData;
        public neuralData(List<float[]> input, List<float> output)
        {
            objectData = typeof(float);
            inputData = input;
            outputData = output;
        }
    }
    static class neuralNetwork {
        public static float constant = 10;
        public static int topLayer = 2;
        public static int max;
        public static bool flexible;
        public static neuralData data;
        public static List<node> nodes = new List<node>();
        public static bool useMemory;
        
        public static void createNetwork(neuralData Data)
        {
            flexible = true;
            data = Data;
            nodes.Add(new node(data.objectData, false, 0, "start"));
            nodes.Add(new node(data.objectData, true, 2, "end"));
        }
        public static void createNode(int layer) {
            nodes.Insert(1, new node(data.objectData, false, layer, "new"));
            for (int i = 1; i < nodes.Count; i++) {
                nodes[i].pastNodes.Add(nodes[i - 1].id);
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
                foreach (node n in nodes)
                {
                    if (n.layer == i)
                    {
                        Console.WriteLine("Node id: " + n.id + ", Layer: " + i);
                        if (n.layer == topLayer)
                        {
                            n.act();
                            return n.value[0];
                        }
                        if (n.layer == 0)
                        {
                            n.act(input);
                            
                        }
                        else
                        {
                            n.act();
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
            for (int i = 0; i < data.inputData[0].Length; i++) {
                
                accuracy += closeness(runNetwork(data.inputData[0])-data.outputData[i],constant)/data.outputData.Count;
            }
            return accuracy;
        }
        public static float train(float minAccuracy) {
            float currentAccuracy = 0;
            while (currentAccuracy < minAccuracy) {
                //currentAccuracy = estimateAccuracy();
                currentAccuracy += (float).1;
            }
            return currentAccuracy;
        }
    }
    class node {
        public Type input;
        public Type output;
        public float[] value;
        public int layer;
        public string id;
        public List<string> pastNodes = new List<string>();
        //public List<>
        
        public node(Type dataType, bool final, int layer, string id)
        {
            this.id = id;
            this.layer = layer;
            this.input = dataType;
            this.output = dataType;
        }
        public void act() {
            value = neuralNetwork.nodes[neuralNetwork.getNodeByID(pastNodes[0])].value;
        }
        public void act(float[] input)
        {
            value = input;
        }
    }
}