using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static CalculateEnemyMoveUsecaseTest;
using static Enemy;
using static UnityEngine.EventSystems.EventTrigger;

public class GetNextEnemyMoveUsecaseTest
{
    private GetNextEnemyMoveUsecase sut;
    private IGenerateRandomNumberUsecase mockGenerateRandomNumberUsecase;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<(string moveTitle, int moveWeight)>
                {
                    ("1",10),
                    ("2",10),
                    ("3",10)
                },
                20,
                "3"
            ).SetName("weights of 10, 10, 10 with 20 generated");

            yield return new TestCaseData(
                new List<(string moveTitle, int moveWeight)>
                {
                    ("1",10),
                    ("2",10),
                    ("3",10)
                },
                19,
                "2"
            ).SetName("weights of 10, 10, 10 with 19 generated");

            yield return new TestCaseData(
                new List<(string moveTitle, int moveWeight)>
                {
                    ("1",0),
                    ("2",20),
                    ("3",80)
                },
                0,
                "2"
            ).SetName("weights of 0, 20, 80 with 0 generated");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockGenerateRandomNumberUsecase = Substitute.For<IGenerateRandomNumberUsecase>();
        sut = new(generateRandomNumberUsecase: mockGenerateRandomNumberUsecase);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
      List<(string moveTitle, int moveWeight)> movesParam,
      int randomNumberGenerated,
      string title
      )
    {
        List<Move> moves = movesParam.Select(p =>
            {
                return MockEnemyUtil.GenerateMove(title: p.moveTitle, weight: p.moveWeight);
            }
        ).ToList();
        Enemy enemy = MockEnemyUtil.GenerateEnemy(moves: moves);

        int totalWeight = enemy.Moves.Sum(m => m.Weight);
        mockGenerateRandomNumberUsecase.Invoke(totalWeight).Returns(randomNumberGenerated);

        Move move = sut.Invoke(enemy);
        Assert.AreEqual(title, move.Title);

        mockGenerateRandomNumberUsecase.Received(1).Invoke(totalWeight);
    }
}
