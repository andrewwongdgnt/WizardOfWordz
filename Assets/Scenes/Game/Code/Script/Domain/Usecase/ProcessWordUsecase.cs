


using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ProcessWordUsecase
{
    public void Invoke(
        string word,
        List<Word> dictionary
        )
    {
        Word foundWord = dictionary.Find(w => w.value.ToLower() == word.ToLower());

        Debug.Log(foundWord != null ? $"{word} is a word and it is {foundWord.posTag}" : $"{word} is not a word");
    }
}