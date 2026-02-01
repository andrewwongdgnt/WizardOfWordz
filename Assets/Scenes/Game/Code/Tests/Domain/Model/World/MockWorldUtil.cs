using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class MockWorldUtil
{
    public static WorldInfo GenerateWorldInfoForLibrary(
        string title = "",
        string description = "",
        List<int>? levelPicks = null,
        List<List<string>>? levelChoices = null
        )
    {
        return GenerateWorldInfo(
            worldEnum: WorldEnum.Library,
            title: title,
            description: description,
            levelPicks: levelPicks,
            levelChoices: levelChoices
            );
    }

    private static WorldInfo GenerateWorldInfo(
        WorldEnum worldEnum,
        string title,
        string description,
        List<int>? levelPicks = null,
        List<List<string>>? levelChoices = null
        )
    {
        List<int> resultingLevelPicks = levelPicks ?? new List<int>
        {
            3,
            3,
            3
        };

        List<List<string>> resultingLevelChoices = levelChoices ?? new List<List<string>>
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
        };

        return new()
        {
            Library = new()
            {
                title = title,
                description = description,
                levels = resultingLevelPicks.Zip(resultingLevelChoices, (pick, choices) => (pick, choices))
                .Select(p =>
                {
                    return new WorldInfo.DetailInfo.LevelSummaryInfo()
                    {
                        pick = p.pick,
                        choices = p.choices
                    };
                }).ToList()
            }
        };
    }
}