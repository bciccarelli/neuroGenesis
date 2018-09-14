using System;
using System.IO;
using System.Collections.Generic;

using System.Threading;
namespace aiNameSpace
{

    class neuralData {
        public List<object[]> inputData;
        public List<object> outputData;

        public Type objectData;
        public neuralData(List<object[]> input, List<object> output) {
            objectData = input[0].GetType();
            inputData = input;
            outputData = output;
        }
    }
    class neuralNetwork {
        public int max;
        public neuralData data;
        public bool useMemory;
        public neuralNetwork(neuralData Data, int maxNumOfNodes) {
            max = maxNumOfNodes;
            data = Data;
        }
        public cull() {

        }
    }
    class node {
        public Type input;
        public Type output;
        public node(Type input, Type output)
        {
            this.input = input;
            this.output = output;
        }
    }
}