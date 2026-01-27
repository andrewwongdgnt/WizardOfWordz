using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoRepository : IEnemyInfoRepository
{
    private readonly TextAsset jsonFile = Resources.Load<TextAsset>("EnemyInfo");

    public EnemyInfo Get()
    {
        return JsonUtility.FromJson<EnemyInfo>(jsonFile.text);
    }
}
