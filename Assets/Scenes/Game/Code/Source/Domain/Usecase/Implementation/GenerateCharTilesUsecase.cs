using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class GenerateCharTilesUsecase: IGenerateCharTilesUsecase
{
    private readonly IGetTileAdjustedScoreUsecase getTileAdjustedScoreUsecase;
    private readonly ILetterDistributionRepository letterDistributionRepository;
    private readonly IPlayerManager playerManager;
    private readonly IGenerateRandomNumberUsecase generateRandomNumberUsecase;

    [Inject]
    public GenerateCharTilesUsecase(
        IGetTileAdjustedScoreUsecase getTileAdjustedScoreUsecase,
        ILetterDistributionRepository letterDistributionRepository,
        IPlayerManager playerManager,
        IGenerateRandomNumberUsecase generateRandomNumberUsecase
        )
    {
        this.getTileAdjustedScoreUsecase = getTileAdjustedScoreUsecase;
        this.letterDistributionRepository = letterDistributionRepository;
        this.playerManager = playerManager;
        this.generateRandomNumberUsecase = generateRandomNumberUsecase;
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
                    int index = generateRandomNumberUsecase.Invoke(ts.Count);
                    Tile pickedTile = ts[index];
                    ts.RemoveAt(index);
                    return pickedTile;
                }
            )
            .ToList();
    }

}
