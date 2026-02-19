using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

public class PlayerManagerTest
{
    private PlayerManager sut;

    public static IEnumerable<TestCaseData> InvokeTestCasesForUpdateHealthBy
    {
        get
        {
            yield return new TestCaseData(
                -10,
                MockPlayerUtil.DEFAULT_HEALTH-10
            ).SetName("Lose 10 health");

            yield return new TestCaseData(
                10,
                MockPlayerUtil.DEFAULT_HEALTH
            ).SetName("Gain 10 health while at max");

            yield return new TestCaseData(
                -1000,
                0
            ).SetName("Lose 1000 health while at 100");
        }
    }

    public static IEnumerable<TestCaseData> InvokeTestCasesForIsDead
    {
        get
        {
            yield return new TestCaseData(
                -MockPlayerUtil.DEFAULT_HEALTH,
                true
            ).SetName("Lose all health");

            yield return new TestCaseData(
                -10,
                false
            ).SetName("Lose 10 health");

            yield return new TestCaseData(
                0,
                false
            ).SetName("Lose 0 health");
        }
    }

    public static IEnumerable<TestCaseData> InvokeTestCasesForHandleRewardMaxHealth
    {
        get
        {
            yield return new TestCaseData(
                -10,
                10,
                MockPlayerUtil.DEFAULT_HEALTH,
                MockPlayerUtil.DEFAULT_HEALTH + 10
            ).SetName("Lose 10 health then get rewarded +10 max health");

            yield return new TestCaseData(
                0,
                10,
                MockPlayerUtil.DEFAULT_HEALTH + 10,
                MockPlayerUtil.DEFAULT_HEALTH + 10
            ).SetName("Lose 0 health then get rewarded +10 max health");

            yield return new TestCaseData(
                -20,
                50,
                MockPlayerUtil.DEFAULT_HEALTH + 30,
                MockPlayerUtil.DEFAULT_HEALTH + 50
            ).SetName("Lose 20 health then get rewarded +50 max health");
        }
    }

    public static IEnumerable<TestCaseData> InvokeTestCasesForHandleRewardMaxTile
    {
        get
        {
            yield return new TestCaseData(
                1,
                MockPlayerUtil.DEFAULT_TILE_COUNT + 1
            ).SetName("Get rewarded +1 max tile");

            yield return new TestCaseData(
                2,
                MockPlayerUtil.DEFAULT_TILE_COUNT + 2
            ).SetName("Get rewarded +2 max tile");
        }
    }

    [SetUp]
    public void SetUp()
    {
        PlayerInfo playerInfo = MockPlayerUtil.GeneratePlayerInfo();
        IPlayerInfoRepository mockPlayerInfoRepository = Substitute.For<IPlayerInfoRepository>();
        mockPlayerInfoRepository.Get().Returns(playerInfo);
        sut = new(
            playerInfoRepository: mockPlayerInfoRepository
            );
        sut.Init();
    }

    [Test]
    public void TestInit()
    {
        Assert.AreEqual(MockPlayerUtil.DEFAULT_HEALTH, sut.MaxHealth);
        Assert.AreEqual(MockPlayerUtil.DEFAULT_HEALTH, sut.CurrentHealth);
        Assert.AreEqual(MockPlayerUtil.DEFAULT_TILE_COUNT, sut.TileCount);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCasesForUpdateHealthBy))]
    public void TestUpdateHealthBy(
       int healthChange,
       int expectedCurrentHealth
        )
    {
        sut.UpdateHealthBy(healthChange);
        Assert.AreEqual(expectedCurrentHealth, sut.CurrentHealth);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCasesForIsDead))]
    public void TestIsDead(
       int healthChange,
       bool expected
        )
    {
        sut.UpdateHealthBy(healthChange);
        Assert.AreEqual(expected, sut.IsDead());
    }

    [Test]
    public void TestFullHealth()
    {
        sut.UpdateHealthBy(-10);
        Assert.IsFalse(sut.CurrentHealth == sut.MaxHealth);
        sut.FullHeath();
        Assert.IsTrue(sut.CurrentHealth == sut.MaxHealth);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCasesForHandleRewardMaxHealth))]
    public void TestHandleRewardMaxHealth(
       int healthChange,
       int maxHealthRewardChange,
       int expectedCurrentHealth,
       int expectedMaxHealth
        )
    {
        sut.UpdateHealthBy(healthChange);
        Reward reward = MockRewardUtil.GenerateReward(rewardEnum: RewardEnum.MaxHealth, values: new List<Reward.RewardValue>()
                {
                    MockRewardUtil.GenerateRewardValue(value: maxHealthRewardChange)
                });
        reward.Pick();
        sut.HandleReward(reward);
        Assert.AreEqual(expectedCurrentHealth, sut.CurrentHealth);
        Assert.AreEqual(expectedMaxHealth, sut.MaxHealth);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCasesForHandleRewardMaxTile))]
    public void TestHandleRewardMaxTile(
       int maxTileRewardChange,
       int expectedCurrentTileCount
        )
    {
        Reward reward = MockRewardUtil.GenerateReward(rewardEnum: RewardEnum.MaxTile, values: new List<Reward.RewardValue>()
                {
                    MockRewardUtil.GenerateRewardValue(value: maxTileRewardChange)
                });
        reward.Pick();
        sut.HandleReward(reward);
        Assert.AreEqual(expectedCurrentTileCount, sut.TileCount);
    }
}