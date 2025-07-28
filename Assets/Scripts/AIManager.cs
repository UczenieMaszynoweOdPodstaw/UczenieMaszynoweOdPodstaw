using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] int carsCount;
    [SerializeField] AICar carPrefab;
    List<AICar> aiCars;
    Generation currentGeneration;

    void Awake()
    {
        var startGeneration = new Generation();
        startGeneration.InitRandom(carsCount);
        InitAICars();
        StartNewRun(startGeneration);
    }

    void InitAICars()
    {
        aiCars = new List<AICar>();
        for (int i = 0; i < carsCount; i++)
        {
            var car = Instantiate(carPrefab);
            aiCars.Add(car);
        }
    }

    void StartNewRun(Generation nextGeneration)
    {
        currentGeneration = nextGeneration;
        for (int i = 0; i < aiCars.Count; i++)
        {
            aiCars[i].SteeringModel = currentGeneration.SteeringModels[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
