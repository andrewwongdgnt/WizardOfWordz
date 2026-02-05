using System;

public class MockTileUtil
{
    public static TileInfo GenerateTileInfo(
        char value = 'A',
        int count = 1,
        int score = 1,
        Action<TileInfo>? action = null
        )
    {
        TileInfo tileInfo = new(
                value: value,
                count: count,
                score: score
            );
        action?.Invoke(tileInfo);
        return tileInfo;
    }

    public static Tile GenerateTile(
        char value = 'A',
        int score = 1,
        bool pickable = true
        )
    {
        return new(
            value: value,
            score: score
            )
        {
            pickable = pickable
        };
    }
}