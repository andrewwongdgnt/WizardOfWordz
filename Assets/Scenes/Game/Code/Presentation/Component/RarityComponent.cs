using UnityEngine;
using UnityEngine.UI;

public class RarityComponent : MonoBehaviour
{

    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Apply(RarityEnum rarityEnum)
    {
        image.color = rarityEnum switch
        {
            RarityEnum.Common => Color.white,
            RarityEnum.Uncommon => Color.green,
            RarityEnum.Rare => Color.deepSkyBlue,
            RarityEnum.Epic => Color.purple,
            RarityEnum.Legendary => Color.orange,
            _ => Color.white
        };
    }
}
