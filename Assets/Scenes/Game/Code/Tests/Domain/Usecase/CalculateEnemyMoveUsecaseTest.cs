using NSubstitute;
using NUnit.Framework;
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
                    generateMovePair(0, 1, MoveEnum.Heal),
                    generateMovePair(1, 2, MoveEnum.Heal)
                },
                new List<Enemy>
                {
                    generateMockEnemy(),
                    generateMockEnemy()
                },
                new List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)>
                {
                    (MoveEnum.Heal, 0, 1, 1),
                    (MoveEnum.Heal, 1, 2, 1)
                }
            ).SetName("2 unique heals");

            yield return new TestCaseData(
                new List<(int enemyIndex, Enemy.Move move)>
                {
                    generateMovePair(0, 2, MoveEnum.Heal),
                    generateMovePair(1, 2, MoveEnum.Heal)
                },
                new List<Enemy>
                {
                    generateMockEnemy(),
                    generateMockEnemy()
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
                    generateMovePair(0, 1, MoveEnum.Attack),
                    generateMovePair(1, 2, MoveEnum.Attack),
                },
                new List<Enemy>
                {
                    generateMockEnemy(),
                    generateMockEnemy()
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
                    generateMovePair(0, 9, MoveEnum.Attack),
                    generateMovePair(1, 9, MoveEnum.Attack),
                    generateMovePair(2, 9, MoveEnum.Attack)
                 },
                 new List<Enemy>
                 {
                    generateMockEnemy(),
                    generateMockEnemy(),
                    generateMockEnemy()
                 },
                 new List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)>
                 {
                    (MoveEnum.Attack, -1, 9, 3),
                 }
             ).SetName("3 same attacks");

            yield return new TestCaseData(
                 new List<(int enemyIndex, Enemy.Move move)>
                 {
                    generateMovePair(0, 2, MoveEnum.Attack),
                    generateMovePair(2, 5, MoveEnum.Attack),
                 },
                 new List<Enemy>
                 {
                    generateMockEnemy(),
                    generateMockEnemy(),
                    generateMockEnemy()
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
                    generateMovePair(0, 2, MoveEnum.Attack),
                    generateMovePair(1, 5, MoveEnum.Heal),
                 },
                 new List<Enemy>
                 {
                    generateMockEnemy(),
                    generateMockEnemy(),
                    generateMockEnemy()
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
        mockPlayerManager = Substitute.For<IPlayerManager>();
        sut = new(playerManager: mockPlayerManager);
    }

    [TearDown]
    public void TearDown()
    {
        // Optional cleanup
        // Example: reset static state, dispose resources
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<(int enemyIndex, Enemy.Move move)> movesPair,
        List<Enemy> enemies,
        List<(MoveEnum moveEnum, int enemyIndex, int value, int repeats)> expectedValues
        )
    {
        sut.Invoke(
            movesPair: movesPair,
            enemies: enemies
            );

        expectedValues.ForEach(ev =>
            {
                switch (ev.moveEnum)
                {
                    case MoveEnum.Attack:
                        mockPlayerManager.Received(ev.repeats).UpdateHealthBy(-ev.value);
                        break;
                    case MoveEnum.Heal:
                        enemies[ev.enemyIndex].Received(ev.repeats).UpdateHealthBy(ev.value);
                        break;
                }

            }
        );

    }

    private static Enemy generateMockEnemy()
    {
        return Substitute.For<Enemy>(
                        EnemyEnum.Note,
                        RarityEnum.Common,
                        "",
                        "",
                        100,
                        new List<Move>() { }
                    );
    }

    private static (int enemyIndex, Enemy.Move move) generateMovePair(
        int enemyIndex,
        int value,
        MoveEnum moveEnum
        )
    {
        return (
                    enemyIndex,
                    new(
                        "",
                        "",
                        value: value,
                        1,
                        1,
                        moveEnum
                    )
                );
    }
}
