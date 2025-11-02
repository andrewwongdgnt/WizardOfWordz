using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MainScript : MonoBehaviour
{
    public WordReader wordReader;

    private readonly List<Key> monitoredKeys = new()
    {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J,
        Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T,
        Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
        Key.Enter, Key.Backspace
    };

    private readonly List<Key> allowedChars = new();

    private readonly Stack<char> currentWordStack = new();

    private List<Word> allWords;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allWords = wordReader.Get();
        allowedChars.Add(Key.A);
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
                ProcessCurrentWord();
                break;
            case Key.Backspace:
                if (currentWordStack.Any())
                    currentWordStack.Pop();
                break;
            default:
                char c = GetCharFromKey(key);
                currentWordStack.Push(c);
                break;
        }

        if (key != Key.Enter)
        {
            String word = GetCurrentWordStackAsString();
            Debug.Log(word);
        }
    }

    private string GetCurrentWordStackAsString()
    {
        return new(currentWordStack.Reverse().ToArray());
    }

    private void ProcessCurrentWord()
    {
        String word = GetCurrentWordStackAsString();
        currentWordStack.Clear();

        Word foundWord = allWords.Find(w => w.word.ToLower() == word.ToLower());

        Debug.Log(foundWord != null ? $"{word} is a word and it is {foundWord.tag}" : $"{word} is not a word");
    }

    private char GetCharFromKey(Key key)
    {
        if (key >= Key.A && key <= Key.Z)
        {
            // 'A' + (offset from Key.A)
            char baseChar = (char)('A' + (key - Key.A));
            return baseChar;
        }

        return '\0'; // null character if not A–Z
    }
}
