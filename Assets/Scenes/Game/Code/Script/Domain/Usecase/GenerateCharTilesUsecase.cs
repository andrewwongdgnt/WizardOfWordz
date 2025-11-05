using System.Collections.Generic;
using System.Linq;
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

        letterDistributionRepository.Get().ForEach(letter =>
        {
            foreach (var item in Enumerable.Repeat(letter, letter.Count))
            {
                tiles.Add(new Tile(
                    item.Value
                    ));
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
