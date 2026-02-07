using System.Collections.Generic;

public interface IProcessWordUsecase
{
    public void Invoke(
        string word,
        List<Enemy> enemies,
        int attackIndex
        );
}