using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileGameObject : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI letter;
    public TextMeshProUGUI score;

    public Action<Tile> tileAction;

    private Tile tile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
        UpdateState();
    }

    public void UpdateState()
    {
        if (tile == null)
        {
            return;
        }
        letter.text = tile.Value.ToString();
        score.text = tile.Score.ToString();
        button.interactable = tile.pickable;
    }

    private void OnClick()
    {
        if (tileAction != null && tile != null)
        {
            tileAction.Invoke(tile);
        }
    }
}
