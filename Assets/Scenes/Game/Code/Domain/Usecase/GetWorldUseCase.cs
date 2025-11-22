using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class GetWorldUseCase
{

    private readonly WorldInfo worldInfo;
    private readonly LevelInfo levelInfo;
    [Inject]
    public GetWorldUseCase(
    WorldInfoRepository worldInfoRepository,
    LevelInfoRepository levelInfoRepository
    )
    {
        worldInfo = worldInfoRepository.Get();
        levelInfo = levelInfoRepository.Get();
    }

    public World Invoke(WorldEnum worldEnum)
    {
        WorldInfo.DetailInfo detailInfo = worldEnum switch
        {
            WorldEnum.Library => worldInfo.Library,
            _ => throw new System.NotImplementedException(),
        };

        List<World.LevelChoice> levelChoices = GetLevelChoices(detailInfo);
        return new World(
            worldEnum,
            detailInfo.title,
            detailInfo.description,
            levelChoices
            );
    }

    private List<World.LevelChoice> GetLevelChoices(WorldInfo.DetailInfo detailInfo)
    {
        return detailInfo.levels.Select(l =>
            new World.LevelChoice(
                l.pick,
                l.choices.Select(GetLevel).ToList()
                )
        ).ToList();
    }

    private Level GetLevel(string levelname)
    {
        LevelEnum levelEnum = (LevelEnum)Enum.Parse(typeof(LevelEnum), levelname);
        LevelInfo.DetailInfo levelDetail = levelEnum switch
        {
            LevelEnum.F_1_1_a => levelInfo.F_1_1_a,
            LevelEnum.F_1_1_b => levelInfo.F_1_1_b,
            LevelEnum.F_1_1_c => levelInfo.F_1_1_c,
            LevelEnum.F_1_1_d => levelInfo.F_1_1_d,
            LevelEnum.F_1_1_e => levelInfo.F_1_1_e,
            LevelEnum.F_1_2_a => levelInfo.F_1_2_a,
            LevelEnum.F_1_2_b => levelInfo.F_1_2_b,
            LevelEnum.F_1_2_c => levelInfo.F_1_2_c,
            LevelEnum.F_1_2_d => levelInfo.F_1_2_d,
            LevelEnum.F_1_3_a => levelInfo.F_1_3_a,
            LevelEnum.F_1_3_b => levelInfo.F_1_3_b,
            LevelEnum.F_1_3_c => levelInfo.F_1_3_c,
            _ => throw new NotImplementedException(),
        };
        return levelDetail.type switch
        {
            LevelTypeEnumConstants.FIGHT => CreateFightLevel(levelEnum, levelDetail),
            _ => throw new NotImplementedException()
        };
    }

    private Level.Fight CreateFightLevel(
        LevelEnum levelEnum,
        LevelInfo.DetailInfo levelDetail
        )
    {
        List<Level.Fight.EnemySummary> enemySummaries = levelDetail.enemies.Select(GetEnemySummary).ToList();

        return new(
         levelEnum,
         levelDetail.title,
         levelDetail.description,
         enemySummaries,
         enemySummaries.Max(e => e.RarityEnum)
         );
    }

    private Level.Fight.EnemySummary GetEnemySummary(LevelInfo.DetailInfo.EnemyArg enemyArg)
    {
        return new Level.Fight.EnemySummary(
            (EnemyEnum)Enum.Parse(typeof(EnemyEnum), enemyArg.enemy),
            (RarityEnum)Enum.Parse(typeof(RarityEnum), enemyArg.rarity)
            );
    }
}