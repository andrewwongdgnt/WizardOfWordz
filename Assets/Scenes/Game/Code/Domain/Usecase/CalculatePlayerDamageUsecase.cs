using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

public class CalculatePlayerDamageUsecase
{
    public void Invoke(
        List<(int enemyIndex, Enemy.Move move)> movesPair,
        List<Enemy> enemies,
        PlayerManager playerManager
        )
    {
        if (movesPair.IsEmpty())
        {
            return;
        }
        string attackLog = string.Join(",", movesPair.Select(mp => $"{enemies[mp.enemyIndex].ShortLabel()} at {mp.enemyIndex} does {mp.move}"));
        Debug.Log(attackLog);
        movesPair.ForEach(mp =>
        {
            if (mp.move is Enemy.Move.Attack attack)
            {
                playerManager.UpdateHealthBy(-attack.Damage);
            }
        });

    }
}