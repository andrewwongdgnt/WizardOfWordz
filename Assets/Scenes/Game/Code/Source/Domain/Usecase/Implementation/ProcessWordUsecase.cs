


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ProcessWordUsecase: IProcessWordUsecase
{
    private readonly IGetTileAdjustedScoreUsecase getTileAdjustedScoreUsecase;

    private readonly Dictionary<char, int> tileScoreMap;

    private readonly Dictionary<string, Word> dictionary;

    [Inject]
    public ProcessWordUsecase(
        ILetterDistributionRepository letterDistributionRepository,
        IGetTileAdjustedScoreUsecase getTileAdjustedScoreUsecase,
        IRetrieveWordsFromDictionaryUsecase retrieveWordsFromDictionaryUsecase
        )
    {
        dictionary = retrieveWordsFromDictionaryUsecase.Invoke();
        tileScoreMap = letterDistributionRepository.Get().ToDictionary(t => t.Value, t => t.Score);
        this.getTileAdjustedScoreUsecase = getTileAdjustedScoreUsecase;
    }
    public void Invoke(
        string word,
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
                    int adjustedScore = getTileAdjustedScoreUsecase.Invoke(c, tileScore);

                    return acc + adjustedScore;
                });
            Debug.Log($"{word} is a word worth {score} and it is {foundWord.Tag}");
            enemies[attackIndex].UpdateHealthBy(-score);
        }
        else
        {
            Debug.Log($"{word} is not a word");

        }

    }
}