using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BoardContainerGameObject : MonoBehaviour
{

    public TileGameObject tileGO;

    public GameObject tileContainer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUpTiles(List<Tile> tiles)
    {
        tiles.ForEach(t =>
            {
                TileGameObject newTileGO = Instantiate(tileGO, tileContainer.transform.position, Quaternion.identity);
                newTileGO.transform.SetParent(tileContainer.transform);
                newTileGO.SetLetterValue(t.Value);
                newTileGO.SetScore(t.Score);

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
