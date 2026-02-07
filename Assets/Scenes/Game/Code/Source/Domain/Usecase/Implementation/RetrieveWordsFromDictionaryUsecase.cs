using System.Collections.Generic;
using System.Linq;
using Zenject;

public class RetrieveWordsFromDictionaryUsecase : IRetrieveWordsFromDictionaryUsecase
{
    private readonly IDictionaryRepository dictionaryRepository;

    [Inject]
    public RetrieveWordsFromDictionaryUsecase(
        IDictionaryRepository dictionaryRepository
        )
    {
        this.dictionaryRepository = dictionaryRepository;
    }

    public Dictionary<string, Word> Invoke()
    {
        return dictionaryRepository.Get().ToDictionary(w => w.Value.ToUpper(), w => w);
    }

}
