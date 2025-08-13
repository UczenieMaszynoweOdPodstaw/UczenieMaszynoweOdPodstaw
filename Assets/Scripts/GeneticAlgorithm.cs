using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm
{
    const float TOP_PERCENT_SELECTION = 0.5f;
    public Generation GetNextGeneration(Generation currentGen) //currentGen is already evaluated here!
    {
        var nextGeneration = new Generation();
        var currentGenOrdered = GetCopy(currentGen).SteeringModels.OrderByDescending(m => m.Reward).ToList();

        //selection
        var selectedModels = currentGenOrdered.Take((int)(currentGenOrdered.Count * TOP_PERCENT_SELECTION));
        var bestModel = currentGenOrdered.First();

        //crossover
        //mutation

        return nextGeneration;
    }

    T GetCopy<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
