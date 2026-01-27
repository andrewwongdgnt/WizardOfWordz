using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Enemy;

public class CalculateEnemyMoveUsecaseTest
{
    private CalculateEnemyMoveUsecase sut;
    private IPlayerManager mockPlayerManager;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<(int enemyIndex, Enemy.Move move)>
                {
                    MockEnemyUtil.GenerateMovePair(0, 5, MoveEnum.Heal),
                    MockEnemyUtil.GenerateMovePair(1, 2, MoveEnum.Heal)
                },
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy()
                },
                new List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)>
                {
                    (MoveEnum.Heal, 0, 5, 1),
                    (MoveEnum.Heal, 1, 2, 1)
                }
            ).SetName("2 unique heals");

            yield return new TestCaseData(
                new List<(int enemyIndex, Enemy.Move move)>
                {
                    MockEnemyUtil.GenerateMovePair(0, 2, MoveEnum.Heal),
                    MockEnemyUtil.GenerateMovePair(1, 2, MoveEnum.Heal)
                },
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy()
                },
                new List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)>
                {
                    (MoveEnum.Heal, 0, 2, 1),
                    (MoveEnum.Heal, 1, 2, 1)
                }
            ).SetName("2 same heals");

            yield return new TestCaseData(
                new List<(int enemyIndex, Enemy.Move move)>
                {
                    MockEnemyUtil.GenerateMovePair(0, 1, MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(1, 2, MoveEnum.Attack),
                },
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy()
                },
                new List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)>
                {
                    (MoveEnum.Attack, -1, 1, 1),
                    (MoveEnum.Attack, -1, 2, 1)
                }
            ).SetName("2 unique attacks");

            yield return new TestCaseData(
                 new List<(int enemyIndex, Enemy.Move move)>
                 {
                    MockEnemyUtil.GenerateMovePair(0, 9, MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(1, 9, MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(2, 9, MoveEnum.Attack)
                 },
                 new List<Enemy>
                 {
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy()
                 },
                 new List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)>
                 {
                    (MoveEnum.Attack, -1, 9, 3),
                 }
             ).SetName("3 same attacks");

            yield return new TestCaseData(
                 new List<(int enemyIndex, Enemy.Move move)>
                 {
                    MockEnemyUtil.GenerateMovePair(0, 2, MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(2, 5, MoveEnum.Attack),
                 },
                 new List<Enemy>
                 {
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy()
                 },
                 new List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)>
                 {
                    (MoveEnum.Attack, -1, 2, 1),
                    (MoveEnum.Attack, -1, 5, 1),
                 }
             ).SetName("2 attacks with 3 enemies");

            yield return new TestCaseData(
                 new List<(int enemyIndex, Enemy.Move move)>
                 {
                    MockEnemyUtil.GenerateMovePair(0, 2, MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(1, 5, MoveEnum.Heal),
                 },
                 new List<Enemy>
                 {
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy(),
                    MockEnemyUtil.GenerateMockEnemy()
                 },
                 new List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)>
                 {
                    (MoveEnum.Attack, -1, 2, 1),
                    (MoveEnum.Heal, 1, 5, 1),
                 }
             ).SetName("1 attack and 1 heal");
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
        List<(int enemyIndex, Enemy.Move move)> movesPair,
        List<Enemy> mockEnemies,
        List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)> expectedValues
        )
    {
        sut.Invoke(
            movesPair: movesPair,
            enemies: mockEnemies
            );

        expectedValues.ForEach(ev =>
            {
                switch (ev.moveEnum)
                {
                    case MoveEnum.Attack:
                        mockPlayerManager.Received(ev.repeats).UpdateHealthBy(-ev.value);
                        break;
                    case MoveEnum.Heal:
                        mockEnemies[ev.enemyIndex].Received(ev.repeats).UpdateHealthBy(ev.value);
                        break;
                }

            }
        );

        TestUtils.ClearReceivedCalls(mockEnemies);
    }

    public static CalculateEnemyMoveUsecase GenerateMock()
    {
        return GenerateMock(_ => { });
    }

    public static CalculateEnemyMoveUsecase GenerateMock(Action<CalculateEnemyMoveUsecase> action)
    {
        CalculateEnemyMoveUsecase mock = Substitute.For<CalculateEnemyMoveUsecase>(
             PlayerManagerTest.GenerateMock()
             );
        action(mock);
        return mock;
    }
}
