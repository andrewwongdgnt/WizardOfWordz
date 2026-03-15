using System;
using System.Collections.Generic;
using static Level.Fight;

public class MockLevelUtil
{
    public static Level GenerateLevel(
        LevelTypeEnum levelTypeEnum = LevelTypeEnum.Fight,
        string title = ""
        )
    {
        return levelTypeEnum switch
        {
            LevelTypeEnum.Fight => new Level.Fight(
                                levelEnum: LevelEnum.F_1_1_a,
                                title: title,
                                description: "",
                                enemies: new List<EnemySummary>(),
                                topRarity: RarityEnum.Common
                                ),
            _ => throw new Exception(),
        };
    }

    public static LevelInfo GenerateMockLevelInfo()
    {
        return new()
        {
            F_1_1_a = GenerateMockFight(LevelEnum.F_1_1_a.ToString()),
            F_1_1_b = GenerateMockFight(LevelEnum.F_1_1_b.ToString()),
            F_1_1_c = GenerateMockFight(LevelEnum.F_1_1_c.ToString()),
            F_1_1_d = GenerateMockFight(LevelEnum.F_1_1_d.ToString()),
            F_1_1_e = GenerateMockFight(LevelEnum.F_1_1_e.ToString()),
            F_1_2_a = GenerateMockFight(LevelEnum.F_1_2_a.ToString()),
            F_1_2_b = GenerateMockFight(LevelEnum.F_1_2_b.ToString()),
            F_1_2_c = GenerateMockFight(LevelEnum.F_1_2_c.ToString()),
            F_1_2_d = GenerateMockFight(LevelEnum.F_1_2_d.ToString()),
            F_1_3_a = GenerateMockFight(LevelEnum.F_1_3_a.ToString()),
            F_1_3_b = GenerateMockFight(LevelEnum.F_1_3_b.ToString()),
            F_1_3_c = GenerateMockFight(LevelEnum.F_1_3_c.ToString()),
        };
    }

    private static LevelInfo.DetailInfo.FightInfo GenerateMockFight(string id)
    {
        return new()
        {
            type = LevelTypeEnum.Fight.ToString(),
            title = id,
            description = id + " Desc",
            enemies = new List<LevelInfo.DetailInfo.FightInfo.EnemyArg>()
            {
                new()
                {
                    enemy = EnemyEnum.Note.ToString(),
                    rarity = RarityEnum.Common.ToString()
                }
            }
        };
    }
}