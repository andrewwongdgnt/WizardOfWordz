using UnityEngine;

public class LevelInfoRepositoryImpl : LevelInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("Levels");

    public LevelInfo Get()
    {
        return JsonUtility.FromJson<LevelInfo>(jsonFile.text);
    }
}