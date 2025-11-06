using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoRepositoryImpl : EnemyInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("EnemyInfo");

    public EnemyInfo Get()
    {
        return JsonUtility.FromJson<EnemyInfo>(jsonFile.text);
    }
}
