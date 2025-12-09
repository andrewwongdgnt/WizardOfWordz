using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelGameObject : MonoBehaviour
{
    public Image baseImage;
    public RarityComponent rarityComponent;
    public TextMeshProUGUI selectIndicatorText;

    public Action<Level> levelSelectedAction;
    public Action<Level> levelHoverAction;

    private Level level;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect()
    {
        ApplyAction(levelSelectedAction);
    }

    public void OnHover()
    {
        ApplyAction(levelHoverAction);
    }
    public void UpdateState(Level levelThatIsTargeted)
    {
        if (level == null)
        {
            return;
        }

        UpdateSelectIndicator(levelThatIsTargeted == level);
    }
    public void Init(
        Level level,
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        this.level = level;
        ApplyRarity(level);
        ApplySprite(baseSprite, rarityElementSprite);
        ApplySize(baseSprite, rarityElementSprite);
    }
    private void ApplyRarity(Level level)
    {
        if (level is Level.Fight fightLevel)
        {
            rarityComponent.Apply(fightLevel.RarityEnum);
        }
    }

    private void ApplySprite(
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        baseImage.sprite = baseSprite;
        rarityComponent.image.sprite = rarityElementSprite;
    }

    private void ApplySize(
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        var vector = new Vector2(baseSprite.rect.width, baseSprite.rect.height);
        GetComponent<RectTransform>().sizeDelta = vector;
        baseImage.rectTransform.sizeDelta = vector;
        rarityComponent.image.rectTransform.sizeDelta = new Vector2(rarityElementSprite.rect.width, rarityElementSprite.rect.height);
    }

    private void ApplyAction(Action<Level> action)
    {
        if (level != null)
        {
            action(level);
        }
    }
    private void UpdateSelectIndicator(bool isSelected)
    {
        selectIndicatorText.text = isSelected ? "V" : "";
    }
}
