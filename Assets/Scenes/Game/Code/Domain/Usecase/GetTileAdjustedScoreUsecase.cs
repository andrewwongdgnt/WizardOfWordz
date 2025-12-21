using Zenject;

public class GetTileAdjustedScoreUsecase
{
    private readonly RewardManager rewardManager;

    [Inject]
    public GetTileAdjustedScoreUsecase(
        RewardManager rewardManager
        )
    {
        this.rewardManager = rewardManager;
    }
    public int Invoke(char c, int baseScore)
    {
        int value = c switch
        {
            'A' or
            'E' or
            'I' or
            'O' or
            'U' => rewardManager.GetCurrentValue(RewardEnum.UpgradeAEIOU).Value,
            'L' or
            'N' or
            'R' or
            'S' or
            'T' => rewardManager.GetCurrentValue(RewardEnum.UpgradeLNRST).Value,
            'B' or
            'C' or
            'D' or
            'G' or
            'M' or
            'P' => rewardManager.GetCurrentValue(RewardEnum.UpgradeBCDGMP).Value,
            'F' or
            'K' or
            'H' or
            'V' or
            'W' or
            'Y' => rewardManager.GetCurrentValue(RewardEnum.UpgradeFKHVWY).Value,
            'J' or
            'X' or
            'Q' or
            'Z' => rewardManager.GetCurrentValue(RewardEnum.UpgradeJXQZ).Value,
            _ => 0
        };
        return baseScore + value;
    }
}