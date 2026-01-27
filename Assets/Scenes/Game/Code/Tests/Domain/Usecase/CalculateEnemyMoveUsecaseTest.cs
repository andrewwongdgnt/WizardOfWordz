using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Enemy;

public class CalculateEnemyMoveUsecaseTest
{
    private CalculateEnemyMoveUsecase sut;
    private IPlayerManager mockPlayerManager;

    public class MoveTestResult
    {
        MoveEnum moveEnum;
        public class Attack : MoveTestResult
        {
            public int value;
            public int repeats;
            public Attack(int value, int repeats)
            {
                moveEnum = MoveEnum.Attack;
                this.value = value;
                this.repeats = repeats;
            }
        }
        public class Heal : MoveTestResult
        {
            public int enemyIndex;
            public int value;
            public Heal(int enemyIndex, int value)
            {
                moveEnum = MoveEnum.Attack;
                this.enemyIndex = enemyIndex;
                this.value = value;
            }
        }
    }

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<(int enemyIndex, Enemy.Move move)>
                {
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 0, value: 5, moveEnum: MoveEnum.Heal),
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 1, value: 2, moveEnum: MoveEnum.Heal)
                },
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy()
                },
                new List<MoveTestResult>
                {
                    new MoveTestResult.Heal(0, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 5),
                    new MoveTestResult.Heal(1, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 2)
                }
            ).SetName("2 unique heals");

            yield return new TestCaseData(
                new List<(int enemyIndex, Enemy.Move move)>
                {
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 0, value: 2, moveEnum: MoveEnum.Heal),
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 1, value: 2, moveEnum: MoveEnum.Heal)
                },
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy()
                },
                new List<MoveTestResult>
                {
                    new MoveTestResult.Heal(0, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 2),
                    new MoveTestResult.Heal(1, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 2)
                }
            ).SetName("2 same heals");

            yield return new TestCaseData(
                new List<(int enemyIndex, Enemy.Move move)>
                {
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 0, value: 1, moveEnum: MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 1, value: 2, moveEnum: MoveEnum.Attack),
                },
                new List<Enemy>
                {
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy()
                },
                new List<MoveTestResult>
                {
                    new MoveTestResult.Attack(1, 1),
                    new MoveTestResult.Attack(2, 1)
                }
            ).SetName("2 unique attacks");

            yield return new TestCaseData(
                 new List<(int enemyIndex, Enemy.Move move)>
                 {
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 0, value: 9, moveEnum: MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 1, value: 9, moveEnum: MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 2, value: 9, moveEnum: MoveEnum.Attack)
                 },
                 new List<Enemy>
                 {
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy()
                 },
                 new List<MoveTestResult>
                 {
                   new MoveTestResult.Attack(9, 3),
                 }
             ).SetName("3 same attacks");

            yield return new TestCaseData(
                 new List<(int enemyIndex, Enemy.Move move)>
                 {
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 0, value: 2, moveEnum: MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 2, value: 5, moveEnum: MoveEnum.Attack),
                 },
                 new List<Enemy>
                 {
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy()
                 },
                 new List<MoveTestResult>
                 {
                    new MoveTestResult.Attack(2, 1),
                    new MoveTestResult.Attack(5, 1),
                 }
             ).SetName("2 attacks with 3 enemies");

            yield return new TestCaseData(
                 new List<(int enemyIndex, Enemy.Move move)>
                 {
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 0, value: 2, moveEnum: MoveEnum.Attack),
                    MockEnemyUtil.GenerateMovePair(enemyIndex: 1, value: 5, moveEnum: MoveEnum.Heal),
                 },
                 new List<Enemy>
                 {
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy(),
                    MockEnemyUtil.GenerateEnemy()
                 },
                 new List<MoveTestResult>
                 {
                   new MoveTestResult.Attack(2, 1),
                   new MoveTestResult.Heal(1, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 5),
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

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<(int enemyIndex, Enemy.Move move)> movesPair,
        List<Enemy> enemies,
        List<MoveTestResult> expectedValues
        )
    {
        sut.Invoke(
            movesPair: movesPair,
            enemies: enemies
            );

        expectedValues.ForEach(ev =>
            {
                if (ev is MoveTestResult.Attack attackResult)
                {
                    mockPlayerManager.Received(attackResult.repeats).UpdateHealthBy(-attackResult.value);
                }
                else if (ev is MoveTestResult.Heal healResult)
                {
                    Assert.AreEqual(enemies[healResult.enemyIndex].CurrentHealth, healResult.value);
                }
            }
        );
    }
}
