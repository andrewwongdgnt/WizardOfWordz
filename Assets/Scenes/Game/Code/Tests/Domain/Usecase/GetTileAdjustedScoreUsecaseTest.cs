using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static Enemy;

public class GetTileAdjustedScoreUsecaseTest
{
    private GetTileAdjustedScoreUsecase sut;
    private IRewardManager mockRewardManager;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                'A',
                1,
                2
            ).SetName("A with base score of 1");

            yield return new TestCaseData(
                'E',
                1,
                2
            ).SetName("E with base score of 1");

            yield return new TestCaseData(
                'I',
                1,
                2
            ).SetName("I with base score of 1");

            yield return new TestCaseData(
                'O',
                1,
                2
            ).SetName("O with base score of 1");

            yield return new TestCaseData(
                'U',
                1,
                2
            ).SetName("U with base score of 1");

            yield return new TestCaseData(
                'L',
                2,
                4
            ).SetName("L with base score of 2");

            yield return new TestCaseData(
                'N',
                2,
                4
            ).SetName("N with base score of 2");

            yield return new TestCaseData(
                'R',
                2,
                4
            ).SetName("R with base score of 2");

            yield return new TestCaseData(
                'S',
                2,
                4
            ).SetName("S with base score of 2");

            yield return new TestCaseData(
                'T',
                2,
                4
            ).SetName("T with base score of 2");

            yield return new TestCaseData(
                'B',
                1,
                4
            ).SetName("B with base score of 1");

            yield return new TestCaseData(
                'C',
                1,
                4
            ).SetName("C with base score of 1");

            yield return new TestCaseData(
                'D',
                1,
                4
            ).SetName("D with base score of 1");

            yield return new TestCaseData(
                'G',
                1,
                4
            ).SetName("G with base score of 1");

            yield return new TestCaseData(
                'M',
                1,
                4
            ).SetName("M with base score of 1");

            yield return new TestCaseData(
                'P',
                1,
                4
            ).SetName("P with base score of 1");

            yield return new TestCaseData(
                'F',
                2,
                6
            ).SetName("F with base score of 2");

            yield return new TestCaseData(
                'K',
                2,
                6
            ).SetName("K with base score of 2");

            yield return new TestCaseData(
                'H',
                2,
                6
            ).SetName("H with base score of 2");

            yield return new TestCaseData(
                'V',
                2,
                6
            ).SetName("V with base score of 2");

            yield return new TestCaseData(
                'W',
                2,
                6
            ).SetName("W with base score of 2");

            yield return new TestCaseData(
                'Y',
                2,
                6
            ).SetName("Y with base score of 2");

            yield return new TestCaseData(
                'J',
                3,
                8
            ).SetName("J with base score of 3");

            yield return new TestCaseData(
                'X',
                3,
                8
            ).SetName("X with base score of 3");

            yield return new TestCaseData(
                'Q',
                3,
                8
            ).SetName("Q with base score of 3");

            yield return new TestCaseData(
                'Z',
                3,
                8
            ).SetName("Z with base score of 3");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockRewardManager = Substitute.For<IRewardManager>();

        mockRewardManager.GetCurrentValue(RewardEnum.UpgradeAEIOU)
            .Returns(MockRewardUtil.GenerateRewardValue(value: 1));
        mockRewardManager.GetCurrentValue(RewardEnum.UpgradeLNRST)
            .Returns(MockRewardUtil.GenerateRewardValue(value: 2));
        mockRewardManager.GetCurrentValue(RewardEnum.UpgradeBCDGMP)
            .Returns(MockRewardUtil.GenerateRewardValue(value: 3));
        mockRewardManager.GetCurrentValue(RewardEnum.UpgradeFKHVWY)
            .Returns(MockRewardUtil.GenerateRewardValue(value: 4));
        mockRewardManager.GetCurrentValue(RewardEnum.UpgradeJXQZ)
            .Returns(MockRewardUtil.GenerateRewardValue(value: 5));
        sut = new(rewardManager: mockRewardManager);
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        char c,
        int baseScore,
        int expected
      )
    {
        int result = sut.Invoke(
            c: c,
            baseScore: baseScore
            );
        Assert.AreEqual(expected, result);
    }
}
