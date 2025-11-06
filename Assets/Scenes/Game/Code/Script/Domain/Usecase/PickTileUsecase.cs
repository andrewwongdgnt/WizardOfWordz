using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PickTileUsecase
{

    public char Invoke(
        Key key,
        List<Tile> allowedTiles
        )
    {
        char c = GetCharFromKey(key);
        if (c == '\0') return '\0';

        Tile foundTile = allowedTiles.Find(t => t.Value == c && t.pickable);
        if (foundTile == null) return '\0';
        foundTile.pickable = false;
        return c;
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
