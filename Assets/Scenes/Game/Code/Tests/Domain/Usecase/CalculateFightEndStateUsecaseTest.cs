using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static Enemy;

public class CalculateFightEndStateUsecaseTest
{
    private CalculateFightEndStateUsecase sut;
    private IPlayerManager mockPlayerManager;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<bool>
                {
                    true,
                    true
                },
                false,
                FightEndStateEnum.Ongoing
            ).SetName("2 alive enemies, player is alive");

            yield return new TestCaseData(
                new List<bool>
                {
                    false,
                    true
                },
                false,
                FightEndStateEnum.Ongoing
            ).SetName("1 dead and 1 alive enemy, player is alive");

            yield return new TestCaseData(
                new List<bool>
                {
                    false,
                    false
                },
                false,
                FightEndStateEnum.Win
            ).SetName("2 dead enemies, player is alive");

            yield return new TestCaseData(
                new List<bool>
                {
                    true,
                    true
                },
                true,
                FightEndStateEnum.Lose
            ).SetName("2 alive enemies, player is dead");

            yield return new TestCaseData(
                new List<bool>
                {
                    false,
                    true
                },
                true,
                FightEndStateEnum.Lose
            ).SetName("1 dead and 1 alive enemy, player is dead");

            yield return new TestCaseData(
                new List<bool>
                {
                    false,
                    false
                },
                true,
                FightEndStateEnum.Lose
            ).SetName("2 dead enemies, player is dead");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockPlayerManager = Substitute.For<IPlayerManager>();
        sut = new(playerManager: mockPlayerManager);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<bool> aliveState,
        bool isPlayerDead,
        FightEndStateEnum expected
        )
    {
        List<Enemy> enemies = aliveState.Select(v =>
        {
            return v ? MockEnemyUtil.GenerateEnemy() : MockEnemyUtil.GenerateEnemy(startingHealth: 0);
        }).ToList();

        mockPlayerManager.IsDead().Returns(isPlayerDead);

        FightEndStateEnum result = sut.Invoke(
            enemies: enemies
            );

        Assert.AreEqual(expected, result);
    }
}
