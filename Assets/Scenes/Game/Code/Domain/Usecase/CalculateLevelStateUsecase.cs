using System.Collections.Generic;
using System.Linq;

public class CalculateLevelStateUsecase
{
    public LevelStateEnum Invoke(
        List<Enemy> enemies,
        PlayerManager playerManager
        )
    {
        if (enemies.All(e => e.IsDead()))
        {

            return LevelStateEnum.Win;
        }

        if (playerManager.IsDead())
        {
            return LevelStateEnum.Lose;
        }

        return LevelStateEnum.Ongoing;
    }
}