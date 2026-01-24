using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
                        e.IsDead2().Returns(true);
                    }),  
                    MockEnemyUtil.GenerateMockEnemy(e =>
                    {
                        e.IsDead2().Returns(true);
                    })
                }
            ).SetName("DGNT");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockGetNextEnemyMoveUsecase = GetNextEnemyMoveUsecaseTest.GenerateMock(usecase =>
        {

        });
        sut = new(getNextEnemyMoveUsecase: mockGetNextEnemyMoveUsecase);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<Enemy> enemies
        )
    {
        //mockGetNextEnemyMoveUsecase.Invoke

        sut.Invoke(enemies);
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
