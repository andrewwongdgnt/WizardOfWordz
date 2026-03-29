using System;
using UnityEngine;

public class LevelGameObject : MonoBehaviour
{
    public ApplySpriteComponent applySpriteForBaseComponent;
    public ApplySpriteComponent applySpriteForRarityComponent;
    public AdjustSizeComponent adjustSizeForBaseComponent;
    public AdjustSizeComponent adjustmentSizeForRarityComponent;
    public SelectIndicatorComponent selectIndicatorComponent;
    public ApplyRarityColorComponent applyRarityColorComponent;

    public Action<Level> levelSelectedAction;
    public Action<Level> levelHoverAction;

    private Level level;


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
        adjustSizeForBaseComponent.Apply(baseSprite, GetComponent<RectTransform>());
        if (rarityElementSprite != null)
        {
            applySpriteForRarityComponent.Apply(rarityElementSprite);
            adjustmentSizeForRarityComponent.Apply(rarityElementSprite, GetComponent<RectTransform>());
        }
    }
    private void ApplyRarity(Level level)
    {
        if (level is Level.Fight fightLevel)
        {
            applyRarityColorComponent.Apply(fightLevel.RarityEnum);
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
