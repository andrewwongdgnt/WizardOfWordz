using UnityEngine;

public class RewardInfoRepositoryImpl : RewardInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("RewardInfo");

    public RewardInfo Get()
    {
        return JsonUtility.FromJson<RewardInfo>(jsonFile.text);
    }
}