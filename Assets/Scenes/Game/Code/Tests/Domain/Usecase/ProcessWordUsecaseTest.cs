using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class ProcessWordUsecaseTest
{
    private ProcessWordUsecase sut;
    private ILetterDistributionRepository mockLetterDistributionRepository;
    private IGetTileAdjustedScoreUsecase mockGetTileAdjustedScoreUsecase;
    private IRetrieveWordsFromDictionaryUsecase mockRetrieveWordsFromDictionaryUsecase;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                "HELLO",
                new List<string>() { "HELLO", "HI" },
                3,
                0,
                8
            ).SetName("Word exists");

            yield return new TestCaseData(
                "HELLO",
                new List<string>() { "YO", "HI" },
                3,
                0,
                0
            ).SetName("Word doesn't exist");
        }
    }

    [SetUp]
    public void SetUp()
    {
        List<TileInfo> tileInfoList = MockTileUtil.GenerateMockTileInfoList();
        mockLetterDistributionRepository = Substitute.For<ILetterDistributionRepository>();
        mockLetterDistributionRepository.Get().Returns(tileInfoList);

        mockGetTileAdjustedScoreUsecase = Substitute.For<IGetTileAdjustedScoreUsecase>();
        tileInfoList.ForEach(tileInfo =>
            {
                mockGetTileAdjustedScoreUsecase.Invoke(tileInfo.Value, tileInfo.Score).Returns(tileInfo.Score);
            }
        );

        mockRetrieveWordsFromDictionaryUsecase = Substitute.For<IRetrieveWordsFromDictionaryUsecase>();
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        string word,
        List<string> validWords,
        int enemyCount,
        int attackIndex,
        int expectedScore
      )
    {
        mockRetrieveWordsFromDictionaryUsecase.Invoke().Returns(validWords.ToDictionary(w => w, w=> new Word(word, "tag")));

        sut = new(
            letterDistributionRepository: mockLetterDistributionRepository,
            getTileAdjustedScoreUsecase: mockGetTileAdjustedScoreUsecase,
            retrieveWordsFromDictionaryUsecase: mockRetrieveWordsFromDictionaryUsecase
            );

        List<Enemy> enemies = MockEnemyUtil.GenerateEnemies(enemyCount);

        sut.Invoke(
            word: word,
            enemies: enemies,
            attackIndex: attackIndex
            );

        Assert.AreEqual(MockEnemyUtil.DEFAULT_STARTING_HEALTH - expectedScore, enemies[attackIndex].CurrentHealth);
    }

}
