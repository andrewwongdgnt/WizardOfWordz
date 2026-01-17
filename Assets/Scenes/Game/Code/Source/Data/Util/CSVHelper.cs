
using ModestTree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSVHelper
{
    public static List<string[]> parse(TextAsset csv)
    {
        if (csv == null)
        {
            Debug.LogError("CSV file not found!");
            return new();
        }

        string[] lines = csv.text.Split('\n');

        List<string[]> result = lines
            .Select(l => l.Split(','))
            .Where(l => !l.IsEmpty())
            .ToList();
        return result;
    }
}
