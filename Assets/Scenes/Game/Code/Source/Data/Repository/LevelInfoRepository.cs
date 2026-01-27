using UnityEngine;

public class LevelInfoRepository : ILevelInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("Levels");

    public LevelInfo Get()
    {
        return JsonUtility.FromJson<LevelInfo>(jsonFile.text);
    }
}