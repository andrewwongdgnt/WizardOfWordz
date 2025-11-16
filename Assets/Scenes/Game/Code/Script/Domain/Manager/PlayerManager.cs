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

    public void Init()
    {
        MaxHealth = playerInfo.health;
        CurrentHealth = playerInfo.health;
        TileCount = playerInfo.tileCount;
    }

    public void UpdateHealthBy(int value)
    {
        CurrentHealth += value;
    }
}