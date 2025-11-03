using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[System.Serializable]
public class GenerateCharTilesUsecase
{

    public TextAsset letterDist;

    public int num;

    public List<Tile> Invoke()
    {
        if (letterDist == null)
        {
            Debug.LogError("CSV file not found!");
            return new();
        }
        List<Tile> tiles = new();

        string[] lines = letterDist.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');
            if (parts.Length < 3)
                continue; // skip invalid lines

            string letter = parts[0].Trim();
            string count = parts[1].Trim();

            foreach (var item in Enumerable.Repeat(letter, int.Parse(count)))
            {
                tiles.Add(new Tile(char.Parse(item)));
            }
        }

        System.Random random = new();
        return Enumerable.Repeat(tiles, num)
            .Select(s =>
                {
                    return s[random.Next(s.Count)];
                }
            )
            .ToList();
    }

}
