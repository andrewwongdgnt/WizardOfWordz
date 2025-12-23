using UnityEngine;
using UnityEngine.UI;

public class AdjustSizeComponent : MonoBehaviour
{
    public Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Apply(
        Sprite sprite,
        RectTransform rectTransform
        )
    {
        var vector = new Vector2(sprite.rect.width, sprite.rect.height);
        //rectTransform.sizeDelta = vector;
        image.rectTransform.sizeDelta = vector;
    }
}
