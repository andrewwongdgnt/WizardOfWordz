using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DictionaryRepositoryImpl : DictionaryRepository
{
    public List<WordEntity> Get()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Dictionary");

        List<string[]> parts = CSVHelper.parse(csvFile);

        return parts.Select(p =>
        {
            string word = p[1].Trim();
            string index = p[0].Trim();
            string tag = p[2].Trim();

            return new WordEntity(
                word,
                int.Parse(index),
                tag
                );

        }).ToList();
    }


}
