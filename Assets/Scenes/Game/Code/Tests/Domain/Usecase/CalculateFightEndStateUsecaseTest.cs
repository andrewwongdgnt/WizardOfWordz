using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Enemy;

public class CalculateFightEndStateUsecaseTest
{
    private CalculateFightEndStateUsecase sut;
    private PlayerManager mockPlayerManager;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(false);
                    }),  
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(false);
                    })
                },
                false,
                FightEndStateEnum.Ongoing
            ).SetName("2 alive enemies, player is alive");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(false);
                    }),
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(true);
                    })
                },
                false,
                FightEndStateEnum.Ongoing
            ).SetName("1 dead and 1 alive enemy, player is alive");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(true);
                    }),
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(true);
                    })
                },
                false,
                FightEndStateEnum.Win
            ).SetName("2 dead enemies, player is alive");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(false);
                    }),
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(false);
                    })
                },
                true,
                FightEndStateEnum.Lose
            ).SetName("2 alive enemies, player is dead");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(false);
                    }),
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(true);
                    })
                },
                true,
                FightEndStateEnum.Lose
            ).SetName("1 dead and 1 alive enemy, player is dead");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(true);
                    }),
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(true);
                    })
                },
                true,
                FightEndStateEnum.Lose
            ).SetName("2 dead enemies, player is dead");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockPlayerManager = PlayerManagerTest.GenerateMock();
        sut = new(playerManager: mockPlayerManager);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<Enemy> mockEnemies,
        bool isPlayerDead,
        FightEndStateEnum expected
        )
    {
        mockPlayerManager.IsDead().Returns(isPlayerDead);

        FightEndStateEnum result = sut.Invoke(
            enemies: mockEnemies
            );

        Assert.AreEqual(expected, result);

        TestUtils.ClearReceivedCalls(mockEnemies);
    }

    public static CalculateFightEndStateUsecase GenerateMock()
    {
        return GenerateMock(_ => { });
    }

    public static CalculateFightEndStateUsecase GenerateMock(Action<CalculateFightEndStateUsecase> action)
    {
        CalculateFightEndStateUsecase mock = Substitute.For<CalculateFightEndStateUsecase>(
             PlayerManagerTest.GenerateMock()
             );
        action(mock);
        return mock;
    }
}
