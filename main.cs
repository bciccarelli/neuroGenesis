using System;
using System.IO;
using System.Collections.Generic;

using System.Threading;
namespace AI
{

    class neuralData {

        public Type objectData;
        public neuralData(List<int[]> input, List<int> output) {
            objectData = typeof(int);
            List<int[]> inputData = input;
            List<int> outputData = output;
        }
        public neuralData(List<float[]> input, List<float> output)
        {
            objectData = typeof(float);
            List<float[]> inputData = input;
            List<float> outputData = output;
        }
    }
    class neuralNetwork {
        public int max;
        public bool flexible;
        public neuralData data;
        public List<node> nodes = new List<node>();
        public bool useMemory;
        
        public neuralNetwork(neuralData Data, int maxNumOfNodes) {
            max = maxNumOfNodes;
            data = Data;
            nodes.Add(new node(data.objectData, false));
            nodes.Add(new node(data.objectData, true));
        }
        public neuralNetwork(neuralData Data, bool flexible)
        {
            flexible = true;
            data = Data;
            nodes.Add(new node(data.objectData, false));
            nodes.Add(new node(data.objectData, true));
        }
        public void createNode(int pos) {
            nodes.Insert(pos, new node(data.objectData, false));
            for (int i = nodes.Count - 2; i > 0; i --) {
                nodes[i].pastNodes.Add(nodes[i-1]);
            }
        }
        public float runNetwork(float[] input) {
            Console.WriteLine(input[0]);

            nodes[0].act(input);
            return input[0];
        }
    }
    class node {
        public Type input;
        public bool isFinal = false;
        public Type output;
        public float value;
        public List<node> pastNodes = new List<node>();
        //public List<>
        public node(Type input, Type output, bool final)
        {
            isFinal = final;
            this.input = input;
            this.output = output;
        }
        public node(Type dataType, bool final)
        {
            isFinal = final;
            this.input = dataType;
            this.output = dataType;
        }
        public void act(float[] input) {
            
            input[0]+=(float)1;
            this.value = input[0];
            
        }

        public void act(string[] input)
        {
            input[0]+=" ";
            //nextNodes[0].act(input);
        }
    }
}