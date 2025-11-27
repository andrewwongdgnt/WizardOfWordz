using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldGameObject : MonoBehaviour
{

    public Button button;
    public TextMeshProUGUI text;
    public WorldEnum worldEnum;

    public Action<WorldEnum> action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = worldEnum.ToString(); 
        button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick()
    {
        button.interactable = false;
        action.Invoke(worldEnum);
    }
}
