using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GenerateCharTilesUsecase
{
    private readonly LetterDistributionRepository letterDistributionRepository;

    [Inject]
    public GenerateCharTilesUsecase(
        LetterDistributionRepository letterDistributionRepository
        )
    {
        this.letterDistributionRepository = letterDistributionRepository;
    }

    public List<Tile> Invoke()
    {
        int num = 8;

        List<Tile> tiles = new();

        letterDistributionRepository.Get().ForEach(tile =>
        {
            foreach (var item in Enumerable.Repeat(tile, tile.Count))
            {
                tiles.Add(tile.Clone());
            }
        });

        System.Random random = new();
        return Enumerable.Repeat(tiles, num)
            .Select(s =>
                {
                    return s[random.Next(s.Count)];
                }
            )
            .ToList();
    }

}
