using Zenject;

public class PlayerManager
{
    private readonly PlayerInfo playerInfo;


    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int TileCount { get; private set; }

    [Inject]
    public PlayerManager(
        PlayerInfoRepository playerInfoRepository
        )
    {
        playerInfo = playerInfoRepository.Get();
    }

    public virtual void Init()
    {
        MaxHealth = playerInfo.health;
        CurrentHealth = playerInfo.health;
        TileCount = playerInfo.tileCount;
    }

    public virtual void UpdateHealthBy(int value)
    {
        CurrentHealth += value;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
        else if (CurrentHealth < 0)
            CurrentHealth = 0;
    }

    public virtual bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    // TODO temp code
    public virtual void FullHeath()
    {
        CurrentHealth = MaxHealth;
    }

    public virtual void HandleReward(Reward reward)
    {
        switch(reward.RewardEnum)
        {
            case RewardEnum.MaxHealth:
                IncreaseMaxHealth(reward.GetCurrentValue().Value);
                break;
            case RewardEnum.MaxTile:
                IncreaseTileCount(reward.GetCurrentValue().Value);
                break;
        }
    }

    private void IncreaseMaxHealth(int value)
    {
        MaxHealth += value;
        CurrentHealth += value;
    }

    private void IncreaseTileCount(int value)
    {
        TileCount += value;
    }
}