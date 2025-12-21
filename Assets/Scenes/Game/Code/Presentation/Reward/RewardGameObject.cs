using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class RewardGameObject : MonoBehaviour
{
    public ApplySpriteComponent applySpriteForMainComponent;
    public AdjustSizeComponent adjustSizeForMainComponent;
    public SelectIndicatorComponent selectIndicatorComponent;
    public ApplyRarityColorComponent applyRarityComponent;

    public Action<Reward> rewardSelectedAction;
    public Action<Reward> rewardHoverAction;

    public Sprite rewardContainerBaseSprite;
    public Sprite rewardContainerRarityElementSprite;

    private Reward reward;
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
        ApplyAction(rewardSelectedAction);
    }

    public void OnHover()
    {
        ApplyAction(rewardHoverAction);
    }
    public void UpdateState(Reward rewardThatIsTargeted)
    {
        if (reward == null)
        {
            return;
        }

        selectIndicatorComponent.Apply(rewardThatIsTargeted == reward);
    }

    public void Init(
        Reward reward,
        Sprite mainSprite
        )
    {
        this.reward = reward;
        applyRarityComponent.Apply(reward.GetFutureValue().RarityEnum);
        applySpriteForMainComponent.Apply(mainSprite);
        adjustSizeForMainComponent.Apply(mainSprite, GetComponent<RectTransform>());
    }

    private void ApplyAction(Action<Reward> action)
    {
        if (reward != null)
        {
            action(reward);
        }
    }

}
