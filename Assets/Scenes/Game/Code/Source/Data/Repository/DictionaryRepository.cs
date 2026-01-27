using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DictionaryRepository : IDictionaryRepository
{
    private readonly TextAsset csvFile = Resources.Load<TextAsset>("Dictionary");

    public List<Word> Get()
    {
        List<string[]> parts = CSVHelper.parse(csvFile);

        return parts.Select(p =>
        {
            string word = p[1].Trim();
            string tag = p[2].Trim();

            return new Word(
                word,
                tag
                );

        }).ToList();
    }


}
