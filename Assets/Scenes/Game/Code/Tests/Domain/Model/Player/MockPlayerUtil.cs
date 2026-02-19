public class MockPlayerUtil
{
    public const int DEFAULT_HEALTH = 100;
    public const int DEFAULT_TILE_COUNT = 7;


    public static PlayerInfo GeneratePlayerInfo(
            int health = DEFAULT_HEALTH,
            int tileCount = DEFAULT_TILE_COUNT
        )
    {
        return new PlayerInfo
        {
            health = health,
            tileCount = tileCount
        };
    }
}