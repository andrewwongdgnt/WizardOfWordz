using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static Enemy;

public class CalculateTurnFromEnemiesUsecaseTest
{
    private CalculateTurnFromEnemiesUsecase sut;
    private GetNextEnemyMoveUsecase mockGetNextEnemyMoveUsecase;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(false);
                        Enemy.Move mockMove = MockEnemyUtil.GenerateMockMove();
                        e.CurrentMove.Returns(mockMove);
                        e.TurnsRemaining.Returns(11);
                    }),
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead().Returns(false);
                        Enemy.Move mockMove = MockEnemyUtil.GenerateMockMove();
                        e.CurrentMove.Returns(mockMove);
                        e.TurnsRemaining.Returns(10);
                    })
                },
                new List<int>
                {
                    1
                }
            ).SetName("2 alive enemies with 1 new move");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockGetNextEnemyMoveUsecase = GetNextEnemyMoveUsecaseTest.GenerateMock();
        sut = new(getNextEnemyMoveUsecase: mockGetNextEnemyMoveUsecase);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<Enemy> mockEnemies,
        List<int> enemyIndices
        )
    {
        List<(int enemyIndex, Enemy.Move move)> expected = enemyIndices.Select(i =>
            {
                Enemy.Move mockNewMove = MockEnemyUtil.GenerateMockMove();
                mockGetNextEnemyMoveUsecase.Invoke(mockEnemies[i]).Returns(mockNewMove);
                return (i, mockNewMove);
            }
        ).ToList();
        List<(int enemyIndex, Enemy.Move move)> result = sut.Invoke(enemies: mockEnemies);

        Assert.AreEqual(expected, result);

        expected.ForEach(p =>
        {
            mockEnemies[p.enemyIndex].Received(1).SetCurrentMove(p.move);
        }
        );

        TestUtils.ClearReceivedCalls(mockEnemies);
        TestUtils.ClearReceivedCalls(new List<GetNextEnemyMoveUsecase> { mockGetNextEnemyMoveUsecase });
    }

    public static CalculateTurnFromEnemiesUsecase GenerateMock()
    {
        return GenerateMock(_ => { });
    }

    public static CalculateTurnFromEnemiesUsecase GenerateMock(Action<CalculateTurnFromEnemiesUsecase> action)
    {
        CalculateTurnFromEnemiesUsecase mock = Substitute.For<CalculateTurnFromEnemiesUsecase>(
            GetNextEnemyMoveUsecaseTest.GenerateMock()
            );
        action(mock);
        return mock;
    }
}
