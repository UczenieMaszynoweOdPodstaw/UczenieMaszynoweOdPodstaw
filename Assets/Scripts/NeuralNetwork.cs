using System.Linq;
using UnityEngine;

public class NeuralNetwork
{
    public const int INPUT_COUNT = 6; 
    public const int HIDDEN_LAYER_NODES = 8; 
    public NeuralNetworkNode[] HiddenLayer { get; set; }
    public NeuralNetworkNode[] OutputLayer { get; set; }

    public void InitRandom()
    {
        HiddenLayer = new NeuralNetworkNode[HIDDEN_LAYER_NODES];
        for (int i = 0; i < HiddenLayer.Length; i++)
        {
            var node = new NeuralNetworkNode();
            node.Weights = new float[INPUT_COUNT];
            for (int j = 0; j < node.Weights.Length; j++)
            {
                node.Weights[j] = Random.Range(-1f, 1f);
            }
            node.Bias = Random.Range(-10f, 10f);
            HiddenLayer[i] = node;
        }

        OutputLayer = new NeuralNetworkNode[CarController.ACTION_COUNT];
        for (int i = 0; i < OutputLayer.Length; i++)
        {
            var node = new NeuralNetworkNode();
            node.Weights = new float[HIDDEN_LAYER_NODES];
            for (int j = 0; j < node.Weights.Length; j++)
            {
                node.Weights[j] = Random.Range(-1f, 1f);
            }
            node.Bias = Random.Range(-10f, 10f);
            OutputLayer[i] = node;
        }
    }

    public int CalculateOutput(float[] inputs)
    {
        foreach (var node in HiddenLayer)
            node.CalculateValue(inputs);

        foreach (var node in OutputLayer)
            node.CalculateValue(HiddenLayer.Select(n => n.Value).ToArray());

        var highestValueNode = OutputLayer.OrderByDescending(on => on.Value).First();
        return OutputLayer.ToList().IndexOf(highestValueNode);
    }
}
