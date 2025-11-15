using UnityEngine;
using UnityEngine.UI;

public class EnemyGameObject : MonoBehaviour
{

    public Image rarityElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Enemy enemyModel)
    {
        ApplyRarity(enemyModel);
    }

    private void ApplyRarity(Enemy enemyModel)
    {
        Color color = enemyModel.RarityEnum switch
        {
            RarityEnum.Common => Color.white,
            RarityEnum.Uncommon => Color.green,
            RarityEnum.Rare => Color.deepSkyBlue,
            RarityEnum.Epic => Color.purple,
            RarityEnum.Legendary => Color.orange,
            _ => Color.white
        };
        rarityElement.color = color;
    }
}