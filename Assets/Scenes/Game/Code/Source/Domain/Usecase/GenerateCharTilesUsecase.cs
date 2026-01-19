using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class GenerateCharTilesUsecase
{
    private readonly GetTileAdjustedScoreUsecase getTileAdjustedScoreUsecase;
    private readonly LetterDistributionRepository letterDistributionRepository;
    private readonly IPlayerManager playerManager;
    private readonly Random random = new();

    [Inject]
    public GenerateCharTilesUsecase(
        GetTileAdjustedScoreUsecase getTileAdjustedScoreUsecase,
        LetterDistributionRepository letterDistributionRepository,
        IPlayerManager playerManager
        )
    {
        this.getTileAdjustedScoreUsecase = getTileAdjustedScoreUsecase;
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
                    getTileAdjustedScoreUsecase.Invoke(t.Value, t.Score)
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
