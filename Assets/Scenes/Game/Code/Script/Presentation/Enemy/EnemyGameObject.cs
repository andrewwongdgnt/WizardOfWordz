using UnityEngine;
using UnityEngine.UI;

public class EnemyGameObject : MonoBehaviour
{

    public Image baseImage;
    public Image rarityElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(
        Enemy enemyModel,
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        ApplyRarity(enemyModel);
        ApplySprite(baseSprite, rarityElementSprite);
        ApplySize(baseSprite, rarityElementSprite);
    }

    private void ApplyRarity(Enemy enemyModel)
    {
        rarityElement.color = enemyModel.RarityEnum switch
        {
            RarityEnum.Common => Color.white,
            RarityEnum.Uncommon => Color.green,
            RarityEnum.Rare => Color.deepSkyBlue,
            RarityEnum.Epic => Color.purple,
            RarityEnum.Legendary => Color.orange,
            _ => Color.white
        };
    }

    private void ApplySprite(
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        baseImage.sprite = baseSprite;
        rarityElement.sprite = rarityElementSprite;
    }

    private void ApplySize(
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        var vector = new Vector2(baseSprite.rect.width, baseSprite.rect.height);
        GetComponent<RectTransform>().sizeDelta = vector;
        baseImage.rectTransform.sizeDelta = vector;
        rarityElement.rectTransform.sizeDelta = new Vector2(rarityElementSprite.rect.width, rarityElementSprite.rect.height);
    }
}