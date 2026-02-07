using NUnit.Framework;
using System;
using System.Collections.Generic;

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

    public static List<TileInfo> GenerateMockTileInfoList()
    {
        return new List<TileInfo>()
        {
            GenerateTileInfo('A', score: 1),
            GenerateTileInfo('B', score: 3),
            GenerateTileInfo('C', score: 3),
            GenerateTileInfo('D', score: 2),
            GenerateTileInfo('E', score: 1),
            GenerateTileInfo('F', score: 4),
            GenerateTileInfo('G', score: 2),
            GenerateTileInfo('H', score: 4),
            GenerateTileInfo('I', score: 1),
            GenerateTileInfo('J', score: 8),
            GenerateTileInfo('K', score: 5),
            GenerateTileInfo('L', score: 1),
            GenerateTileInfo('M', score: 3),
            GenerateTileInfo('N', score: 1),
            GenerateTileInfo('O', score: 1),
            GenerateTileInfo('P', score: 3),
            GenerateTileInfo('Q', score: 10),
            GenerateTileInfo('R', score: 1),
            GenerateTileInfo('S', score: 1),
            GenerateTileInfo('T', score: 1),
            GenerateTileInfo('U', score: 1),
            GenerateTileInfo('V', score: 4),
            GenerateTileInfo('W', score: 4),
            GenerateTileInfo('X', score: 8),
            GenerateTileInfo('Y', score: 4),
            GenerateTileInfo('Z', score: 10)
        };
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