using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class RewardManager
{
    private readonly PlayerManager playerManager;

    private Dictionary<RewardEnum, Reward> availableRewards;
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
        return availableRewards.Values.OrderBy(r => random.Next()).Take(count).ToList();
    }

    public void Pick(Reward reward)
    {
        reward.Pick();

    }
    public int GetCurrentValue(Reward reward)
    {
        return reward.GetCurrentValue();
    }

    public (int, int) GetCurrentAndFutureState(Reward reward)
    {
        return reward.RewardEnum switch
        {
            RewardEnum.Reroll => (0, 0),
            RewardEnum.MaxHealth => GetMaxHealthState(reward),
            RewardEnum.MaxTile => GetMaxTileState(reward),
            _ => throw new NotImplementedException(),
        };
    }

    private void Init(RewardInfo rewardInfo)
    {
        availableRewards = new() {
            { RewardEnum.Reroll, InitReward(rewardInfo.Reroll, RewardEnum.Reroll) },
            { RewardEnum.MaxHealth, InitReward(rewardInfo.MaxHealth, RewardEnum.MaxHealth) },
            { RewardEnum.MaxTile, InitReward(rewardInfo.MaxTile, RewardEnum.MaxTile) }
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

    private (int, int) GetMaxHealthState(Reward reward)
    {
        int current = playerManager.MaxHealth;
        int future = (playerManager.MaxHealth + reward.GetFutureValue());
        return (current, future);
    }

    private (int, int) GetMaxTileState(Reward reward)
    {
        int current = playerManager.TileCount;
        int future = (playerManager.TileCount + reward.GetFutureValue());
        return (current, future);
    }
}