using UnityEngine;

public class PlayerStatsContainerGameObject : MonoBehaviour
{
    public HealthGameObject healthGO;

    public void UpdateState()
    {
        healthGO.UpdateState();
    }

    public void SetUp(IPlayerManager playerManager)
    {
        healthGO.Init(playerManager);
    }
}
