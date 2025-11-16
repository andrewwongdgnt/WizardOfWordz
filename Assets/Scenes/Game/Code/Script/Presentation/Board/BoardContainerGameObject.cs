using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BoardContainerGameObject : MonoBehaviour
{
    public TileGameObject tileGO;

    public GameObject tileContainer;

    public GameObject wordContainer;

    public Action<Tile> tileAction;

    public Action<Tile> tileInWordAction;

    private readonly Dictionary<Tile, TileGameObject> tileMap = new();

    private readonly Dictionary<Tile, TileGameObject> tileInWordMap = new();

    private readonly Dictionary<Tile, Tile> wordTileToTile = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateState(
        List<Tile> currentWordList,
        Tile tileThatChanged
        )
    {
        wordTileToTile.Clear();
        List<Tile> newCurrentWordList = new();
        currentWordList.ForEach(t =>
        {
            Tile clonedTile = t.Clone();
            wordTileToTile[clonedTile] = t;
            newCurrentWordList.Add(clonedTile);
        });

        SetUp(newCurrentWordList, tileInWordMap, wordContainer, TileInWordAction, 0.6f);

        if (tileThatChanged == null)
        {
            return;
        }
        tileMap.TryGetValue(tileThatChanged, out TileGameObject foundTileGO);
        if (foundTileGO != null)
        {
            foundTileGO.UpdateState();
        }

    }

    public void SetUpTiles(List<Tile> tiles)
    {
        SetUp(tiles, tileMap, tileContainer, tileAction);
    }

    private void SetUp(
        List<Tile> tiles,
        Dictionary<Tile, TileGameObject> tileMap,
        GameObject container,
        Action<Tile> action,
        float scale = 1f
        )
    {
        foreach (var tileItem in tileMap)
        {
            Destroy(tileItem.Value.gameObject);
        }
        tileMap.Clear();

        tiles.ForEach(tile =>
        {
            TileGameObject newTileGO = Instantiate(tileGO, container.transform.position, Quaternion.identity);
            newTileGO.transform.SetParent(container.transform);
            newTileGO.SetTile(tile);
            tileMap[tile] = newTileGO;
            newTileGO.tileAction = action;

            AdjustPosition(newTileGO.GetComponent<RectTransform>(), scale);
        }
        );
    }

    private void AdjustPosition(RectTransform rect, float scaling = 1f)
    {
        rect.localPosition = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = new Vector3(scaling, scaling, scaling);
        rect.anchoredPosition = Vector2.zero;
    }

    private void TileInWordAction(Tile wordTile)
    {
        wordTileToTile.TryGetValue(wordTile, out Tile originalTile);
        if (originalTile != null)
        {
            tileInWordAction(originalTile);
        }
    }
}
