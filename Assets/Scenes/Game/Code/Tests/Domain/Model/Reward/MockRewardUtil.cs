using System;
using System.Collections.Generic;
using static Reward;

public class MockRewardUtil
{   public static RewardValue GenerateRewardValue(
            int value = 1,
            RarityEnum rarityEnum = RarityEnum.Common
        )
    {
        return new(
                value: value,
                rarityEnum: rarityEnum
            );
    }

    public static Reward GenerateReward(
        RewardEnum rewardEnum = RewardEnum.MaxHealth,
        string title = "",
        string description = "",
        List<RewardValue>? values = null
        )
    {
        return new(
            rewardEnum: rewardEnum,
            title: title,
            description: description,
            values: values ?? new List<RewardValue> { GenerateRewardValue() }
            );
    }

    public static RewardInfo GenerateRewordInfo()
    {
        return new()
        {
            UpgradeLNRST = GenerateDetailInfo("UpgradeLNRST"),
            UpgradeBCDGMP = GenerateDetailInfo("UpgradeBCDGMP"),
            UpgradeFKHVWY = GenerateDetailInfo("UpgradeFKHVWY"),
            UpgradeJXQZ = GenerateDetailInfo("UpgradeJXQZ"),
            UpgradeAEIOU = GenerateDetailInfo("UpgradeAEIOU"),
            MaxHealth = GenerateDetailInfo("MaxHealth"),
            MaxTile = GenerateDetailInfo("MaxTile"),
        };
    }

    private static RewardInfo.DetailInfo GenerateDetailInfo(string title)
    {
        return new()
        {
            title = title,
            description = title + " desc",
            values = new List<RewardInfo.DetailInfo.ValueInfo>()
            {
                new() { value = 1, rarity = RarityEnum.Common.ToString() },
                new() { value = 2, rarity = RarityEnum.Uncommon.ToString() }
            }
        };
    }
}