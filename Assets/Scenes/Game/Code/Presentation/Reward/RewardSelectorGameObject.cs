using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardSelectorGameObject : MonoBehaviour
{

    public Animator animator;

    public GameObject rewardContainer;
    public RewardGameObject rewardGO;

    public Sprite upgradeLNRSTSprite;
    public Sprite upgradeBCDGMPSprite;
    public Sprite upgradeFKHVWYSprite;
    public Sprite upgradeJXQZSprite;
    public Sprite upgradeAEIOUSprite;
    public Sprite maxHealthSprite;
    public Sprite maxTileSprite;

    public Action<Reward> rewardSelectedAction;
    public Action<Reward> rewardHoverAction;

    private readonly Dictionary<Reward, RewardGameObject> rewardMap = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateState(Reward rewardThatIsTargeted)
    {
        foreach (var rewardGo in rewardMap.Values)
        {
            rewardGo.UpdateState(rewardThatIsTargeted);
        }
    }
    public void SetUp(List<Reward> rewards)
    {
        ClearRewards();

        rewards.ForEach(reward =>
        {

            Sprite sprite = reward.RewardEnum switch
            {
                RewardEnum.UpgradeLNRST => upgradeLNRSTSprite,
                RewardEnum.UpgradeBCDGMP => upgradeBCDGMPSprite,
                RewardEnum.UpgradeFKHVWY => upgradeFKHVWYSprite,
                RewardEnum.UpgradeJXQZ => upgradeJXQZSprite,
                RewardEnum.UpgradeAEIOU => upgradeAEIOUSprite,
                RewardEnum.MaxHealth => maxHealthSprite,
                RewardEnum.MaxTile => maxTileSprite,
                _ => throw new NotImplementedException(),
            };


            RewardGameObject newRewardGO = Instantiate(rewardGO, rewardContainer.transform.position, Quaternion.identity);
            newRewardGO.transform.SetParent(rewardContainer.transform);
            newRewardGO.Init(reward, sprite);
            rewardMap[reward] = newRewardGO;
            newRewardGO.rewardSelectedAction = rewardSelectedAction;
            newRewardGO.rewardHoverAction = rewardHoverAction;

            AdjustPosition(newRewardGO.GetComponent<RectTransform>());
        }
        );
    }

    private void ClearRewards()
    {
        foreach (var rewardItem in rewardMap)
        {
            Destroy(rewardItem.Value.gameObject);
        }
        rewardMap.Clear();
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
