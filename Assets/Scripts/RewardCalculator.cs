using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardCalculator : MonoBehaviour
{
    const float CHECKPOINT_REWARD = 1000;
    const float COLLISION_PENALTY = 500;

    public float RunStartTime { get; set; }
    public int CollisionCount { get; set; }
    public Dictionary<int, float> CheckpointsPassed { get; set; }

    private void Awake()
    {
        CheckpointsPassed = new Dictionary<int, float>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionCount++;
    }
    public float CalculateReward()
    {
        //reward for every checkpoint
        float reward = 0;
        foreach (var checkpoint in CheckpointsPassed)
        {
            reward += CHECKPOINT_REWARD - checkpoint.Value;
        }

        //penalty for every collision
        var totalCollisionPenalty = CollisionCount * COLLISION_PENALTY;

        //penalty for distance to next checkpoint
        var nextCheckpointNumber = CheckpointsPassed.Keys.Any() ? CheckpointsPassed.Keys.Max() + 1 : 1;
        var nextCheckpoint = FindObjectsByType<TimeCheckpoint>(FindObjectsSortMode.None).Single(c => c.checkpointNumber == nextCheckpointNumber);
        var distanceToNextCheckpoint = (nextCheckpoint.transform.position - transform.position).magnitude;

        return reward - totalCollisionPenalty - distanceToNextCheckpoint;
    }

    public void CheckpointPassed(int checkpointNumber)
    {
        if (!CheckpointsPassed.ContainsKey(checkpointNumber))
        {
            var previousCheckpointsTimeSum = CheckpointsPassed.Values.Sum();
            CheckpointsPassed.Add(checkpointNumber, Time.time - RunStartTime - previousCheckpointsTimeSum);
        }
    }
}
