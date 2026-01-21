using ModestTree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CalculateEnemyMoveUsecase
{
    private readonly IPlayerManager playerManager;

    [Inject]
    public CalculateEnemyMoveUsecase(
      IPlayerManager playerManager
      )
    {
        this.playerManager = playerManager;
    }

    public void Invoke(
        List<(int enemyIndex, Enemy.Move move)> movesPair,
        List<Enemy> enemies
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
                    enemy.UpdateHealthBy(value);
                    break;

            }
            
        });

    }
}