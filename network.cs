using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
#pragma warning disable 0649
namespace AI
{
    static class network {
        public static List<List<neuron>> brain = new List<List<neuron>>();
        public static int outs, ins, layers, nodesPerLayer = 0;
        public static decimal step = (decimal)0.01;
        public static void setup(int numInputs, int numOutputs, int numLayers, int numNodesPerLayer) {
            int outs = numOutputs, ins = numInputs, layers = numLayers, nodesPerLayer = numNodesPerLayer;
            brain.Add(new List<neuron>());
            for (int j = 0; j < numInputs; j++)
            {
                brain[0].Add(new neuron(numInputs));
            }
            for (int i = 1; i < layers; i++) {
                brain.Add(new List<neuron>());
            }
            for (int i = 1; i < layers; i++)
            {
                for (int j = 0; j < numNodesPerLayer; j++)
                {
                    brain[i].Add(new neuron(numNodesPerLayer));
                    brain[i][j].typeOfNeuron = i%2;
                }
            }
            brain.Add(new List<neuron>());
            for (int j = 0; j < numOutputs; j++)
            {
                brain[brain.Count - 1].Add(new neuron(numNodesPerLayer));
            }
        }
        public static void startNetwork(List<decimal> input) {

            for (int j = 0; j < brain[0].Count; j++)
            {
                brain[0][j].run(input);
            }
            for (int i = 1; i < brain.Count; i++)
            {
                for (int j = 0; j < brain[i].Count; j++)
                {
                    brain[i][j].run(brain[i - 1]);
                }
            }
        }
        public static void train(List<List<decimal>> inputs, List<List<decimal>> outputs, int numTimes)
        {   
            if (outputs.Count == inputs.Count)
            {
                if (outputs[0].Count == brain[brain.Count - 1].Count)
                {
                    for (int i = 0; i < numTimes; i++) {
                        for (int j = 0; j < brain[brain.Count - 1].Count; j++)
                        {
                            brain[brain.Count - 1][j].pull = 0;
                        }
                        for (int g = 0; g < inputs.Count; g++)
                        {
                            startNetwork(inputs[g]);
                            for (int j = 0; j < brain[brain.Count - 1].Count; j++)
                            {
                                brain[brain.Count - 1][j].pull += (outputs[g][j] - functions.sigmoid(brain[brain.Count - 1][j].value));
                            }
                            for (int j = 0; j < brain[brain.Count - 1].Count; j++)
                            {
                                pullNeuron(brain.Count - 1, j);
                            }
                        }
                        for (int layer = brain.Count-1; layer > 0; layer --)
                        {
                            
                            for (int n1 = brain[layer].Count-1; n1 >= 0; n1--)
                            {
                                for (int n2 = brain[layer-1].Count-1; n2 > 0; n2--)
                                {
                                    //Console.WriteLine(brain[layer - 1][n2].pull);
                                    brain[layer][n1].weights[n2] -= brain[layer-1][n2].pull;
                                    //Console.WriteLine(brain[b - 1][g].pull);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Wrong num of output dimensions. Expected: " + brain[brain.Count - 1].Count + ", Recieved: " + outputs[0].Count);
                }
            }
            else {
                Console.WriteLine("Num outputs does not match num inputs. Expected: " + inputs.Count + ", Recieved: " + outputs.Count);
            }

        }
        public static void pullNeuron(int layer, int index) {
            for (int g = 0; g < brain[layer-1].Count; g++)
            {
                if (layer - 1 > 0) {
                    brain[layer - 1][g].pull += (brain[layer][index].pull)*functions.derivative(layer,index,g)/brain[layer - 1].Count;
                    pullNeuron(layer-1,g);
                }
            }
        }
    }
    static class functions {
        public static decimal sigmoid(decimal toSigmoid) {
            if (toSigmoid > 50) {
                return 1;
            } else if (toSigmoid < -50) {
                return 0;
            }
            return 1 / (1 + (decimal)Math.Pow(Math.E, (double)(-toSigmoid)));
        }
        public static decimal derivative( int nLayer, int nIndex, int wIndex )
        {
            decimal oldVal = network.brain[nLayer][nIndex].value;
            network.brain[nLayer][nIndex].weights[wIndex] += network.step;
            network.brain[nLayer][nIndex].run(network.brain[nLayer-1]);
            decimal newVal = network.brain[nLayer][nIndex].value;
            network.brain[nLayer][nIndex].weights[wIndex] -= network.step;
            network.brain[nLayer][nIndex].run(network.brain[nLayer - 1]);
            return newVal-oldVal;
        }
    }
    class neuron {
        public decimal pull = 0;
        public decimal value = 0;
        public List<decimal> weights = new List<decimal>();
        public int typeOfNeuron = 0;
        public neuron(int numInputs) {
            for (int i = 0; i < numInputs; i++) {
                weights.Add((decimal)1);
            }
        }
        public void run(List<decimal> input)
        {
            switch (typeOfNeuron)
            {
                case 0:
                    value = 1;
                    for (int i = 0; i < weights.Count - 1; i++)
                    {
                        value *= weights[i] * functions.sigmoid(input[i]);
                    }
                    break;
                case 1:
                    value = 0;
                    for (int i = 0; i < weights.Count - 1; i++)
                    {
                        value += weights[i] * functions.sigmoid(input[i]);
                    }
                    break;
            }
        }
        public void run(List<neuron> input)
        {
            
            switch (typeOfNeuron)
            {
                case 0:
                    value = 1;
                    for (int i = 0; i < input.Count-1; i++)
                    {
                        Console.WriteLine("weight: "+weights[i]+", input"+input[i].value);
                        value *= weights[i] * input[i].value;
                    }
                    value *= weights[weights.Count - 1];
                    break;
                case 1:
                    value = 0;
                    for (int i = 0; i < input.Count-1; i++)
                    {
                        value += weights[i] * input[i].value;
                    }
                    value += weights[weights.Count - 1];
                    break;
            }
            value = functions.sigmoid(value);
        }
    }
    static class analyze {
        public static void writeNeuron(int layer, int index) {
            neuron n = network.brain[layer][index];
            string output = "Layer: "+layer+", Node "+index+": ";
            List<decimal> w = n.weights;
            for (int i = 0; i < w.Count; i++) {
                output += "w"+i+": "+w[i] + ", ";
            }
            output += "Value: " + functions.sigmoid(n.value) + ", ";
            output += "Pull: " + n.pull;
            Console.WriteLine(output);
        }
    }
}