using UnityEngine;

public class RewardInfoRepository : IRewardInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("RewardInfo");

    public RewardInfo Get()
    {
        return JsonUtility.FromJson<RewardInfo>(jsonFile.text);
    }
}