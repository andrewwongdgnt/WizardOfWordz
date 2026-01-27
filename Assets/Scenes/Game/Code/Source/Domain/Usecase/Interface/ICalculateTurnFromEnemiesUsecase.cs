using System.Collections.Generic;

public interface ICalculateTurnFromEnemiesUsecase
{
    public List<(int enemyIndex, Enemy.Move move)> Invoke(List<Enemy> enemies);
}