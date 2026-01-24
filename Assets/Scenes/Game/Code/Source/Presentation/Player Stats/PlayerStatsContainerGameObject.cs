using UnityEngine;

public class PlayerStatsContainerGameObject : MonoBehaviour
{
    public HealthGameObject healthGO;

    public void UpdateState()
    {
        healthGO.UpdateState();
    }

    public void SetUp(PlayerManager playerManager)
    {
        healthGO.Init(playerManager);
    }
}
