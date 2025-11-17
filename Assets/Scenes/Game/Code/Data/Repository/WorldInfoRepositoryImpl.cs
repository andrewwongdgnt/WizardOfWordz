using UnityEngine;

public class WorldInfoRepositoryImpl: WorldInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("Worlds");

    public WorldInfo Get()
    {
        return JsonUtility.FromJson<WorldInfo>(jsonFile.text);
    }
}