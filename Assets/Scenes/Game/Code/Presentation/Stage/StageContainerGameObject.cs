using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageContainerGameObject : MonoBehaviour
{

    public GameObject enemyContainer;
    public EnemyGameObject enemyGO;

    public Sprite noteBaseSprite;
    public Sprite noteRarityElementSprite;

    public Sprite noteBookBaseSprite;
    public Sprite noteBookRarityElementSprite;

    public Action<Enemy> enemySelectedAction;
    public Action<Enemy> enemyHoverAction;

    private readonly Dictionary<Enemy, EnemyGameObject> enemyMap = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateState(Enemy enemyThatIsTargeted)
    {
        foreach (var enemyGo in enemyMap.Values)
        {
            enemyGo.UpdateState(enemyThatIsTargeted);
        }
    }
    public void SetUp(List<Enemy> enemies)
    {
        ClearEnemies();

        enemies.ForEach(enemy =>
            {
                (Sprite, Sprite) spritePair = enemy.EnemyEnum switch
                {
                    EnemyEnum.Note => (noteBaseSprite, noteRarityElementSprite),
                    EnemyEnum.Notebook => (noteBookBaseSprite, noteBookRarityElementSprite),
                    _ => throw new NotImplementedException(),
                };

                EnemyGameObject newEnemyGO = Instantiate(enemyGO, enemyContainer.transform.position, Quaternion.identity);
                newEnemyGO.transform.SetParent(enemyContainer.transform);
                newEnemyGO.Init(enemy, spritePair.Item1, spritePair.Item2);
                enemyMap[enemy] = newEnemyGO;
                newEnemyGO.enemySelectedAction = enemySelectedAction;
                newEnemyGO.enemyHoverAction = enemyHoverAction;

                AdjustPosition(newEnemyGO.GetComponent<RectTransform>());
            }
        );
    }

    public void ClearEverything()
    {
        ClearEnemies();
    }

    private void ClearEnemies()
    {
        foreach (var enemyItem in enemyMap)
        {
            Destroy(enemyItem.Value.gameObject);
        }
        enemyMap.Clear();
    }

    private void AdjustPosition(RectTransform rect)
    {
        rect.localPosition = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector2.zero;
    }
}
