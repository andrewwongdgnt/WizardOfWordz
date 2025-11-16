using ModestTree;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MainScript : MonoBehaviour
{
    public List<EnemyArg> enemyArgs;
    public int attackIndex;
    public BoardContainerGameObject boardContainerGO;
    public StageContainerGameObject stageContainerGO;
    public PlayerStatsContainerGameObject playerStatsContainerGameObject;

    [Inject]
    private readonly RetrieveWordsFromDictionaryUsecase retrieveWordsFromDictionaryUsecase;

    [Inject]
    private readonly PickTileUsecase pickTileUsecase;

    [Inject]
    private readonly PopulateEnemiesUsecase populateEnemiesUsecase;

    [Inject]
    private readonly ProcessWordUsecase processWordUsecase;

    [Inject]
    private readonly GenerateCharTilesUsecase generateCharTilesUsecase;

    [Inject]
    private readonly GetNextTargetUsecase getNextTargetUsecase;

    [Inject]
    private readonly CalculateTurnFromEnemiesUsecase calculateTurnFromEnemiesUsecase;

    [Inject]
    private readonly PlayerManager playerManager;

    private readonly ISet<Key> monitoredKeys = new HashSet<Key>()
    {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J,
        Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T,
        Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
        Key.Enter, Key.Backspace,
    };

    private readonly ISet<Key> movementKeys = new HashSet<Key>()
    {
        Key.LeftArrow, Key.RightArrow
    };

    private List<Tile> allowedTiles;
    private List<Enemy> enemies;

    private readonly List<Tile> currentWordList = new();

    private Dictionary<string, Word> dictionary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager.Init();
        dictionary = retrieveWordsFromDictionaryUsecase.Invoke();
        SetUp();
        UpdateUIState();
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;


        foreach (var key in monitoredKeys)
        {
            if (keyboard[key]?.wasPressedThisFrame == true)
            {
                AddToCurrentWord(key);
            }
        }
        foreach (var key in movementKeys)
        {
            if (keyboard[key]?.wasPressedThisFrame == true)
            {
                TargetNewEnemy(key);
            }
        }
    }

    private void AddToCurrentWord(Key key)
    {
        Tile tileThatChanged = null;
        switch (key)
        {
            case Key.Enter:
                bool valid = ProcessWord();
                if (!valid)
                {
                    return;
                }
                break;
            case Key.Backspace:
                if (currentWordList.Any())
                {
                    tileThatChanged = currentWordList[^1];
                    currentWordList.RemoveAt(currentWordList.Count - 1);
                    tileThatChanged.pickable = true;
                }
                break;
            default:
                tileThatChanged = pickTileUsecase.Invoke(key, allowedTiles);
                if (tileThatChanged != null)
                {
                    currentWordList.Add(tileThatChanged);
                }
                break;
        }

        UpdateUIState(tileThatChanged: tileThatChanged);
    }

    private bool ProcessWord()
    {
        string word = GetCurrentWordListAsString();
        if (word.IsEmpty())
        {
            return false;
        }
        currentWordList.Clear();
        processWordUsecase.Invoke(
            word,
            dictionary,
            enemies,
            attackIndex
            );
        if (enemies[attackIndex].IsDead())
        {
            attackIndex = getNextTargetUsecase.Invoke(
               true,
               attackIndex,
               enemies
               );
        }
        List<(int enemyIndex, Enemy.Move move)> movesPair = calculateTurnFromEnemiesUsecase.Invoke(
            enemies
            );
        if (!movesPair.IsEmpty())
        {
            string attackLog = string.Join(",", movesPair.Select(mp => $"{enemies[mp.enemyIndex].ShortLabel()} at {mp.enemyIndex} does {mp.move}"));
            Debug.Log(attackLog);
            movesPair.ForEach(mp =>
            {
                if (mp.move is Enemy.Move.Attack attack)
                {
                    playerManager.UpdateHealthBy(-attack.Damage);
                }
            });
        }

        RestartAllowedTiles();
        return true;
    }

    private void TargetNewEnemy(Key key)
    {
        attackIndex = getNextTargetUsecase.Invoke(
            key == Key.RightArrow,
            attackIndex,
            enemies
            );
        UpdateUIState();
    }

    private void UpdateUIState(Tile tileThatChanged = null)
    {
        boardContainerGO.UpdateState(currentWordList, tileThatChanged);
        stageContainerGO.UpdateState(enemies[attackIndex]);
        playerStatsContainerGameObject.UpdateState();


        string word = GetCurrentWordListAsString();
        Debug.Log($"{playerManager.CurrentHealth}hp & Targeting: {attackIndex}\n{string.Join(" - ", enemies)}\n{string.Join("", allowedTiles)}\n{word}");
    }

    private string GetCurrentWordListAsString()
    {
        return new(currentWordList.Select(t => t.Value).ToArray());
    }

    private void SetUp()
    {
        boardContainerGO.tileAction = TileAction;
        boardContainerGO.tileInWordAction = TileInWordAction;
        stageContainerGO.enemySelectedAction = EnemySelectedAction;
        stageContainerGO.enemyHoverAction = EnemyHoverAction;

        playerStatsContainerGameObject.SetUp(playerManager);
        PopulateEnemies();
        RestartAllowedTiles();
    }

    private void PopulateEnemies()
    {
        enemies = populateEnemiesUsecase.Invoke(enemyArgs);
        stageContainerGO.SetUp(enemies);
    }

    private void RestartAllowedTiles()
    {
        allowedTiles = generateCharTilesUsecase.Invoke();
        boardContainerGO.SetUpTiles(allowedTiles);
    }

    private void TileAction(Tile tile)
    {
        bool exists = pickTileUsecase.Invoke(tile, allowedTiles);
        if (exists)
        {
            currentWordList.Add(tile);
        }
        UpdateUIState(tileThatChanged: tile);
    }

    private void TileInWordAction(Tile tile)
    {
        currentWordList.Remove(tile);
        tile.pickable = true;
        UpdateUIState(tileThatChanged: tile);
    }

    private void EnemySelectedAction(Enemy enemy)
    {
        int originalIndex = attackIndex;
        TargetNewEnemy(enemy);
        bool valid = ProcessWord();
        if (!valid)
        {
            attackIndex = originalIndex;
            return;
        }
        UpdateUIState();

    }

    private void EnemyHoverAction(Enemy enemy)
    {
        int originalIndex = attackIndex;
        TargetNewEnemy(enemy);
        if (originalIndex != attackIndex)
        {
            UpdateUIState();
        }
    }

    private void TargetNewEnemy(Enemy enemy)
    {
        int index = enemies.IndexOf(enemy);
        if (index < 0)
        {
            return;
        }
        attackIndex = index;
    }
}
