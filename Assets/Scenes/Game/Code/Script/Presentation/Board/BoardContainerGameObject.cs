using NUnit.Framework;
using System.Collections.Generic;
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

    public TextMeshProUGUI wordGO;

    private readonly Dictionary<Tile, TileGameObject> tileMap = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wordGO.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateState(
        string word,
        Tile tileThatChanged = null
        )
    {
        wordGO.text = word;
        if (tileThatChanged != null)
        {
            tileMap.TryGetValue(tileThatChanged, out TileGameObject foundTileGO);
            if (foundTileGO != null)
            {
                foundTileGO.SetPickable(tileThatChanged.pickable);
            }
        }
    }

    public void SetUpTiles(List<Tile> tiles)
    {
        foreach (var tileItem in tileMap)
        {
            Destroy(tileItem.Value.gameObject);
        }
        tileMap.Clear();

        tiles.ForEach(t =>
            {
                TileGameObject newTileGO = Instantiate(tileGO, tileContainer.transform.position, Quaternion.identity);
                newTileGO.transform.SetParent(tileContainer.transform);
                newTileGO.SetLetterValue(t.Value);
                newTileGO.SetScore(t.Score);
                tileMap[t] = newTileGO;

                AdjustPosition(newTileGO.GetComponent<RectTransform>());
            }
        );
    }

    private void AdjustPosition(RectTransform rect)
    {
        rect.localPosition = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector2.zero;
    }
}
