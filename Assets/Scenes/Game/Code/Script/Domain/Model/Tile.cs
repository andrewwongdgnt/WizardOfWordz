using System;
using UnityEngine;

public class Tile
{
    public char value;
    public bool pickable;

    public Tile(char value)
    {
        this.value = value;
        pickable = true;
    }

    public override string ToString()
    {
        string valueAsString = value.ToString();
        string valueWithVisualIndicator = pickable ? valueAsString.ToUpper() : valueAsString.ToLower();
        return valueWithVisualIndicator;
    }
}
