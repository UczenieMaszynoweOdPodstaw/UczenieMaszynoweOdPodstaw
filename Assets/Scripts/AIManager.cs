using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] int carsCount;
    [SerializeField] AICar carPrefab;
    [SerializeField] float maxRunTime;
    [SerializeField] float timeScale;
    [SerializeField] bool startWithBestModelEver;
    List<AICar> aiCars;
    GeneticAlgorithm geneticAlgorithm;
    Generation currentGeneration;
    float runStartTime;

    void Awake()
    {
        Time.timeScale = timeScale;
        geneticAlgorithm = new GeneticAlgorithm();
        var startGeneration = new Generation();
        startGeneration.InitRandom(carsCount);
        if (startWithBestModelEver && FileManager.ReadModelFromJsonFile() != null)
        {
            startGeneration.SteeringModels.Remove(startGeneration.SteeringModels.Last());
            startGeneration.SteeringModels.Add(FileManager.ReadModelFromJsonFile());
        }
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
            aiCars[i].SteeringModel.Reward = 0;
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
            EvaluateCars();
            TrySaveBestModelEver();
            GetComponent<LearningStats>().UpdateStats(currentGeneration);
            var nextGeneration = geneticAlgorithm.GetNextGeneration(currentGeneration);
            StartNewRun(nextGeneration);
        }
    }

    void EvaluateCars()
    {
        foreach (var car in aiCars)
        {
            car.SteeringModel.Reward = car.GetComponent<RewardCalculator>().CalculateReward();
        }
    }

    void TrySaveBestModelEver()
    {
        var bestModelEver = FileManager.ReadModelFromJsonFile();
        var bestModelFromCurrentGen = currentGeneration.SteeringModels.OrderByDescending(sm => sm.Reward).ToList().First();
        if (bestModelEver == null || bestModelFromCurrentGen.Reward > bestModelEver.Reward)
        {
            FileManager.SaveModelToJsonFile(bestModelFromCurrentGen);
            Debug.Log("New best model ever! Reward: " + bestModelFromCurrentGen.Reward);
        }
    }
}
