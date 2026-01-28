using System;
using System.Linq;
using Zenject;

public class GetNextEnemyMoveUsecase: IGetNextEnemyMoveUsecase
{
    private readonly IGenerateRandomNumberUsecase generateRandomNumberUsecase;

    [Inject]
    public GetNextEnemyMoveUsecase(
        IGenerateRandomNumberUsecase generateRandomNumberUsecase
        )
    {
        this.generateRandomNumberUsecase = generateRandomNumberUsecase;
    }

    public Enemy.Move Invoke(Enemy enemy)
    {
        int totalWeight = enemy.Moves.Sum(m => m.Weight);
        int randomValue = generateRandomNumberUsecase.Invoke(totalWeight);

        int cumulative = 0;
        foreach (var move in enemy.Moves)
        {
            cumulative += move.Weight;
            if (randomValue < cumulative)
                return move;
        }

        return enemy.Moves[generateRandomNumberUsecase.Invoke(enemy.Moves.Count)];
    }
}