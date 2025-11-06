


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ProcessWordUsecase
{
    private readonly LetterDistributionRepository letterDistributionRepository;

    private readonly Dictionary<char, int> tileScoreMap;

    [Inject]
    public ProcessWordUsecase(
        LetterDistributionRepository letterDistributionRepository
        )
    {
        tileScoreMap = letterDistributionRepository.Get().ToDictionary(t => t.Value, t => t.Score);
    }
    public void Invoke(
        string word,
        Dictionary<string, Word> dictionary,
        List<Enemy> enemies,
        int attackIndex
        )
    {

        dictionary.TryGetValue(word.ToUpper(), out Word foundWord);
        if (foundWord != null)
        {
            int score = word.ToUpper().ToCharArray()
                .Aggregate(0, (acc, c) =>
                {

                    tileScoreMap.TryGetValue(c, out int tileScore);

                    return acc + tileScore;
                });
            Debug.Log($"{word} is a word worth {score} and it is {foundWord.Tag}");
            enemies[attackIndex].TakeDamage(score);
        }
        else
        {
            Debug.Log($"{word} is not a word");

        }

    }
}