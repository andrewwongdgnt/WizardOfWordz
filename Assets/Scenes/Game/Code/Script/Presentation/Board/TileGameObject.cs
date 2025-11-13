using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileGameObject : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI letter;
    public TextMeshProUGUI score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLetterValue(char value)
    {
        letter.text = value.ToString();
    }

    public void SetScore(int value)
    {
        score.text = value.ToString();
    }

    public void SetPickable(bool value)
    {
        button.interactable = value;
    }
}
