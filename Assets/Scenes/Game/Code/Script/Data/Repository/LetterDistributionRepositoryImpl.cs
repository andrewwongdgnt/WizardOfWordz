using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LetterDistributionRepositoryImpl : LetterDistributionRepository
{
    private readonly TextAsset csvFile = Resources.Load<TextAsset>("LetterDistribution");

    public List<TileInfo> Get()
    {
        List<string[]> parts = CSVHelper.parse(csvFile);

        return parts.Select(p =>
        {
            string letter = p[0].Trim();
            string count = p[1].Trim();
            string score = p[2].Trim();

            return new TileInfo(
                char.Parse(letter),
                int.Parse(count),
                int.Parse(score)
                );

        }).ToList();
    }
}
