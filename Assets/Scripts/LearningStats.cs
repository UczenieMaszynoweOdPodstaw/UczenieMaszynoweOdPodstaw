using System.Linq;
using UnityEngine;

public class LearningStats : MonoBehaviour
{
    public int GenerationNumber { get; set; }
    public float BestReward { get; set; }
    public float AverageReward { get; set; }

    private void Awake()
    {
        GenerationNumber = 1;
    }

    public void UpdateStats(Generation gen)
    {
        GenerationNumber++;
        BestReward = gen.SteeringModels.Max(sm => sm.Reward);
        AverageReward = gen.SteeringModels.Select(sm => sm.Reward).Sum() / gen.SteeringModels.Count;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Generation: {GenerationNumber}");
        GUI.Label(new Rect(10, 30, 200, 20), $"Best: {BestReward.ToString("F0")}");
        GUI.Label(new Rect(10, 50, 200, 20), $"Average: {AverageReward.ToString("F0")}");
    }
}
