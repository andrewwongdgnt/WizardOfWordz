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

    [Inject]
    private readonly RetrieveWordsFromDictionaryUsecase retrieveWordsFromDictionaryUsecase;

    [Inject]
    private readonly PickTileUsecase pickTileUsecase;

    [Inject]
    private readonly PopulateEnemiesUsecase populateEnemiesUsecase;

    [Inject]
    private readonly ReturnTileUsecase returnTileUsecase;

    [Inject]
    private readonly ProcessWordUsecase processWordUsecase;

    [Inject]
    private readonly GenerateCharTilesUsecase generateCharTilesUsecase;

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

    private readonly Stack<char> currentWordStack = new();

    private Dictionary<string, Word> dictionary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dictionary = retrieveWordsFromDictionaryUsecase.Invoke(); 
        PopulateEnemies();
        RestartAllowedTiles();
        LogState();
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
        switch (key)
        {
            case Key.Enter:
                string word = GetCurrentWordStackAsString();
                currentWordStack.Clear();
                processWordUsecase.Invoke(
                    word,
                    dictionary,
                    enemies,
                    attackIndex
                    );
                RestartAllowedTiles();
                break;
            case Key.Backspace:
                if (currentWordStack.Any())
                {
                    char removedChar = currentWordStack.Pop();
                    returnTileUsecase.Invoke(removedChar, allowedTiles);
                }
                break;
            default:
                char c = pickTileUsecase.Invoke(key, allowedTiles);
                if (c != '\0')
                    currentWordStack.Push(c);
                break;
        }

        LogState();
    }

    private void TargetNewEnemy(Key key)
    {
        if (key == Key.LeftArrow)
        {
            attackIndex--;
        }
        else if (key == Key.RightArrow)
        {
            attackIndex++;
        }
        if (attackIndex < 0)
        {
            attackIndex = enemies.Count - 1;
        }
        else if (attackIndex >= enemies.Count)
        {
            attackIndex = 0;
        }
        LogState();
    }

    private void LogState()
    {
        string word = GetCurrentWordStackAsString();
        Debug.Log($"attackIndex: {attackIndex}\n{string.Join(" - ", enemies)}\n{string.Join("", allowedTiles)}\n{word}");
    }

    private string GetCurrentWordStackAsString()
    {
        return new(currentWordStack.Reverse().ToArray());
    }

    private void PopulateEnemies()
    {
        enemies = populateEnemiesUsecase.Invoke(enemyArgs);
    }

    private void RestartAllowedTiles()
    {
        allowedTiles = generateCharTilesUsecase.Invoke();
    }
}
