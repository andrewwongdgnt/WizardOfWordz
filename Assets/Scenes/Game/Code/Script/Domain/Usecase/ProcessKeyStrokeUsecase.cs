using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class ProcessKeyStrokeUsecase
{

    public char Invoke(
        Key key,
        List<char> allowedChars
        )
    {
        char c = GetCharFromKey(key);
        return c == '\0' || !allowedChars.Remove(c) ? '\0' : c;
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
