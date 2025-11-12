using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Zenject;
using static EnemyInfo;

public class PopulateEnemiesUsecase
{
    private readonly Dictionary<EnemyEnum, EnemyInfo.StatsInfo> enemyInfoMap;

    [Inject]
    public PopulateEnemiesUsecase(
        EnemyInfoRepository enemyInfoRepository
        )
    {
        EnemyInfo enemyInfo = enemyInfoRepository.Get();
        enemyInfoMap = new()
        {
            {EnemyEnum.Note, enemyInfo.Note },
            {EnemyEnum.Notebook, enemyInfo.Notebook }
        };
    }

    public List<Enemy> Invoke(List<EnemyArg> enemyArgs)
    {
        return enemyArgs.Select(e =>
        {
            EnemyEnum enemyEnum = e.enemyEnum;
            RarityEnum enemyRarity = e.rarityEnum;
            EnemyInfo.StatsInfo statsInfo = enemyInfoMap[enemyEnum];
            int health = GetRarityValue(enemyRarity, statsInfo.health);
            int delay = GetRarityValue(enemyRarity, statsInfo.delay);

            List<Enemy.Move> moves = new();
            statsInfo.moves.ForEach(m =>
            {
                Enemy.Move move = m.type switch
                {
                    "Attack" =>
                        new Enemy.Move.Attack(
                            m.title,
                            GetRarityValue(enemyRarity, m.wait),
                            m.weight,
                            GetRarityValue(enemyRarity, m.damage)
                            ),

                    _ => throw new NotImplementedException(),
                };
                moves.Add(move);
            });

            return new Enemy(
                enemyRarity,
                statsInfo.title,
                statsInfo.description,
                health,
                delay,
                moves
                );
        }).ToList();
    }

    private int GetRarityValue(
        RarityEnum rarity,
        EnemyInfo.StatsInfo.RarityInfo rarityInfo
        )
    {
        return rarity switch
        {
            RarityEnum.Common => rarityInfo.common,
            RarityEnum.Uncommon => rarityInfo.uncommon,
            RarityEnum.Rare => rarityInfo.rare,
            RarityEnum.Epic => rarityInfo.epic,
            RarityEnum.Legendary => rarityInfo.legendary,
            _ => throw new NotImplementedException(),
        };
    }
}