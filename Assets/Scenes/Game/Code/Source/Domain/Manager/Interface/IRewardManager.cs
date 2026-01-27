using System;
using System.Collections.Generic;
using static Reward;

public interface IRewardManager
{
    public List<Reward> Present();
    public void Pick(Reward reward);
    public RewardValue GetCurrentValue(RewardEnum rewardEnum);
    public (int, int) GetCurrentAndFutureState(Reward reward);
}