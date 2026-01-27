using System.Collections.Generic;
using System.Linq;
using Zenject;

public class CalculateTurnFromEnemiesUsecase: ICalculateTurnFromEnemiesUsecase
{
    private readonly IGetNextEnemyMoveUsecase getNextEnemyMoveUsecase;

    [Inject]
    public CalculateTurnFromEnemiesUsecase(
        IGetNextEnemyMoveUsecase getNextEnemyMoveUsecase
        )
    {
        this.getNextEnemyMoveUsecase = getNextEnemyMoveUsecase;
    }

    public List<(int enemyIndex, Enemy.Move move)> Invoke(List<Enemy> enemies)
    {

        List<(int, Enemy)> aliveEnemies = enemies.Select((e, i) => (i, e))
            .Where(p => !p.e.IsDead())
            .ToList();

        List<(int, Enemy.Move)> moveSet = new();

        aliveEnemies.ForEach(p =>
        {
            int index = p.Item1;
            Enemy enemy = p.Item2;
            
            if (enemy.CurrentMove != null)
            {
                enemy.TurnsRemaining -= 3;
            }
            
            if (enemy.TurnsRemaining <= 0)
            {
                Enemy.Move newMove = getNextEnemyMoveUsecase.Invoke(enemy);
                enemy.SetCurrentMove(newMove);
                moveSet.Add((index, newMove));
            }
        });
        return moveSet;
    }
}