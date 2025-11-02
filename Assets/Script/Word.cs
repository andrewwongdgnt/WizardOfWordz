using UnityEngine;

[System.Serializable]
public class Word
{
    public string word;
    public string tag;

    public Word(string word, string tag)
    {
        this.word = word;
        this.tag = tag;
    }
}
