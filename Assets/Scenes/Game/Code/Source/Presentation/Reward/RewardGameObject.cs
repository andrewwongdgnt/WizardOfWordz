using System;
using TMPro;
using UnityEngine;

public class RewardGameObject : MonoBehaviour
{
    public ApplySpriteComponent applySpriteForMainComponent;
    public AdjustSizeComponent adjustSizeForMainComponent;
    public SelectIndicatorComponent selectIndicatorComponent;
    public ApplyRarityColorComponent applyRarityComponent;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI currentStateText;
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI futureStateText;

    public Action<Reward> rewardSelectedAction;
    public Action<Reward> rewardHoverAction;

    private Reward reward;

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
        IRewardManager rewardManager,
        Reward reward,
        Sprite mainSprite
        )
    {
        this.reward = reward;
        applyRarityComponent.Apply(reward.GetFutureValue().RarityEnum);
        applySpriteForMainComponent.Apply(mainSprite);
        adjustSizeForMainComponent.Apply(mainSprite, GetComponent<RectTransform>());
        InitText(rewardManager, reward);
    }

    private void InitText(
        IRewardManager rewardManager,
        Reward reward
        )
    {
        titleText.text = reward.Title;
        (int, int) pair = rewardManager.GetCurrentAndFutureState(reward);

        switch (reward.RewardEnum)
        {
            case RewardEnum.UpgradeLNRST:
            case RewardEnum.UpgradeBCDGMP:
            case RewardEnum.UpgradeFKHVWY:
            case RewardEnum.UpgradeJXQZ:
            case RewardEnum.UpgradeAEIOU:
                currentStateText.text = "";
                stateText.text = $"+{(reward.GetFutureValue().Value - reward.GetCurrentValue().Value)}";
                futureStateText.text = "";
                break;
            case RewardEnum.MaxHealth:
            case RewardEnum.MaxTile:
                currentStateText.text = pair.Item1.ToString();
                stateText.text = $"+{reward.GetFutureValue().Value}";
                futureStateText.text = pair.Item2.ToString();
                break;
        }

    }

    private void ApplyAction(Action<Reward> action)
    {
        if (reward != null)
        {
            action(reward);
        }
    }
}
