using System.Collections.Generic;

public class ReturnTileUsecase
{
    public void Invoke(char removedChar, List<Tile> allowedTiles)
    {
        Tile foundTile = allowedTiles.FindLast(t => t.Value == removedChar && !t.pickable);
        if (foundTile != null)
        {
            foundTile.pickable = true;
        }
    }
}
