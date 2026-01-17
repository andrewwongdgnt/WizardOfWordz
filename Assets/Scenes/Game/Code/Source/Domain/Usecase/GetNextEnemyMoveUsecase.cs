using System;
using System.Linq;

public class GetNextEnemyMoveUsecase
{
    private readonly Random random = new();

    public Enemy.Move Invoke(Enemy enemy)
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