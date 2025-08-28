using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm
{
    const float TOP_PERCENT_SELECTION = 0.5f;
    const float MIN_MUTATION_STRENGHT = 0.5f;
    const float MAX_MUTATION_STRENGHT = 1.5f;
    const float MUTATION_CHANCE = 0.05f;
    public Generation GetNextGeneration(Generation currentGen) //currentGen is already evaluated here!
    {
        var nextGeneration = new Generation();
        var currentGenOrdered = GetCopy(currentGen).SteeringModels.OrderByDescending(m => m.Reward).ToList();

        //selection
        var selectedModels = currentGenOrdered.Take((int)(currentGenOrdered.Count * TOP_PERCENT_SELECTION)).ToList();
        var bestModel = currentGenOrdered.First();

        //crossover
        while (nextGeneration.SteeringModels.Count < currentGen.SteeringModels.Count)
        {
            var parent1 = selectedModels[Random.Range(0, selectedModels.Count)];
            var parent2 = selectedModels.Where(sm => sm != parent1).ToList()[Random.Range(0, selectedModels.Count - 1)];
            var children = CreateChildren(parent1, parent2);
            nextGeneration.SteeringModels.AddRange(children);
        }

        //mutation
        foreach (var model in nextGeneration.SteeringModels)
        {
            if (Random.value < MUTATION_CHANCE)
            {
                MutateModel(model);
            }
        }

        nextGeneration.SteeringModels.Remove(nextGeneration.SteeringModels.Last());
        nextGeneration.SteeringModels.Add(GetCopy(bestModel));

        return nextGeneration;
    }

    List<SteeringModel> CreateChildren(SteeringModel parent1, SteeringModel parent2)
    {
        var child1 = GetCopy(parent1);
        var child2 = GetCopy(parent2);

        if (Random.value < 0.5f)
        {
            //swap hidden layer weights
            var child1HiddenLayerWeights = child1.Network.HiddenLayer.SelectMany(hl => hl.Weights).ToArray();
            var child2HiddenLayerWeights = child2.Network.HiddenLayer.SelectMany(hl => hl.Weights).ToArray();

            for (int i = 0; i < child1HiddenLayerWeights.Length; i++)
            {
                if (Random.value < 0.5f)
                {
                    var temp = child1HiddenLayerWeights[i];
                    child1HiddenLayerWeights[i] = child2HiddenLayerWeights[i];
                    child2HiddenLayerWeights[i] = temp;
                }
            }

            for (int i = 0; i < NeuralNetwork.HIDDEN_LAYER_NODES; i++)
            {
                var weights1 = child1HiddenLayerWeights.Skip(i * NeuralNetwork.INPUT_COUNT).Take(NeuralNetwork.INPUT_COUNT).ToArray();
                child1.Network.HiddenLayer[i].Weights = weights1;
                var weights2 = child2HiddenLayerWeights.Skip(i * NeuralNetwork.INPUT_COUNT).Take(NeuralNetwork.INPUT_COUNT).ToArray();
                child2.Network.HiddenLayer[i].Weights = weights2;
            }
        }
        else 
        {
            //swap output layer weights
            var child1OutputLayerWeights = child1.Network.OutputLayer.SelectMany(hl => hl.Weights).ToArray();
            var child2OutputLayerWeights = child2.Network.OutputLayer.SelectMany(hl => hl.Weights).ToArray();

            for (int i = 0; i < child1OutputLayerWeights.Length; i++)
            {
                if (Random.value < 0.5f)
                {
                    var temp = child1OutputLayerWeights[i];
                    child1OutputLayerWeights[i] = child2OutputLayerWeights[i];
                    child2OutputLayerWeights[i] = temp;
                }
            }

            for (int i = 0; i < CarController.ACTION_COUNT; i++)
            {
                var weights1 = child1OutputLayerWeights.Skip(i * NeuralNetwork.HIDDEN_LAYER_NODES).Take(NeuralNetwork.HIDDEN_LAYER_NODES).ToArray();
                child1.Network.OutputLayer[i].Weights = weights1;
                var weights2 = child2OutputLayerWeights.Skip(i * NeuralNetwork.HIDDEN_LAYER_NODES).Take(NeuralNetwork.HIDDEN_LAYER_NODES).ToArray();
                child2.Network.OutputLayer[i].Weights = weights2;
            }
        }

        return new List<SteeringModel> { child1, child2 };
    }

    void MutateModel(SteeringModel model)
    {
        var nodes = model.Network.HiddenLayer.Concat(model.Network.OutputLayer).ToArray();
        var mutations = Random.Range(1, 6);

        for (int i = 0; i < mutations; i++)
        {
            var nodeToMutate = nodes[Random.Range(0, nodes.Length)];
            var mutationStrenght = Random.Range(MIN_MUTATION_STRENGHT, MAX_MUTATION_STRENGHT);
            var weightIndex = Random.Range(0, nodeToMutate.Weights.Length);
            nodeToMutate.Weights[weightIndex] *= mutationStrenght;
            nodeToMutate.Weights[weightIndex] += Random.Range(-0.1f, 0.1f);
            if (Random.value < 0.3f)
            {
                nodeToMutate.Weights[weightIndex] *= -1;
            }
        }
    }

    T GetCopy<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
