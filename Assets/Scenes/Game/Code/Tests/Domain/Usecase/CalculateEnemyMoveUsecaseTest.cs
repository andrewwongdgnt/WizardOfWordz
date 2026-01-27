using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            public int repeats;
            public int value;
            public Attack(int repeats, int value)
            {
                moveEnum = MoveEnum.Attack;
                this.repeats = repeats;
                this.value = value;
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
                new List<(int enemyIndex, int value, MoveEnum moveEnum)>
                {
                    (0, 5, MoveEnum.Heal),
                    (1, 2, MoveEnum.Heal)
                },
                2,
                new List<MoveTestResult>
                {
                    new MoveTestResult.Heal(0, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 5),
                    new MoveTestResult.Heal(1, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 2)
                }
            ).SetName("2 unique heals");

            yield return new TestCaseData(
                new List<(int enemyIndex, int value, MoveEnum moveEnum)>
                {
                    (0, 2, MoveEnum.Heal),
                    (1, 2, MoveEnum.Heal)
                },
                2,
                new List<MoveTestResult>
                {
                    new MoveTestResult.Heal(0, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 2),
                    new MoveTestResult.Heal(1, MockEnemyUtil.DEFAULT_STARTING_HEALTH + 2)
                }
            ).SetName("2 same heals");

            yield return new TestCaseData(
                new List<(int enemyIndex, int value, MoveEnum moveEnum)>
                {
                    (0, 1, MoveEnum.Attack),
                    (1, 2, MoveEnum.Attack)
                },
                2,
                new List<MoveTestResult>
                {
                    new MoveTestResult.Attack(1, 1),
                    new MoveTestResult.Attack(1, 2)
                }
            ).SetName("2 unique attacks");

            yield return new TestCaseData(
                 new List<(int enemyIndex, int value, MoveEnum moveEnum)>
                 {
                   (0, 9, MoveEnum.Attack),
                   (1, 9, MoveEnum.Attack),
                   (2, 9, MoveEnum.Attack),
                 },
                 3,
                 new List<MoveTestResult>
                 {
                   new MoveTestResult.Attack(3, 9),
                 }
             ).SetName("3 same attacks");

            yield return new TestCaseData(
                 new List<(int enemyIndex, int value, MoveEnum moveEnum)>
                 {
                    (0, 2, MoveEnum.Attack),
                    (2, 5, MoveEnum.Attack),
                 },
                 3,
                 new List<MoveTestResult>
                 {
                    new MoveTestResult.Attack(1, 2),
                    new MoveTestResult.Attack(1, 5),
                 }
             ).SetName("2 attacks with 3 enemies");

            yield return new TestCaseData(
                 new List<(int enemyIndex, int value, MoveEnum moveEnum)>
                 {
                    (0, 2, MoveEnum.Attack),
                    (1, 5, MoveEnum.Heal),
                 },
                 3,
                 new List<MoveTestResult>
                 {
                   new MoveTestResult.Attack(1, 2),
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
        List<(int enemyIndex, int value, MoveEnum moveEnum)> movesParam,
        int enemyCount,
        List<MoveTestResult> expectedValues
        )
    {

        List<(int enemyIndex, Enemy.Move move)> movesPair = movesParam.Select(param =>
        {
            return MockEnemyUtil.GenerateMovePair(enemyIndex: param.enemyIndex, value: param.value, moveEnum: param.moveEnum);
        }).ToList();

        List<Enemy> enemies = Enumerable.Range(1, enemyCount).Select(i =>
        {
            return MockEnemyUtil.GenerateEnemy();
        }).ToList();

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
