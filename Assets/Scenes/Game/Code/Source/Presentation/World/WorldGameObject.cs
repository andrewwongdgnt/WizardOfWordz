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

    void Start()
    {
        text.text = worldEnum.ToString(); 
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        button.interactable = false;
        action.Invoke(worldEnum);
    }
}
