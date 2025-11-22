using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using static Level.Fight;

public class PopulateEnemiesUsecase
{
    private readonly Dictionary<EnemyEnum, EnemyInfo.DetailInfo> enemyInfoMap; 
    private readonly GetNextEnemyMoveUsecase getNextEnemyMoveUsecase;


    [Inject]
    public PopulateEnemiesUsecase(
        EnemyInfoRepository enemyInfoRepository,
        GetNextEnemyMoveUsecase getNextEnemyMoveUsecase
        )
    {
        EnemyInfo enemyInfo = enemyInfoRepository.Get();
        enemyInfoMap = new()
        {
            {EnemyEnum.Note, enemyInfo.Note },
            {EnemyEnum.Notebook, enemyInfo.Notebook }
        };
        this.getNextEnemyMoveUsecase = getNextEnemyMoveUsecase;
    }

    public List<Enemy> Invoke(List<EnemySummary> enemyArgs)
    {
        return enemyArgs.Select(e =>
        {
            EnemyEnum enemyEnum = e.EnemyEnum;
            RarityEnum enemyRarity = e.RarityEnum;
            EnemyInfo.DetailInfo statsInfo = enemyInfoMap[enemyEnum];
            int health = GetRarityValue(enemyRarity, statsInfo.health);

            List<Enemy.Move> moves = new();
            statsInfo.moves.ForEach(m =>
            {

                Enum.TryParse(m.type, out MoveEnum moveEnum);

                Enemy.Move move = new(
                            m.title,
                            m.description,
                            GetRarityValue(enemyRarity, m.value),
                            GetRarityValue(enemyRarity, m.wait),
                           GetRarityValue(enemyRarity, m.weight),
                           moveEnum
                    );


                moves.Add(move);
            });
            Enemy enemy = new(
                enemyEnum,
                enemyRarity,
                statsInfo.title,
                statsInfo.description,
                health,
                moves
                );

            Enemy.Move currentMove = getNextEnemyMoveUsecase.Invoke(enemy);
            enemy.SetCurrentMove(currentMove);
            return enemy;
        }).ToList();
    }

    private int GetRarityValue(
        RarityEnum rarity,
        EnemyInfo.DetailInfo.RarityInfo rarityInfo
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