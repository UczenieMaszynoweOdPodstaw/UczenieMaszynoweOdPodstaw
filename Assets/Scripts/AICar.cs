using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AICar : MonoBehaviour
{
    CarController carController;
    Action[] actions;
    NeuralNetwork neuralNetwork;
    CarStateProvider carStateProvider;

    void Awake()
    {
        carController = GetComponent<CarController>();
        carStateProvider = GetComponent<CarStateProvider>();
        actions = new Action[]
        {
            carController.Accelerate,
            carController.TurnLeft,
            carController.TurnRight,
            carController.Brake,
        };
        neuralNetwork = new NeuralNetwork();
        neuralNetwork.InitRandom();
    }

    void FixedUpdate()
    {
        var action = GetNextAction();
        action();
    }

    Action GetNextAction()
    {
        var carStateAsParams = MapCarStateToInputArray(carStateProvider.GetCarState());
        var neuralNetworkOutput = neuralNetwork.CalculateOutput(carStateAsParams);
        return actions[neuralNetworkOutput];
    }

    float[] MapCarStateToInputArray(CarState state)
    {
        return new float[]
        {
            state.Speed,
            state.LeftDistance,
            state.LeftFrontDistance,
            state.FrontDistance,
            state.RightFrontDistance,
            state.RightDistance
        };
    }
}
