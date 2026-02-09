using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class RetrieveWordsFromDictionaryUsecaseTest
{
    private RetrieveWordsFromDictionaryUsecase sut;
    private IDictionaryRepository mockdictionaryRepository;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                new List<Word>() {
                    MockWordUtil.GenerateWord("SUPER"),
                    MockWordUtil.GenerateWord("MAN"),
                    MockWordUtil.GenerateWord("BAT"),
                },
                new Dictionary<string, string>()
                {
                    { "SUPER", "SUPER" },
                    { "MAN", "MAN" },
                    { "BAT", "BAT" }
                }
            ).SetName("3 word dictionary");
        }
    }

    [SetUp]
    public void SetUp()
    {
        mockdictionaryRepository = Substitute.For<IDictionaryRepository>();
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCases))]
    public void TestInvoke(
        List<Word> validWords,
        Dictionary<string, string> expected
      )
    {
        mockdictionaryRepository.Get().Returns(validWords);

        sut = new(
            dictionaryRepository: mockdictionaryRepository
            );

        Dictionary<string, string> result = sut.Invoke().ToDictionary(k => k.Key, v => v.Value.Value);

        Assert.AreEqual(expected, result);
    }

}
