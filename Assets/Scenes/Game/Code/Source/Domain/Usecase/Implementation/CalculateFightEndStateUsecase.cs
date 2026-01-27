using System.Collections.Generic;
using System.Linq;
using Zenject;

public class CalculateFightEndStateUsecase: ICalculateFightEndStateUsecase
{
    private readonly IPlayerManager playerManager;

    [Inject]
    public CalculateFightEndStateUsecase(
      IPlayerManager playerManager
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

        if (enemies.All(e => e.IsDead()))
        {

            return FightEndStateEnum.Win;
        }

        return FightEndStateEnum.Ongoing;
    }
}