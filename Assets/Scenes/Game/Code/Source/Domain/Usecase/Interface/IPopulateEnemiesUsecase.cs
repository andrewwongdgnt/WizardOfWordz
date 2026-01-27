using System.Collections.Generic;
using static Level.Fight;

public interface IPopulateEnemiesUsecase
{
    public List<Enemy> Invoke(List<EnemySummary> enemyArgs);
}