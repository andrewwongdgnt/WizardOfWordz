using NSubstitute;
using System;
using System.Collections.Generic;
using static Enemy;

public class MockEnemyUtil
{
    public const int DEFAULT_MAX_HEALTH = 100;
    public const int DEFAULT_STARTING_HEALTH = 50;

    public static Enemy GenerateEnemy(
            EnemyEnum enemyEnum = EnemyEnum.Note,
            RarityEnum rarityEnum = RarityEnum.Common,
            string title = "",
            string description = "",
            int health = DEFAULT_MAX_HEALTH,
            int startingHealth = DEFAULT_STARTING_HEALTH,
            List<Move>? moves = null
        )
    {
        Enemy enemy = new(
                enemyEnum: enemyEnum,
                rarityEnum: rarityEnum,
                title: title,
                description: description,
                health: health,
                moves: moves ?? new List<Move>() { }
            );
        enemy.UpdateHealthBy(startingHealth - health);
        return enemy;
    }

    public static (int enemyIndex, Enemy.Move move) GenerateMovePair(
        int enemyIndex,
        string title = "",
        string description = "",
        int value = 0,
        int wait = 1,
        int weight = 1,
        MoveEnum moveEnum = MoveEnum.Attack
        )
    {
        return (
                enemyIndex,
                new(
                    title: title,
                    description: description,
                    value: value,
                    wait: wait,
                    weight: weight,
                    moveEnum
                )
            );
    }
}