using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RetrieveWordsFromDictionaryUsecase
{
    public TextAsset csvFile;

    public List<Word> Invoke()
    {
        List<Word> wordTags = new();

        if (csvFile == null)
        {
            Debug.LogError("CSV file not found!");
            return wordTags;
        }

        string[] lines = csvFile.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');
            if (parts.Length < 3)
                continue; // skip invalid lines

            string word = parts[1].Trim();
            string tag = parts[2].Trim();

            wordTags.Add(new Word(word, tag));
        }

        return wordTags;
    }

}
