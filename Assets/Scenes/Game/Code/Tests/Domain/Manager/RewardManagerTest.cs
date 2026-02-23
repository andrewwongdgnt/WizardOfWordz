using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

public class RewardManagerTest
{
    private RewardManager sut;
    private IPlayerManager mockPlayerManager;
    private IGenerateRandomNumberUsecase mockGenerateRandomNumberUsecase;

    //public static IEnumerable<TestCaseData> InvokeTestCasesForPresent
    //{
    //    get
    //    {
    //        yield return new TestCaseData(
    //            -10,
    //            MockPlayerUtil.DEFAULT_HEALTH-10
    //        ).SetName("Lose 10 health");

    //        yield return new TestCaseData(
    //            10,
    //            MockPlayerUtil.DEFAULT_HEALTH
    //        ).SetName("Gain 10 health while at max");

    //        yield return new TestCaseData(
    //            -1000,
    //            0
    //        ).SetName("Lose 1000 health while at 100");
    //    }
    //}

    [SetUp]
    public void SetUp()
    {
        mockPlayerManager = Substitute.For<IPlayerManager>();
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
    public void TestDifferentRewardStates()
    {
        mockGenerateRandomNumberUsecase.Invoke().Returns(1); 
        List<Reward> rewards = sut.Present();
        sut.Pick(rewards[0]);
        Reward.RewardValue rv = sut.GetCurrentValue(rewards[0].RewardEnum);
        Assert.AreEqual(1, rv.Value);
        Assert.AreEqual(RarityEnum.Common, rv.RarityEnum);


        (int, int) currentAndFutureState = sut.GetCurrentAndFutureState(rewards[0]);
        Assert.AreEqual((1, 3), currentAndFutureState);
    }
}