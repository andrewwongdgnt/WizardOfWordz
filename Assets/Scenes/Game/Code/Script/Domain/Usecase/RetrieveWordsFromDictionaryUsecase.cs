using System.Collections.Generic;
using System.Linq;
using Zenject;

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

    public Dictionary<string, Word> Invoke()
    {
        return dictionaryRepository.Get().ToDictionary(w => w.Value.ToUpper(), w => w );
    }

}
