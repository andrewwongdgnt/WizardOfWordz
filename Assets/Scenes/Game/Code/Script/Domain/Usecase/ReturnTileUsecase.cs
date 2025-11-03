using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReturnTileUsecase
{
    public void Invoke(char removedChar, List<Tile> allowedTiles)
    {
        Tile foundTile = allowedTiles.FindLast(t => t.value == removedChar && !t.pickable);
        if (foundTile != null)
        {
            foundTile.pickable = true;
        }
    }
}
