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
}