using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardManagerTest
{
    private RewardManager sut;
    private IPlayerManager mockPlayerManager;
    private IGenerateRandomNumberUsecase mockGenerateRandomNumberUsecase;

    public static IEnumerable<TestCaseData> InvokeTestCasesForDifferentRewardStates
    {
        get
        {
            yield return new TestCaseData(
                RewardEnum.UpgradeLNRST,
                new List<int>() {1, 2},
                (1, 3)
            ).SetName("UpgradeLNRST with values of 1, 2");

            yield return new TestCaseData(
                RewardEnum.UpgradeBCDGMP,
                new List<int>() { 1, 2 },
                (1, 3)
            ).SetName("UpgradeBCDGMP with values of 1, 2");

            yield return new TestCaseData(
                RewardEnum.UpgradeFKHVWY,
                new List<int>() { 1, 2 },
                (1, 3)
            ).SetName("UpgradeFKHVWY with values of 1, 2");

            yield return new TestCaseData(
                RewardEnum.UpgradeJXQZ,
                new List<int>() { 1, 2 },
                (1, 3)
            ).SetName("UpgradeJXQZ with values of 1, 2");

            yield return new TestCaseData(
                RewardEnum.UpgradeAEIOU,
                new List<int>() { 1, 2 },
                (1, 3)
            ).SetName("UpgradeAEIOU with values of 1, 2");

            yield return new TestCaseData(
                RewardEnum.MaxHealth,
                new List<int>() { 1, 2 },
                (1, 3)
            ).SetName("MaxHealth with values of 1, 2");

            yield return new TestCaseData(
                RewardEnum.MaxTile,
                new List<int>() { 1, 2 },
                (1, 3)
            ).SetName("MaxTile with values of 1, 2");

        }
    }

    [SetUp]
    public void SetUp()
    {
        mockPlayerManager = Substitute.For<IPlayerManager>();
        mockPlayerManager.MaxHealth.Returns(1);
        mockPlayerManager.TileCount.Returns(1);
        mockGenerateRandomNumberUsecase = Substitute.For<IGenerateRandomNumberUsecase>();
        IRewardInfoRepository mockRewardInfoRepository = Substitute.For<IRewardInfoRepository>();
        mockRewardInfoRepository.Get().Returns(MockRewardUtil.GenerateRewordInfo());
        sut = new(
            playerManager: mockPlayerManager,
            generateRandomNumberUsecase: mockGenerateRandomNumberUsecase,
            rewardInfoRepository: mockRewardInfoRepository
            );
    }

    [Test]
    public void TestPresent()
    {
        mockGenerateRandomNumberUsecase.Invoke().Returns(1);
        List<Reward> result = sut.Present();
        Assert.AreEqual(RewardManager.MAX_REWARD_PRESENTABLE, result.Count);
    }

    [Test]
    public void TestPick()
    {
        mockGenerateRandomNumberUsecase.Invoke().Returns(1);
        List<Reward> rewards = sut.Present();
        sut.Pick(rewards[0]);
        Reward.RewardValue rv = sut.GetCurrentValue(rewards[0].RewardEnum);
        Assert.AreEqual(1, rv.Value);
        Assert.AreEqual(RarityEnum.Common, rv.RarityEnum);
        sut.Pick(rewards[0]);
        Reward.RewardValue rv2 = sut.GetCurrentValue(rewards[0].RewardEnum);
        Assert.AreEqual(2, rv2.Value);
        Assert.AreEqual(RarityEnum.Uncommon, rv2.RarityEnum);
    }

    [Test]
    public void TestPickUntilNoLongerPickable()
    {
        mockGenerateRandomNumberUsecase.Invoke().Returns(1);
        List<Reward> rewards = sut.Present();
        Assert.AreEqual("UpgradeLNRST", rewards[0].Title);
        sut.Pick(rewards[0]);
        sut.Pick(rewards[0]);
        List<Reward> rewards2 = sut.Present();
        Assert.AreEqual("UpgradeBCDGMP", rewards2[0].Title);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCasesForDifferentRewardStates))]
    public void TestDifferentRewardStates(
        RewardEnum rewardEnum,
        List<int> rewardValues,
        (int, int) expected
        )
    {
        Reward reward = MockRewardUtil.GenerateReward(
            rewardEnum: rewardEnum,
            values: rewardValues.Select(r =>
                    new Reward.RewardValue(r, RarityEnum.Common)
                ).ToList()
            );
        sut.Pick(reward);
        (int, int) result = sut.GetCurrentAndFutureState(reward);
        Assert.AreEqual(expected, result);
    }
}