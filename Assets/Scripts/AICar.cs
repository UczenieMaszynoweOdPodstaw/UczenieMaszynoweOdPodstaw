using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AICar : MonoBehaviour
{
    CarController carController;
    Action[] actions;

    void Awake()
    {
        carController = GetComponent<CarController>();
        actions = new Action[]
        {
            carController.Accelerate,
            carController.TurnLeft,
            carController.TurnRight,
            carController.Brake,
        };
    }

    void FixedUpdate()
    {
        var action = GetNextAction();
        action();
    }

    Action GetNextAction()
    {
        return actions[Random.Range(0, CarController.ACTION_COUNT)];
    }
}
