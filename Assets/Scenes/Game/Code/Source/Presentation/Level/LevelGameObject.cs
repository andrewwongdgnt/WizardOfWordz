using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelGameObject : MonoBehaviour
{
    public ApplySpriteComponent applySpriteForBaseComponent;
    public ApplySpriteComponent applySpriteForRarityComponent;
    public AdjustSizeComponent adjustSizeForBaseComponent;
    public AdjustSizeComponent adjustmentSizeForRarityComponent;
    public SelectIndicatorComponent selectIndicatorComponent;
    public ApplyRarityColorComponent applyRarityComponent;

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

        selectIndicatorComponent.Apply(levelThatIsTargeted == level);
    }

    public void Init(
        Level level,
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        this.level = level;
        ApplyRarity(level); 
        applySpriteForBaseComponent.Apply(baseSprite);
        applySpriteForRarityComponent.Apply(rarityElementSprite);
        adjustSizeForBaseComponent.Apply(baseSprite, GetComponent<RectTransform>());
        adjustmentSizeForRarityComponent.Apply(rarityElementSprite, GetComponent<RectTransform>());
    }
    private void ApplyRarity(Level level)
    {
        if (level is Level.Fight fightLevel)
        {
            applyRarityComponent.Apply(fightLevel.RarityEnum);
        }
    }

    private void ApplyAction(Action<Level> action)
    {
        if (level != null)
        {
            action(level);
        }
    }

}
