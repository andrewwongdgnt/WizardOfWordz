using System;
using static Reward;

public class MockRewardUtil
{   public static RewardValue GenerateRewardValue(
            int value = 1,
            RarityEnum rarityEnum = RarityEnum.Common,
            Action<RewardValue>? action = null
        )
    {
        RewardValue rewardValue = new(
                value: value,
                rarityEnum: rarityEnum
            );
        action?.Invoke(rewardValue);
        return rewardValue;
    }
}