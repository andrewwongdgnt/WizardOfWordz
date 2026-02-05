using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using static Level.Fight;

public class PopulateEnemiesUsecaseTest
{
    private PopulateEnemiesUsecase sut;
    private IEnemyInfoRepository mockEnemyInfoRepository;
    private IGetNextEnemyMoveUsecase mockGetNextEnemyMoveUsecase;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<EnemySummary>
                {
                    new(EnemyEnum.Note, RarityEnum.Epic),
                    new(EnemyEnum.Note, RarityEnum.Common),
                },
                new List<(EnemyEnum enemyEnum, RarityEnum enemyRarity)>
                {
                    (EnemyEnum.Note, RarityEnum.Epic),
                    (EnemyEnum.Note, RarityEnum.Common),
                }

            ).SetName("2 of the same enemies with different rarity");

            yield return new TestCaseData(
                new List<EnemySummary>
                {
                    new(EnemyEnum.Note, RarityEnum.Uncommon),
                    new(EnemyEnum.Notebook, RarityEnum.Uncommon),
                },
                new List<(EnemyEnum enemyEnum, RarityEnum enemyRarity)>
                {
                    (EnemyEnum.Note, RarityEnum.Uncommon),
                    (EnemyEnum.Notebook, RarityEnum.Uncommon),
                }

            ).SetName("2 different enemies with same rarity");

            yield return new TestCaseData(
                new List<EnemySummary>
                {
                    new(EnemyEnum.Note, RarityEnum.Legendary),
                    new(EnemyEnum.Notebook, RarityEnum.Rare),
                },
                new List<(EnemyEnum enemyEnum, RarityEnum enemyRarity)>
                {
                    (EnemyEnum.Note, RarityEnum.Legendary),
                    (EnemyEnum.Notebook, RarityEnum.Rare),
                }

            ).SetName("2 different enemies with different rarity");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockEnemyInfoRepository = Substitute.For<IEnemyInfoRepository>();
        mockGetNextEnemyMoveUsecase = Substitute.For<IGetNextEnemyMoveUsecase>();
        mockGetNextEnemyMoveUsecase.Invoke(Arg.Any<Enemy>()).Returns(MockEnemyUtil.GenerateMove());
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<EnemySummary> enemySummaryList,
        List<(EnemyEnum enemyEnum, RarityEnum enemyRarity)> expectedEnemies
      )
    {
        mockEnemyInfoRepository.Get().Returns(MockEnemyUtil.GenerateEnemyInfo());

        sut = new(
            enemyInfoRepository: mockEnemyInfoRepository,
            getNextEnemyMoveUsecase: mockGetNextEnemyMoveUsecase
            );

        List<Enemy> result = sut.Invoke(enemySummaryList);

        Assert.AreEqual(expectedEnemies, result.Select(r => (r.EnemyEnum, r.RarityEnum)).ToList());
    }

}
