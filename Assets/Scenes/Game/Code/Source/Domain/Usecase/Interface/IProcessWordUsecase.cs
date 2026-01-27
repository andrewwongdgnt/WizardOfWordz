using System.Collections.Generic;

public interface IProcessWordUsecase
{
    public void Invoke(
        string word,
        Dictionary<string, Word> dictionary,
        List<Enemy> enemies,
        int attackIndex
        );
}