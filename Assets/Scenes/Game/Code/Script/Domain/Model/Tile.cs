using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Tile
{
    public char Value { get; }
    public int Count { get; }
    public int Score {get;}
    public bool pickable;

    public Tile(
        char value,
        int count,
        int score
        )
    {
        Value = value;
        Count = count;
        Score = score;
        pickable = true;
    }

    public Tile Clone()
    {
        return new Tile(
            Value,
            Count,
            Score);
    }

    public override string ToString()
    {
        string valueAsString = Value.ToString();
        string valueWithVisualIndicator = pickable ? valueAsString.ToUpper() : valueAsString.ToLower();
        return valueWithVisualIndicator;
    }
}
