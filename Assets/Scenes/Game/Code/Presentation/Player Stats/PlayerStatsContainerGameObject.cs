using UnityEngine;

public class PlayerStatsContainerGameObject : MonoBehaviour
{
    public HealthGameObject healthGO;

    private PlayerManager playerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateState()
    {
        healthGO.UpdateState();
    }

    public void SetUp(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        healthGO.Init(playerManager);
    }
}
