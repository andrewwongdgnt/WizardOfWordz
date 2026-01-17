using TMPro;
using UnityEngine;

public class HealthGameObject : MonoBehaviour
{

    public TextMeshProUGUI healthText;

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
        if (playerManager == null)
        {
            return;
        }
        UpdateHealth(playerManager);
    }

    public void Init(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        UpdateHealth(playerManager);
    }

    private void UpdateHealth(PlayerManager playerManager)
    {
        healthText.text = $"{playerManager.CurrentHealth}/{playerManager.MaxHealth}";
    }
}
