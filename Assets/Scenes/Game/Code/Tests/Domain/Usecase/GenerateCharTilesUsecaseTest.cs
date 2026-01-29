using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static Enemy;

public class GenerateCharTilesUsecaseTest
{
    private GenerateCharTilesUsecase sut;
    private IGetTileAdjustedScoreUsecase mockGetTileAdjustedScoreUsecase;
    private ILetterDistributionRepository mockLetterDistributionRepository;
    private IPlayerManager mockPlayerManager;
    private IGenerateRandomNumberUsecase mockGenerateRandomNumberUsecase;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                7,
                new List<TileInfo>
                {
                    MockTileUtil.GenerateTileInfo(value:'A', count: 4),
                    MockTileUtil.GenerateTileInfo(value:'B', count: 3),
                    MockTileUtil.GenerateTileInfo(value:'C', count: 2),
                }, //AAAABBBCC
                new List<int>
                {
                    7,
                    7,
                    0,
                    2,
                    3,
                    3,
                    0
                },
                new List<char>
                {
                    'C',
                    'C',
                    'A',
                    'A',
                    'B',
                    'B',
                    'A'
                }
            ).SetName("Picking 7 from 4A3B2C with RNG indices 7,7,0,2,3,3,0");

            yield return new TestCaseData(
                3,
                new List<TileInfo>
                {
                    MockTileUtil.GenerateTileInfo(value:'X', count: 5),
                    MockTileUtil.GenerateTileInfo(value:'Z', count: 2),
                }, //XXXXXZZ
                new List<int>
                {
                    4,
                    4,
                    4
                },
                new List<char>
                {
                    'X',
                    'Z',
                    'Z'
                }
            ).SetName("Picking 3 from 5X2Z with RNG indices 4,4,4");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockGetTileAdjustedScoreUsecase = Substitute.For<IGetTileAdjustedScoreUsecase>();
        mockLetterDistributionRepository = Substitute.For<ILetterDistributionRepository>();
        mockPlayerManager = Substitute.For<IPlayerManager>();
        mockGenerateRandomNumberUsecase = Substitute.For<IGenerateRandomNumberUsecase>();
        sut = new(
            getTileAdjustedScoreUsecase: mockGetTileAdjustedScoreUsecase,
            letterDistributionRepository: mockLetterDistributionRepository,
            playerManager: mockPlayerManager,
            generateRandomNumberUsecase: mockGenerateRandomNumberUsecase
            );
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        int tileCount,
        List<TileInfo> tileInfoList,
        List<int> randomNumbers,
        List<char> expected
        )
    {
        mockPlayerManager.TileCount.Returns(tileCount);
        mockLetterDistributionRepository.Get().Returns(tileInfoList);
        tileInfoList.ForEach(tileInfo =>
            {
                mockGetTileAdjustedScoreUsecase.Invoke(tileInfo.Value, tileInfo.Score).Returns(tileInfo.Score);
            }
        );

        int totalCount = tileInfoList.Sum(t => t.Count);

        for (int num = totalCount; num > totalCount - tileCount; num--)
        {
            mockGenerateRandomNumberUsecase.Invoke(num).Returns(randomNumbers[totalCount - num]);
        }

        List<char> result = sut.Invoke().Select(t => t.Value).ToList();

        Assert.AreEqual(expected, result);
    }
}
