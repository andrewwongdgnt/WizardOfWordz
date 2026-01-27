using System.Collections.Generic;
using UnityEngine.InputSystem;

public interface IPickTileUsecase
{

    public Tile Invoke(
        Key key,
        List<Tile> allowedTiles
        );

    public bool Invoke(
        Tile tile,
        List<Tile> allowedTiles
        );
}
