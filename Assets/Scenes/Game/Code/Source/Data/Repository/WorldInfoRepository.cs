using UnityEngine;

public class WorldInfoRepository: IWorldInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("Worlds");

    public WorldInfo Get()
    {
        return JsonUtility.FromJson<WorldInfo>(jsonFile.text);
    }
}