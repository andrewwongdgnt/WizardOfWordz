using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class GetWorldUsecaseTest
{
    private GetWorldUsecase sut;
    private IWorldInfoRepository mockWorldInfoRepository;
    private ILevelInfoRepository mockLevelInfoRepository;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                WorldEnum.Classroom,
                (
                "Lib",
                "Lib desc",
                (List<int>)null,
                (List<List<string>>)null
                )
            ).SetName("Library 1");

            yield return new TestCaseData(
                WorldEnum.Classroom,
                (
                "Lib",
                "Lib desc",
                 new List<int>
                 {
                    2,
                    4,
                    3
                 },
                 new List<List<string>>
                 {
                    new() {
                        "F_1_1_a",
                        "F_1_1_b",
                        "F_1_1_c",
                        "F_1_1_d",
                        "F_1_1_e",
                    },
                    new() {
                        "F_1_2_a",
                        "F_1_2_b",
                        "F_1_2_c",
                        "F_1_2_d",
                    },
                    new() {
                        "F_1_3_a",
                        "F_1_3_b",
                        "F_1_3_c",
                    },
                 }
                )
            ).SetName("Library 2");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockWorldInfoRepository = Substitute.For<IWorldInfoRepository>();
        mockLevelInfoRepository = Substitute.For<ILevelInfoRepository>();
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        WorldEnum worldEnum,
        (string title, string description, List<int> levelPicks, List<List<string>> levelChoices) worldInfoParam
      )
    {
        WorldInfo worldInfo = worldEnum switch
        {
            WorldEnum.Classroom => MockWorldUtil.GenerateWorldInfoForLibrary(
                title: worldInfoParam.title,
                description: worldInfoParam.description,
                levelPicks: worldInfoParam.levelPicks,
                levelChoices: worldInfoParam.levelChoices
                ),
            _ => throw new System.NotImplementedException(),
        };

        mockWorldInfoRepository.Get().Returns(worldInfo);
        mockLevelInfoRepository.Get().Returns(MockLevelUtil.GenerateMockLevelInfo());

        sut = new(
            worldInfoRepository: mockWorldInfoRepository,
            levelInfoRepository: mockLevelInfoRepository
            );

        World world = sut.Invoke(
            worldEnum
            );

        Assert.AreEqual(worldInfoParam.title, world.Title);
        Assert.AreEqual(worldInfoParam.description, world.Description);
        if (worldInfoParam.levelPicks != null && worldInfoParam.levelChoices != null)
        {
            Assert.AreEqual(worldInfoParam.levelPicks.Count, world.LevelChoices.Count);
            Assert.AreEqual(worldInfoParam.levelPicks, world.LevelChoices.Select(l => l.Pick));
            Assert.AreEqual(worldInfoParam.levelChoices.Count, world.LevelChoices.Count);
            Assert.AreEqual(worldInfoParam.levelChoices, world.LevelChoices.Select(l => l.Choices.Select(ll => ll.Title)));
        }
    }
}
