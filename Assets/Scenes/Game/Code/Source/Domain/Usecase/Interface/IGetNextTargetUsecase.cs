using System.Collections.Generic;

public interface IGetNextTargetUsecase
{
    public int Invoke(
        bool preferRight,
        int currentAttackIndex,
        List<Enemy> enemies
        );
}
