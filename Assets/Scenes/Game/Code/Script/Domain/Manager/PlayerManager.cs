using Zenject;

public class PlayerManager
{
    private readonly PlayerInfo playerInfo;

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
        CurrentHealth = playerInfo.health;
        TileCount = playerInfo.tileCount;
    }

    public void UpdateHealthBy(int value)
    {
        CurrentHealth += value;
    }
}