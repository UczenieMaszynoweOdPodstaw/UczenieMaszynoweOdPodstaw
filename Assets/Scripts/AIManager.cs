using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] int carsCount;
    [SerializeField] AICar carPrefab;
    [SerializeField] float maxRunTime;
    List<AICar> aiCars;
    Generation currentGeneration;
    float runStartTime;

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
        runStartTime = Time.time;
        for (int i = 0; i < aiCars.Count; i++)
        {
            aiCars[i].SteeringModel = currentGeneration.SteeringModels[i];
            aiCars[i].GetComponent<RewardCalculator>().RunStartTime = runStartTime;
            ResetCar(aiCars[i]);
        }
    }

    void ResetCar(AICar car)
    {
        car.transform.position = Vector3.zero;
        car.transform.rotation = Quaternion.identity;
        car.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        car.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        car.GetComponent<RewardCalculator>().CheckpointsPassed = new Dictionary<int, float>();
        car.GetComponent<RewardCalculator>().CollisionCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > runStartTime + maxRunTime)
        {
            var startGeneration = new Generation();
            startGeneration.InitRandom(carsCount);
            StartNewRun(startGeneration);
        }
    }
}
