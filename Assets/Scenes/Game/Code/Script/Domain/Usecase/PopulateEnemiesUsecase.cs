using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Zenject;

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
            int health = enemyRarity switch
            {
                RarityEnum.Common => statsInfo.health.common,
                RarityEnum.Uncommon => statsInfo.health.uncommon,
                RarityEnum.Rare => statsInfo.health.rare,
                RarityEnum.Epic => statsInfo.health.epic,
                RarityEnum.Legendary => statsInfo.health.legendary,
                _ => throw new NotImplementedException(),
            };

            List<Enemy.Move> moves = new();
            statsInfo.moves.ForEach(m =>
            {
                int wait = enemyRarity switch
                {
                    RarityEnum.Common => m.wait.common,
                    RarityEnum.Uncommon => m.wait.uncommon,
                    RarityEnum.Rare => m.wait.rare,
                    RarityEnum.Epic => m.wait.epic,
                    RarityEnum.Legendary => m.wait.legendary,
                    _ => throw new NotImplementedException(),
                };
                Enemy.Move move = m.type switch
                {
                    "Attack" =>
                        new Enemy.Move.Attack(
                            m.title,
                            wait,
                            m.weight,
                            enemyRarity switch
                            {
                                RarityEnum.Common => m.damage.common,
                                RarityEnum.Uncommon => m.damage.uncommon,
                                RarityEnum.Rare => m.damage.rare,
                                RarityEnum.Epic => m.damage.epic,
                                RarityEnum.Legendary => m.damage.legendary,
                                _ => throw new NotImplementedException(),
                            }
                            ),

                    _ => throw new NotImplementedException(),
                };
                moves.Add(move);
            });

            return new Enemy(
                enemyEnum,
                enemyRarity,
                statsInfo.description,
                health,
                moves
                );
        }).ToList();
    }
}