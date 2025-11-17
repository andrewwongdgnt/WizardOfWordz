using UnityEngine;

public class PlayerInfoRepositoryImpl : PlayerInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("PlayerInfo");

    public PlayerInfo Get()
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonFile.text);
    }
}