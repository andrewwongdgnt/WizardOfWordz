using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MainScript : MonoBehaviour
{
    public List<EnemyArg> enemyArgs;

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

    private readonly List<Key> monitoredKeys = new()
    {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J,
        Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T,
        Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
        Key.Enter, Key.Backspace
    };

    private List<Tile> allowedTiles;
    private List<Enemy> enemies;

    private readonly Stack<char> currentWordStack = new();

    private Dictionary<string, Word> dictionary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dictionary = retrieveWordsFromDictionaryUsecase.Invoke();
        Restart();
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
    }

    private void AddToCurrentWord(Key key)
    {
        switch (key)
        {
            case Key.Enter:
                string word = GetCurrentWordStackAsString();
                currentWordStack.Clear();
                processWordUsecase.Invoke(word, dictionary);
                Restart();
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

        if (key != Key.Enter)
        {
            string word = GetCurrentWordStackAsString();
            Debug.Log($"{string.Join(",", allowedTiles)} == {word}");
        }
    }

    private string GetCurrentWordStackAsString()
    {
        return new(currentWordStack.Reverse().ToArray());
    }

    private void Restart()
    {
        PopulateEnemies();
        RestartAllowedTiles();
    }

    private void PopulateEnemies()
    {
        enemies = populateEnemiesUsecase.Invoke(enemyArgs);
    }

    private void RestartAllowedTiles()
    {
        allowedTiles = generateCharTilesUsecase.Invoke();

        Debug.Log($"Available tiles: {string.Join(",", allowedTiles)}");
    }
}
