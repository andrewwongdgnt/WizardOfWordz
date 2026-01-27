using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Enemy;

public class CalculateNextIndexUsecaseTest
{
    private CalculateNextIndexUsecase sut;

    public static IEnumerable<TestCaseData> InvokeTestCases
    {
        get
        {
            yield return new TestCaseData(
                true,
                0,
                5,
                1
            ).SetName("move right from 0 with max of 5");

            yield return new TestCaseData(
                true,
                4,
                5,
                0
            ).SetName("move right from 4 with max of 5");

            yield return new TestCaseData(
                true,
                0,
                1,
                0
            ).SetName("move right from 0 with max of 1");

            yield return new TestCaseData(
                false,
                4,
                5,
                3
            ).SetName("move left from 5 with max of 5");

            yield return new TestCaseData(
                false,
                0,
                5,
                4
            ).SetName("move left from 0 with max of 5");

            yield return new TestCaseData(
                false,
                0,
                1,
                0
            ).SetName("move left from 0 with max of 1");
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
       int currentIndex,
       int max,
       int expected
        )
    {
        Assert.AreEqual(expected, sut.Invoke(preferRight, currentIndex, max));
    }
}
