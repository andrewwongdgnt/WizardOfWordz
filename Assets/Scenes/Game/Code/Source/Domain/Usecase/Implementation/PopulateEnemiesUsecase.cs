using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using static Level.Fight;

public class PopulateEnemiesUsecase: IPopulateEnemiesUsecase
{
    private readonly Dictionary<EnemyEnum, EnemyInfo.DetailInfo> enemyInfoMap; 
    private readonly IGetNextEnemyMoveUsecase getNextEnemyMoveUsecase;


    [Inject]
    public PopulateEnemiesUsecase(
        IEnemyInfoRepository enemyInfoRepository,
        IGetNextEnemyMoveUsecase getNextEnemyMoveUsecase
        )
    {
        EnemyInfo enemyInfo = enemyInfoRepository.Get();
        enemyInfoMap = EnemyInfo.GetEnemyInfoMap(enemyInfo);
        this.getNextEnemyMoveUsecase = getNextEnemyMoveUsecase;
    }

    public List<Enemy> Invoke(List<EnemySummary> enemyArgs)
    {
        return enemyArgs.Select(e =>
        {
            EnemyEnum enemyEnum = e.EnemyEnum;
            RarityEnum enemyRarity = e.RarityEnum;
            EnemyInfo.DetailInfo statsInfo = enemyInfoMap[enemyEnum];
            int health = GetHealthValue(enemyRarity, statsInfo.health);

            List<Enemy.Move> moves = statsInfo.moves.Select(m =>
            {
                Enum.TryParse(m.type, out MoveEnum moveEnum);

                Enemy.Move move = new(
                            m.title,
                            m.description,
                            GetHealthValue(enemyRarity, m.value),
                            GetHealthValue(enemyRarity, m.wait),
                           GetHealthValue(enemyRarity, m.weight),
                           moveEnum
                    );

                return move;
            }).ToList();
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

    private int GetHealthValue(
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