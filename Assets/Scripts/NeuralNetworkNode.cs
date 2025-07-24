using System.Linq;
using UnityEngine;

public class NeuralNetworkNode
{
    public float[] Weights { get; set; }
    public float Bias { get; set; }
    public float Value { get; private set; }

    public void CalculateValue(float[] inputs)
    {
        var weightedInputs = new float[inputs.Length];
        for (int i = 0; i < weightedInputs.Length; i++)
        {
            weightedInputs[i] = inputs[i] * Weights[i];
        }

        Value = ReLU(weightedInputs.Sum() + Bias);
    }

    float ReLU(float input)
    {
        return Mathf.Max(0, input);
    }
}
