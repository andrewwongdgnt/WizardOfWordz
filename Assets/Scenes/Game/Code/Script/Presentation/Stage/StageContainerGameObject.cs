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

    private readonly Dictionary<Enemy, EnemyGameObject> enemyMap = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateState(List<Enemy> enemies)
    {
        foreach (var enemyItem in enemyMap)
        {
            Destroy(enemyItem.Value.gameObject);
        }
        enemyMap.Clear();

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

                AdjustPosition(newEnemyGO.GetComponent<RectTransform>());
            }
        );
    }

    private void AdjustPosition(RectTransform rect)
    {
        rect.localPosition = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector2.zero;
    }
}
