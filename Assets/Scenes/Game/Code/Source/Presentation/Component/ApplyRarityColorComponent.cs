using UnityEngine;
using UnityEngine.UI;

public class ApplyRarityColorComponent : MonoBehaviour
{

    public Image image; 
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
