using UnityEngine;


public class Word
{
    public string Value { get; }
    public string Tag { get; }

    public Word(string value, string tag)
    {
        Value = value;
        Tag = tag;
    }
}
