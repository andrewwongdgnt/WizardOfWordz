using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using static Reward;

public class RewardManager : IRewardManager
{
    public const int MAX_REWARD_PRESENTABLE = 3;

    private readonly IPlayerManager playerManager;
    private readonly IGenerateRandomNumberUsecase generateRandomNumberUsecase;

    private Dictionary<RewardEnum, Reward> allRewards;
    [Inject]
    public RewardManager(
    IPlayerManager playerManager,
    IGenerateRandomNumberUsecase generateRandomNumberUsecase,
    IRewardInfoRepository rewardInfoRepository
    )
    {
        this.playerManager = playerManager;
        this.generateRandomNumberUsecase = generateRandomNumberUsecase;
        Init(rewardInfoRepository.Get());
    }

    public List<Reward> Present()
    {
        List<Reward> availableRewards = allRewards.Values.Where(r => r.Pickable()).ToList();
        int count = Math.Min(MAX_REWARD_PRESENTABLE, availableRewards.Count);
        return availableRewards.OrderBy(r => generateRandomNumberUsecase.Invoke()).Take(count).ToList();
    }

    public void Pick(Reward reward)
    {
        if (reward.Pickable())
        {
            reward.Pick();
        }
    }
    public RewardValue GetCurrentValue(RewardEnum rewardEnum)
    {
        return allRewards[rewardEnum].GetCurrentValue();
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
        allRewards = new() {
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