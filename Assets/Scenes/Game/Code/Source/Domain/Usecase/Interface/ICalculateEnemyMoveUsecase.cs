using System.Collections.Generic;

public interface ICalculateEnemyMoveUsecase
{
    public void Invoke(
        List<(int enemyIndex, Enemy.Move move)> movesPair,
        List<Enemy> enemies
        );
}