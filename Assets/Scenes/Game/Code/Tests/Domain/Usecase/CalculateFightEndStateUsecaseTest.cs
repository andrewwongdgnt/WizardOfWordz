using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
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
                new List<Enemy>
                {
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(false);
                    }),  
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(false);
                    })
                },
                false,
                FightEndStateEnum.Ongoing
            ).SetName("2 alive enemies, player is alive");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(false);
                    }),
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(true);
                    })
                },
                false,
                FightEndStateEnum.Ongoing
            ).SetName("1 dead and 1 alive enemy, player is alive");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(true);
                    }),
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(true);
                    })
                },
                false,
                FightEndStateEnum.Win
            ).SetName("2 dead enemies, player is alive");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(false);
                    }),
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(false);
                    })
                },
                true,
                FightEndStateEnum.Lose
            ).SetName("2 alive enemies, player is dead");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(false);
                    }),
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(true);
                    })
                },
                true,
                FightEndStateEnum.Lose
            ).SetName("1 dead and 1 alive enemy, player is dead");

            yield return new TestCaseData(
                new List<Enemy>
                {
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(true);
                    }),
                    GenerateMocks.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(true);
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
        mockPlayerManager = Substitute.For<IPlayerManager>();
        sut = new(playerManager: mockPlayerManager);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<Enemy> enemies,
        bool isPlayerDead,
        FightEndStateEnum expected
        )
    {
        mockPlayerManager.IsDead().Returns(isPlayerDead);

        FightEndStateEnum result = sut.Invoke(
            enemies: enemies
            );

        Assert.AreEqual(expected, result);

    }
}
