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
            List<Move>? moves = null,
            Action<Enemy>? action = null
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
        action?.Invoke(enemy);
        return enemy;
    }

    public static Move GenerateMove(
        string title = "",
        string description = "",
        int value = 0,
        int wait = 1,
        int weight = 1,
        MoveEnum moveEnum = MoveEnum.Attack
        )
    {
        return new(
                    title: title,
                    description: description,
                    value: value,
                    wait: wait,
                    weight: weight,
                    moveEnum
                );
    }

    public static (int enemyIndex, Move move) GenerateMovePair(
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
                GenerateMove(
                    title: title,
                    description: description,
                    value: value,
                    wait: wait,
                    weight: weight,
                    moveEnum
                )
            );
    }

    public static EnemyInfo GenerateEnemyInfo()
    {
        EnemyInfo.DetailInfo.RarityInfo mockHealth = new()
        {
            common = 1,
            uncommon = 1,
            rare = 1,
            epic = 1,
            legendary = 1
        };
        return new()
        {
            Note = new()
            {
                title = "noteTitle",
                description = "noteDescription",
                health = mockHealth,
                moves = new()

            },
            Notebook = new()
            {
                title = "noteBookTitle",
                description = "notebookDescription",
                health = mockHealth,
                moves = new()
            }
        };
    }
}