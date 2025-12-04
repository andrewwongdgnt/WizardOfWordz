using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class RewardManager
{
    private readonly PlayerManager playerManager;

    private List<Reward> availableRewards;
    private Random random = new();
    [Inject]
    public RewardManager(
    PlayerManager playerManager,
    RewardInfoRepository rewardInfoRepository
    )
    {
        this.playerManager = playerManager;
        Init(rewardInfoRepository.Get());
    }

    public List<Reward> Present()
    {
        int count = Math.Min(3, availableRewards.Count);
        return availableRewards.OrderBy(r => random.Next()).Take(count).ToList();
    }

    public void Pick(Reward reward)
    {
        int value = reward.GetFutureValue();
        reward.Pick();

    }

    public (String, String) GetCurrentAndFutureStatePair(Reward reward)
    {
        return reward.RewardEnum switch
        {
            RewardEnum.Reroll => ("", ""),
            RewardEnum.MaxHealth => GetMaxHealthState(reward),
            RewardEnum.MaxTile => GetMaxTileState(reward),
            _ => throw new NotImplementedException(),
        };
    } 

    private void Init(RewardInfo rewardInfo)
    {
        availableRewards = new() {
             InitReward(rewardInfo.Reroll, RewardEnum.Reroll),
             InitReward(rewardInfo.MaxHealth, RewardEnum.MaxHealth),
             InitReward(rewardInfo.MaxTile, RewardEnum.MaxTile)
            };
    }

    private Reward InitReward(RewardInfo.DetailInfo rewardInfoDetail, RewardEnum rewardEnum)
    {
        return new(
            rewardEnum,
            (RarityEnum)Enum.Parse(typeof(RarityEnum), rewardInfoDetail.rarity),
            rewardInfoDetail.title,
            rewardInfoDetail.description,
            rewardInfoDetail.values
          );
    }

    private (String, String) GetMaxHealthState(Reward reward)
    {
        String current = playerManager.MaxHealth.ToString();
        String future = (playerManager.MaxHealth + reward.GetFutureValue()).ToString();
        return (current, future);
    }

    private (String, String) GetMaxTileState(Reward reward)
    {
        String current = playerManager.TileCount.ToString();
        String future = (playerManager.TileCount + reward.GetFutureValue()).ToString();
        return (current, future);
    }
}