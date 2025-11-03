using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[System.Serializable]
public class GenerateCharTilesUsecase
{

    public int num;

    public List<Tile> Invoke()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        System.Random random = new();
        return Enumerable.Repeat(chars, num)
            .Select(s =>
                {
                    char c = s[random.Next(s.Length)];
                    return new Tile(c);
                }
            )
            .ToList();
    }

}
