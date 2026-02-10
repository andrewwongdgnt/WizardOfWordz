using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static World;

public class SelectLevelChoicesUsecaseTest
{
    private SelectLevelChoicesUsecase sut;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                0,
                new List<World.LevelChoice> { 
                    new LevelChoice(
                        3,
                        new List<Level>
                            {
                                MockLevelUtil.GenerateLevel(title: "level1"),
                                MockLevelUtil.GenerateLevel(title: "level2"),
                                MockLevelUtil.GenerateLevel(title: "level3"),
                                MockLevelUtil.GenerateLevel(title: "level4"),
                                MockLevelUtil.GenerateLevel(title: "level5"),
                            }
                        )
                },
                new List<string>
                {
                    "level2",
                    "level3",
                    "level4",
                }
            ).SetName("Picking 3 levels out of 5 from stage 1");

            yield return new TestCaseData(
                1,
                new List<World.LevelChoice> {
                    new LevelChoice(
                        2,
                        new List<Level>
                            {
                                MockLevelUtil.GenerateLevel(title: "level1"),
                                MockLevelUtil.GenerateLevel(title: "level2"),
                                MockLevelUtil.GenerateLevel(title: "level3")
                            }
                        ),
                    new LevelChoice(
                        3,
                        new List<Level>
                            {
                                MockLevelUtil.GenerateLevel(title: "level1"),
                                MockLevelUtil.GenerateLevel(title: "level2"),
                                MockLevelUtil.GenerateLevel(title: "level3"),
                                MockLevelUtil.GenerateLevel(title: "level4"),
                                MockLevelUtil.GenerateLevel(title: "level5"),
                            }
                        )
                },
                new List<string>
                {
                    "level2",
                    "level3",
                    "level4",
                }
            ).SetName("Picking 3 levels out of 5 from stage 2");

        }
    }

    [SetUp]
    public void SetUp()
    {
        IGenerateRandomNumberUsecase mockGenerateRandomNumberUsecase = Substitute.For<IGenerateRandomNumberUsecase>();
        mockGenerateRandomNumberUsecase.Invoke(Arg.Any<int>()).Returns(0);

        sut = new(
            generateRandomNumberUsecase: mockGenerateRandomNumberUsecase
            );
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        int levelChoiceIndex,
        List<World.LevelChoice> levelChoices,
        List<string> expectedLevels
      )
    {
        List<Level> levels = sut.Invoke(
            levelChoiceIndex: levelChoiceIndex,
            levelChoices: levelChoices
            );
        List<string> result = levels.Select(l => l.Title).ToList();

        Assert.AreEqual(expectedLevels, result);
    }

}
