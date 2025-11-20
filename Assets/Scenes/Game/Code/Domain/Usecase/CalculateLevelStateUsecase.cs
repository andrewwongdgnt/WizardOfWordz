using System.Collections.Generic;
using System.Linq;

public class CalculateLevelStateUsecase
{
    public FightEndStateEnum Invoke(
        List<Enemy> enemies,
        PlayerManager playerManager
        )
    {
        if (enemies.All(e => e.IsDead()))
        {

            return FightEndStateEnum.Win;
        }

        if (playerManager.IsDead())
        {
            return FightEndStateEnum.Lose;
        }

        return FightEndStateEnum.Ongoing;
    }
}