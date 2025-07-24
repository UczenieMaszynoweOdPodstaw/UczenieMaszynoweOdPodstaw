using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] int carsCount;
    [SerializeField] AICar carPrefab;
    List<AICar> aiCars;

    void Awake()
    {
        InitAICars();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
