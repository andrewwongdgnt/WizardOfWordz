using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class CalculateTurnFromEnemiesUsecase
{

    private readonly Random random = new();

    public List<(int enemyIndex, Enemy.Move move)> Invoke(List<Enemy> enemies)
    {
        return enemies.Select((e,i) =>
        {
            if (!e.IsDead())
                e.TurnsRemaining--;
            return (e,i);
        }).Where(p => p.e.TurnsRemaining < 0 && !p.e.IsDead())
        .Select(p =>
        {
            Enemy.Move move = GetMoveAtRandom(p.e);
            p.e.TurnsRemaining = move.Wait;
            return (p.i,move);
        }).ToList();
    }

    private Enemy.Move GetMoveAtRandom(Enemy enemy)
    {
        int totalWeight = enemy.Moves.Sum(m => m.Weight);
        int randomValue = random.Next(totalWeight);

        int cumulative = 0;
        foreach (var move in enemy.Moves)
        {
            cumulative += move.Weight;
            if (randomValue < cumulative)
                return move;
        }

        return enemy.Moves[random.Next(enemy.Moves.Count)];
    }
}