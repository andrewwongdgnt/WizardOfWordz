using System.Collections.Generic;

public interface IRetrieveWordsFromDictionaryUsecase
{
    public Dictionary<string, Word> Invoke();
}
