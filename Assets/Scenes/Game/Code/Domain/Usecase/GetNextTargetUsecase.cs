using System.Collections.Generic;

public class GetNextTargetUsecase
{

    public int Invoke(
        bool preferRight,
        int currentAttackIndex,
        List<Enemy> enemies
        )
    {
        if (currentAttackIndex < 0 || currentAttackIndex >= enemies.Count)
        {
            return currentAttackIndex;
        }

        int direction = preferRight ? 1 : -1;
        int newAttackIndex = currentAttackIndex;
        int enemyCount = enemies.Count;

        for (int i = 0; i < enemyCount; i++)
        {
            newAttackIndex = (newAttackIndex + direction) % enemyCount;
            if (newAttackIndex < 0)
            {
                newAttackIndex = enemyCount - 1;
            }
            if (!enemies[newAttackIndex].IsDead())
            {
                break;
            }
        }
        return newAttackIndex;
    }
}
