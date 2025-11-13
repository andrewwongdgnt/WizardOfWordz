using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class PickTileUsecase
{

    public (char, Tile) Invoke(
        Key key,
        List<Tile> allowedTiles
        )
    {
        char c = GetCharFromKey(key);
        if (c == '\0') return ('\0', null);

        Tile foundTile = allowedTiles.Find(t => t.Value == c && t.pickable);
        if (foundTile == null) return ('\0', null);
        foundTile.pickable = false;
        return (c, foundTile);
    }

    public bool Invoke(
        Tile tile,
        List<Tile> allowedTiles
        )
    {
        bool exists = allowedTiles.Any(t => t == tile);
        if (exists)
        {
            tile.pickable = !tile.pickable;
        }
        return exists;
    }

    private char GetCharFromKey(Key key)
    {
        if (key >= Key.A && key <= Key.Z)
        {
            // 'A' + (offset from Key.A)
            char baseChar = (char)('A' + (key - Key.A));
            return baseChar;
        }

        return '\0'; // null character if not A–Z
    }

}
