using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class GenerateCharTilesUsecase
{
    private readonly LetterDistributionRepository letterDistributionRepository;
    private readonly PlayerManager playerManager;
    private readonly Random random = new();

    [Inject]
    public GenerateCharTilesUsecase(
        LetterDistributionRepository letterDistributionRepository,
        PlayerManager playerManager
        )
    {
        this.letterDistributionRepository = letterDistributionRepository;
        this.playerManager = playerManager;
    }

    public List<Tile> Invoke()
    {
        int tileCount = playerManager.TileCount;
        List<Tile> tiles = new();

        letterDistributionRepository.Get().ForEach(t =>
        {
            foreach (var item in Enumerable.Repeat(t, t.Count))
            {
                tiles.Add(new Tile(
                    t.Value,
                    t.Score
                    ));
            }
        });

        return Enumerable.Repeat(tiles, tileCount)
            .Select(ts =>
                {
                    int index = random.Next(ts.Count);
                    Tile pickedTile = ts[index];
                    ts.RemoveAt(index);
                    return pickedTile;
                }
            )
            .ToList();
    }

}
