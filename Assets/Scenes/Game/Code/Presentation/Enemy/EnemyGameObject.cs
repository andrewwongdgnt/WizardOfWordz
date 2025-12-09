using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGameObject : MonoBehaviour
{

    public Image baseImage;
    public RarityComponent rarityComponent;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI turnsRemainingText;
    public TextMeshProUGUI selectIndicatorText;

    public Action<Enemy> enemySelectedAction;
    public Action<Enemy> enemyHoverAction;

    private Enemy enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSelect()
    {
        ApplyAction(enemySelectedAction);
    }

    public void OnHover()
    {
        ApplyAction(enemyHoverAction);
    }

    public void UpdateState(Enemy enemyThatIsTargeted)
    {
        if (enemy == null)
        {
            return;
        }
        UpdateHealth(enemy);
        UpdateTurnsRemaining(enemy);
        UpdateSelectIndicator(enemyThatIsTargeted == enemy);
    }

    public void Init(
        Enemy enemyModel,
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        enemy = enemyModel;
        rarityComponent.Apply(enemyModel.RarityEnum);
        ApplySprite(baseSprite, rarityElementSprite);
        ApplySize(baseSprite, rarityElementSprite);
        UpdateHealth(enemyModel);
        UpdateTurnsRemaining(enemyModel);
    }

    private void ApplySprite(
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        baseImage.sprite = baseSprite;
        rarityComponent.image.sprite = rarityElementSprite;
    }

    private void ApplySize(
        Sprite baseSprite,
        Sprite rarityElementSprite
        )
    {
        var vector = new Vector2(baseSprite.rect.width, baseSprite.rect.height);
        GetComponent<RectTransform>().sizeDelta = vector;
        baseImage.rectTransform.sizeDelta = vector;
        rarityComponent.image.rectTransform.sizeDelta = new Vector2(rarityElementSprite.rect.width, rarityElementSprite.rect.height);
    }

    private void ApplyAction(Action<Enemy> action)
    {
        if (enemy != null && !enemy.IsDead())
        {
            action(enemy);
        }
    }

    private void UpdateHealth(Enemy enemyModel)
    {
        healthText.text = $"{enemyModel.CurrentHealth}/{enemyModel.MaxHealth}";
    }

    private void UpdateTurnsRemaining(Enemy enemyModel)
    {
        turnsRemainingText.text = $"-{enemyModel.TurnsRemaining}-";
    }

    private void UpdateSelectIndicator(bool isSelected)
    {
        selectIndicatorText.text = isSelected ? "V" : "";
    }
}