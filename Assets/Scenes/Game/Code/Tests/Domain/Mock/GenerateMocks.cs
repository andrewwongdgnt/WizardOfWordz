using NSubstitute;
using System;
using System.Collections.Generic;
using static Enemy;

public class GenerateMocks
{
    public static Enemy GenerateMockEnemy()
    {
        return GenerateMockEnemy(_ => { });
    }

    public static Enemy GenerateMockEnemy(Action<Enemy> action)
    {
        Enemy mockEnemy = Substitute.For<Enemy>(
                        EnemyEnum.Note,
                        RarityEnum.Common,
                        "",
                        "",
                        100,
                        new List<Move>() { }
                    );
        action(mockEnemy);
        return mockEnemy;
    }

    public static (int enemyIndex, Enemy.Move move) GenerateMovePair(
        int enemyIndex,
        int value,
        MoveEnum moveEnum
        )
    {
        return (
                    enemyIndex,
                    new(
                        "",
                        "",
                        value: value,
                        1,
                        1,
                        moveEnum
                    )
                );
    }
}