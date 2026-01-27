using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static Enemy;

public class CalculateTurnFromEnemiesUsecaseTest
{
    private CalculateTurnFromEnemiesUsecase sut;
    private IGetNextEnemyMoveUsecase mockGetNextEnemyMoveUsecase;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<(int turnsRemaining, bool alive)>
                {
                    (1, true),
                    (1, true)
                },
                new List<int>
                {
                    0,
                    1
                }
            ).SetName("2 alive enemies on last turn");

            yield return new TestCaseData(
                new List<(int turnsRemaining, bool alive)>
                {
                    (5, true),
                    (1, true)
                },
                new List<int>
                {
                    1
                }
            ).SetName("2 alive enemies with 1 on last turn");

            yield return new TestCaseData(
                new List<(int turnsRemaining, bool alive)>
                {
                    (5, true),
                    (5, true)
                },
                new List<int>
                {
                    
                }
            ).SetName("2 alive enemies with none on last turn");

            yield return new TestCaseData(
                new List<(int turnsRemaining, bool alive)>
                {
                    (1, false),
                    (1, true)
                },
                new List<int>
                {
                    1
                }
            ).SetName("1 dead, 1 alive enemies on last turn");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockGetNextEnemyMoveUsecase = Substitute.For<IGetNextEnemyMoveUsecase>();
        sut = new(getNextEnemyMoveUsecase: mockGetNextEnemyMoveUsecase);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<(int turnsRemaining, bool alive)> enemyParam,
        List<int> enemyIndices
        )
    {
        List<Enemy> enemies = enemyParam.Select(p =>
        {
            return MockEnemyUtil.GenerateEnemy(
                startingHealth: p.alive ? MockEnemyUtil.DEFAULT_STARTING_HEALTH : 0,
                action: e =>
                {
                    Move move = MockEnemyUtil.GenerateMove();
                    e.SetCurrentMove(move);
                    e.TurnsRemaining = p.turnsRemaining;
                }
            );
        }).ToList();
        List<(int enemyIndex, Enemy.Move move)> expected = enemyIndices.Select(i =>
            {
                Enemy.Move newMove = MockEnemyUtil.GenerateMove();
                mockGetNextEnemyMoveUsecase.Invoke(enemies[i]).Returns(newMove);
                return (i, newMove);
            }
        ).ToList();
        List<(int enemyIndex, Enemy.Move move)> result = sut.Invoke(enemies: enemies);

        Assert.AreEqual(expected, result);

        enemyIndices.ForEach(i =>
            {
                mockGetNextEnemyMoveUsecase.Received(1).Invoke(enemies[i]);
            }
        );
    }
}
