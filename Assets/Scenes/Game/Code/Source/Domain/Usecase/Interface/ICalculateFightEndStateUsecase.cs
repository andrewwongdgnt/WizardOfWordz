using System.Collections.Generic;

public interface ICalculateFightEndStateUsecase
{
    public FightEndStateEnum Invoke(
        List<Enemy> enemies
        );
}