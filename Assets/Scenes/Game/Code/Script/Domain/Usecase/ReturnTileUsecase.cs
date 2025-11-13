using System.Collections.Generic;

public class ReturnTileUsecase
{
    public Tile Invoke(char removedChar, List<Tile> allowedTiles)
    {
        Tile foundTile = allowedTiles.FindLast(t => t.Value == removedChar && !t.pickable);
        if (foundTile != null)
        {
            foundTile.pickable = true;
            return foundTile;
        }
        return null;
    }
}
