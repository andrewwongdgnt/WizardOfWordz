using TMPro;
using UnityEngine;

public class HealthGameObject : MonoBehaviour
{

    public TextMeshProUGUI healthText;

    private IPlayerManager playerManager;

    public void UpdateState()
    {
        if (playerManager == null)
        {
            return;
        }
        UpdateHealth(playerManager);
    }

    public void Init(IPlayerManager playerManager)
    {
        this.playerManager = playerManager;
        UpdateHealth(playerManager);
    }

    private void UpdateHealth(IPlayerManager playerManager)
    {
        healthText.text = $"{playerManager.CurrentHealth}/{playerManager.MaxHealth}";
    }
}
