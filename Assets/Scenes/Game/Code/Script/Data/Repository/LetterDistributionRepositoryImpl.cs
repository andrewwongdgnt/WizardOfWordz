using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LetterDistributionRepositoryImpl : LetterDistributionRepository
{

    public List<Tile> Get()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("LetterDistribution");

        List<string[]> parts = CSVHelper.parse(csvFile);

        return parts.Select(p =>
        {
            string letter = p[0].Trim();
            string count = p[1].Trim();
            string score = p[2].Trim();

            return new Tile(
                char.Parse(letter),
                int.Parse(count),
                int.Parse(score)
                );

        }).ToList();
    }
}
