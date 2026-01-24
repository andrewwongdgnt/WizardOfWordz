using System.Collections.Generic;
using System.Linq;
using Zenject;

public class CalculateFightEndStateUsecase
{
    private readonly PlayerManager playerManager;

    [Inject]
    public CalculateFightEndStateUsecase(
      PlayerManager playerManager
      )
    {
        this.playerManager = playerManager;
    }

    public FightEndStateEnum Invoke(
        List<Enemy> enemies
        )
    {
        if (playerManager.IsDead())
        {
            return FightEndStateEnum.Lose;
        }

        if (enemies.All(e => e.IsDead2()))
        {

            return FightEndStateEnum.Win;
        }

        return FightEndStateEnum.Ongoing;
    }
}