using UnityEngine;

public class PlayerInfoRepository : IPlayerInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("PlayerInfo");

    public PlayerInfo Get()
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonFile.text);
    }
}