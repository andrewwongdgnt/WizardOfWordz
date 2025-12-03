using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class RewardManager
{
    private List<Reward> availableRewards;
    private Random random = new();
    [Inject]
    public RewardManager(
    RewardInfoRepository rewardInfoRepository
    )
    {
        Init(rewardInfoRepository.Get());
    }

    public List<Reward> Present()
    {
        int count = Math.Min(3, availableRewards.Count);
        return availableRewards.OrderBy(r => random.Next()).Take(count).ToList();
    }

    public void Pick(Reward reward)
    {
        reward.Pick();

    }

    private void Init(RewardInfo rewardInfo)
    {
        availableRewards = new() {
             InitReward(rewardInfo.Reroll),
             InitReward(rewardInfo.MaxHealth),
             InitReward(rewardInfo.MaxTile)
            };
    }

    private Reward InitReward(RewardInfo.DetailInfo rewardInfoDetail)
    {
        return new(
            (RarityEnum)Enum.Parse(typeof(RarityEnum), rewardInfoDetail.rarity),
            rewardInfoDetail.title,
            rewardInfoDetail.description,
            rewardInfoDetail.values
          );
    }

}