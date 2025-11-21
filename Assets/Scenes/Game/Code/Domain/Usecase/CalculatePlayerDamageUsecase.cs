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
            Enemy enemy = enemies[mp.enemyIndex];
            Enemy.Move move = mp.move;
            int value = move.Value;
            switch (move.MoveEnum)
            {
                case MoveEnum.Attack:
                    playerManager.UpdateHealthBy(-value);
                    break;
                case MoveEnum.Heal:
                    enemy.Heal(value);
                    break;

            }
            
        });

    }
}