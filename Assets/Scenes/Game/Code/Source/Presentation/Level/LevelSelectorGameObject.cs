using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorGameObject : MonoBehaviour
{

    public Animator animator;

    public GameObject levelContainer;
    public LevelGameObject levelGO;

    public Sprite fightLevelSprite;
    public Sprite restLevelSprite;

    public Action<Level> levelSelectedAction;
    public Action<Level> levelHoverAction;

    private readonly Dictionary<Level, LevelGameObject> levelMap = new();

    public void UpdateState(Level levelThatIsTargeted)
    {
        foreach (var levelGo in levelMap.Values)
        {
            levelGo.UpdateState(levelThatIsTargeted);
        }
    }
    public void SetUp(List<Level> levels)
    {
        ClearLevels();

        levels.ForEach(level =>
        {
            (Sprite baseSprite, Sprite raritysprite) spritePair;

            spritePair = level switch
            {
                Level.Fight => (fightLevelSprite, fightLevelSprite),
                Level.Rest => (restLevelSprite, restLevelSprite),
                _ => throw new NotImplementedException(),
            };

            LevelGameObject newLevelGO = Instantiate(levelGO, levelContainer.transform.position, Quaternion.identity);
            newLevelGO.transform.SetParent(levelContainer.transform);
            newLevelGO.Init(level, spritePair.baseSprite, spritePair.raritysprite);
            levelMap[level] = newLevelGO;
            newLevelGO.levelSelectedAction = levelSelectedAction;
            newLevelGO.levelHoverAction = levelHoverAction;

            AdjustPosition(newLevelGO.GetComponent<RectTransform>());
        }
        );
    }

    private void ClearLevels()
    {
        foreach (var levelItem in levelMap)
        {
            Destroy(levelItem.Value.gameObject);
        }
        levelMap.Clear();
    }

    private void AdjustPosition(RectTransform rect)
    {
        rect.localPosition = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector2.zero;
    }
    public void Appear(bool appear)
    {
        animator.SetBool("Appear", appear);
    }
}
