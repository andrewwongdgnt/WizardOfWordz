using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class PickTileUsecaseTest
{
    private PickTileUsecase sut;

    public static IEnumerable<TestCaseData> InvokeTestCasesFromKey
    {
        get
        {
            yield return new TestCaseData(
                Key.A,
                new List<(char value, bool pickable)>
                {
                    ('A', true),
                    ('B', true),
                    ('C', true)
                },
                (
                    'A',
                    new List<bool>
                    {
                        false,
                        true,
                        true
                    }
                )
            ).SetName("Key A from ABC");

            yield return new TestCaseData(
                Key.A,
                new List<(char value, bool pickable)>
                {
                                ('A', false),
                                ('B', true),
                                ('C', true)
                },
                (
                    '\0',
                    new List<bool>
                    {
                        false,
                        true,
                        true
                    }
                )
            ).SetName("Key A from aBC");

            yield return new TestCaseData(
                Key.A,
                new List<(char value, bool pickable)>
                {
                                ('A', false),
                                ('A', true),
                                ('B', true),
                                ('C', true)
                },
                (
                    'A',
                    new List<bool>
                    {
                        false,
                        false,
                        true,
                        true
                    }
                )
            ).SetName("Key A from aABC");

            yield return new TestCaseData(
                Key.A,
                new List<(char value, bool pickable)>
                {
                                ('A', true),
                                ('A', false),
                                ('B', true),
                                ('C', true)
                },
                (
                    'A',
                    new List<bool>
                    {
                        false,
                        false,
                        true,
                        true
                    }
                )
            ).SetName("Key A from AaBC");

            yield return new TestCaseData(
                Key.C,
                new List<(char value, bool pickable)>
                {
                    ('A', true),
                    ('B', true),
                    ('C', true)
                },
                (
                    'C',
                    new List<bool>
                    {
                        true,
                        true,
                        false
                    }
                )
            ).SetName("Key C from ABC");

            yield return new TestCaseData(
                Key.Z,
                new List<(char value, bool pickable)>
                {
                    ('A', true),
                    ('B', true),
                    ('C', true)
                },
                (
                    '\0',
                    new List<bool>
                    {
                        true,
                        true,
                        true
                    }
                )
            ).SetName("Key Z from ABC");
        }
    }

    public static IEnumerable<TestCaseData> InvokeTestCasesFromTile
    {
        get
        {
            yield return new TestCaseData(
                0,
                new List<(char value, bool pickable)>
                {
                    ('A', true),
                    ('B', true),
                    ('C', true)
                },
                (
                    true,
                    new List<bool>
                    {
                        false,
                        true,
                        true
                    }
                )
            ).SetName("Tile at 0 from ABC");

            yield return new TestCaseData(
                0,
                new List<(char value, bool pickable)>
                {
                                ('A', false),
                                ('B', true),
                                ('C', true)
                },
                (
                    true,
                    new List<bool>
                    {
                        true,
                        true,
                        true
                    }
                )
            ).SetName("Tile at 0 from aBC");

            yield return new TestCaseData(
                2,
                new List<(char value, bool pickable)>
                {
                                ('A', false),
                                ('A', true),
                                ('B', true),
                                ('C', true)
                },
                (
                    true,
                    new List<bool>
                    {
                        false,
                        true,
                        false,
                        true
                    }
                )
            ).SetName("Tile at 2 from aABC");

            yield return new TestCaseData(
                -1,
                new List<(char value, bool pickable)>
                {
                                ('A', true),
                                ('A', false),
                                ('B', true),
                                ('C', true)
                },
                (
                    false,
                    new List<bool>
                    {
                        true,
                        false,
                        true,
                        true
                    }
                )
            ).SetName("Non existent tile from AaBC");
        }
    }

    [SetUp]
    public void SetUp()
    {
        sut = new();
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCasesFromKey))]
    public void TestInvokeFromKey(
        Key key,
        List<(char value, bool pickable)> paramList,
        (char value, List<bool> pickableList) expectedParams
      )
    {
        List<Tile> allowedTiles = GenerateAllowedTiles(paramList);
        Tile tile = sut.Invoke(
            key: key,
            allowedTiles: allowedTiles
            );

        if (expectedParams.value == '\0')
        {
            Assert.Null(tile);
        }
        else
        {
            Assert.AreEqual(expectedParams.value, tile.Value);
        }
        Assert.AreEqual(expectedParams.pickableList, allowedTiles.Select(t => t.pickable).ToList());
    }

    [Test]
    [TestCaseSource(nameof(InvokeTestCasesFromTile))]
    public void TestInvokeFromTile(
        int tileIndex,
        List<(char value, bool pickable)> paramList,
        (bool exists, List<bool> pickableList) expectedParams
      )
    {
        List<Tile> allowedTiles = GenerateAllowedTiles(paramList);
        bool result = sut.Invoke(
            tile: tileIndex >=0 ? allowedTiles[tileIndex] : MockTileUtil.GenerateTile(),
            allowedTiles: allowedTiles
            );

        Assert.AreEqual(expectedParams.pickableList, allowedTiles.Select(t => t.pickable).ToList());
        Assert.AreEqual(expectedParams.exists, result);
    }

    private List<Tile> GenerateAllowedTiles(List<(char value, bool pickable)> paramList)
    {
        return paramList.Select(p =>
            MockTileUtil.GenerateTile
                (
                    value: p.value,
                    pickable: p.pickable
                )
        ).ToList();
    }
}
