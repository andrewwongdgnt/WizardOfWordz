using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using static Reward;

public class RewardManager
{
    private readonly PlayerManager playerManager;

    private Dictionary<RewardEnum, Reward> availableRewards;
    private readonly Random random = new();
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
    public RewardValue GetCurrentValue(RewardEnum rewardEnum)
    {
        return availableRewards[rewardEnum].GetCurrentValue();
    }

    public (int, int) GetCurrentAndFutureState(Reward reward)
    {
        return reward.RewardEnum switch
        {
            RewardEnum.UpgradeLNRST or
            RewardEnum.UpgradeBCDGMP or
            RewardEnum.UpgradeFKHVWY or
            RewardEnum.UpgradeJXQZ or
            RewardEnum.UpgradeAEIOU => GetState(reward, reward.GetCurrentValue().Value),
            RewardEnum.MaxHealth => GetState(reward, playerManager.MaxHealth),
            RewardEnum.MaxTile => GetState(reward, playerManager.TileCount),
            _ => throw new NotImplementedException(),
        };
    }

    private void Init(RewardInfo rewardInfo)
    {
        availableRewards = new() {
            { RewardEnum.UpgradeLNRST, InitReward(rewardInfo.UpgradeLNRST, RewardEnum.UpgradeLNRST) },
            { RewardEnum.UpgradeBCDGMP, InitReward(rewardInfo.UpgradeBCDGMP, RewardEnum.UpgradeBCDGMP) },
            { RewardEnum.UpgradeFKHVWY, InitReward(rewardInfo.UpgradeFKHVWY, RewardEnum.UpgradeFKHVWY) },
            { RewardEnum.UpgradeJXQZ, InitReward(rewardInfo.UpgradeJXQZ, RewardEnum.UpgradeJXQZ) },
            { RewardEnum.UpgradeAEIOU, InitReward(rewardInfo.UpgradeAEIOU, RewardEnum.UpgradeAEIOU) },
            { RewardEnum.MaxHealth, InitReward(rewardInfo.MaxHealth, RewardEnum.MaxHealth) },
            { RewardEnum.MaxTile, InitReward(rewardInfo.MaxTile, RewardEnum.MaxTile) }
            };
    }

    private Reward InitReward(RewardInfo.DetailInfo rewardInfoDetail, RewardEnum rewardEnum)
    {
        return new(
            rewardEnum,
            rewardInfoDetail.title,
            rewardInfoDetail.description,
            rewardInfoDetail.values.Select(v =>
                new RewardValue(v.value, (RarityEnum)Enum.Parse(typeof(RarityEnum), v.rarity)
            )
            ).ToList()
          );
    }

    private (int, int) GetState(Reward reward, int current)
    {
        int future = (current + reward.GetFutureValue().Value);
        return (current, future);
    }
}