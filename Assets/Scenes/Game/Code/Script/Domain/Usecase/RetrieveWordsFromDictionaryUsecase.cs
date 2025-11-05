using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[System.Serializable]
public class RetrieveWordsFromDictionaryUsecase
{
    private readonly DictionaryRepository dictionaryRepository;

    [Inject]
    public RetrieveWordsFromDictionaryUsecase(
        DictionaryRepository dictionaryRepository
        )
    {
        this.dictionaryRepository = dictionaryRepository;
    }

    public List<Word> Invoke()
    {
        return dictionaryRepository.Get()
            .Select(w => new Word(w.Value, w.Tag))
            .ToList();
    }

}
