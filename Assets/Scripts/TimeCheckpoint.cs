using UnityEngine;

public class TimeCheckpoint : MonoBehaviour
{
    public int checkpointNumber;

    private void OnTriggerEnter(Collider other)
    {
        var rewardCalculator = other.GetComponent<RewardCalculator>();
        if (rewardCalculator != null)
        {
            rewardCalculator.CheckpointPassed(checkpointNumber);
        }
    }
}
