using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using Zenject;

public class GetNextTargetUsecase
{

    internal int Invoke(
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

        for (int i = 0; i < enemies.Count; i++)
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
