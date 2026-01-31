using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static Enemy;

public class GetNextTargetUsecaseTest
{
    private GetNextTargetUsecase sut;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                true,
                0,
                new List<bool>
                {
                    true,
                    true,
                    true
                },
                1
            ).SetName("AAA starting at 0 going right");

            yield return new TestCaseData(
                true,
                0,
                new List<bool>
                {
                    true,
                    false,
                    true
                },
                2
            ).SetName("ADA starting at 0 going right");

            yield return new TestCaseData(
                true,
                2,
                new List<bool>
                {
                    true,
                    true,
                    true
                },
                0
            ).SetName("AAA starting at 2 going right");

            yield return new TestCaseData(
                true,
                0,
                new List<bool>
                {
                    true
                },
                0
            ).SetName("A starting at 0 going right");

            yield return new TestCaseData(
                true,
                0,
                new List<bool>
                {
                    true,
                    false,
                    false
                },
                0
            ).SetName("ADD starting at 0 going right");

            yield return new TestCaseData(
                false,
                2,
                new List<bool>
                {
                    true,
                    true,
                    true
                },
                1
            ).SetName("AAA starting at 0 going left");

            yield return new TestCaseData(
                false,
                2,
                new List<bool>
                {
                    true,
                    false,
                    true
                },
                0
            ).SetName("ADA starting at 2 going left");

            yield return new TestCaseData(
                false,
                0,
                new List<bool>
                {
                    true,
                    true,
                    true
                },
                2
            ).SetName("AAA starting at 0 going left");

            yield return new TestCaseData(
                false,
                0,
                new List<bool>
                {
                    true
                },
                0
            ).SetName("A starting at 0 going left"); 

            yield return new TestCaseData(
                false,
                2,
                new List<bool>
                {
                    false,
                    false,
                    true
                },
                2
            ).SetName("DDA starting at 0 going left");

        }
    }

    [SetUp]
    public void SetUp()
    {
        sut = new();
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        bool preferRight,
        int currentAttackIndex,
        List<bool> aliveState,
        int expected
      )
    {
        List<Enemy> enemies = aliveState.Select(v =>
        {
            return v ? MockEnemyUtil.GenerateEnemy() : MockEnemyUtil.GenerateEnemy(startingHealth: 0);
        }).ToList();

        int result = sut.Invoke(
            preferRight: preferRight,
            currentAttackIndex: currentAttackIndex,
            enemies: enemies
            );
        Assert.AreEqual(expected, result);
    }
}
