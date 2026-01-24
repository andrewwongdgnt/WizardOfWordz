using TMPro;
using UnityEngine;

public class HealthGameObject : MonoBehaviour
{

    public TextMeshProUGUI healthText;

    private PlayerManager playerManager;

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
