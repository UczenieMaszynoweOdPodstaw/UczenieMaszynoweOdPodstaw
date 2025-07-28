using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AICar : MonoBehaviour
{
    public SteeringModel SteeringModel { get; set; }
    CarController carController;
    Action[] actions;
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
    }

    void FixedUpdate()
    {
        var action = GetNextAction();
        action();
    }

    Action GetNextAction()
    {
        var carStateAsParams = MapCarStateToInputArray(carStateProvider.GetCarState());
        var neuralNetworkOutput = SteeringModel.Network.CalculateOutput(carStateAsParams);
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
