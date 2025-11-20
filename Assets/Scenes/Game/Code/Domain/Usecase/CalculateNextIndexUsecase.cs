using System.Collections.Generic;

public class CalculateNextIndexUsecase
{
    public int Invoke(
       bool preferRight,
       int currentIndex,
       int max
       )
    {
        int direction = preferRight ? 1 : -1;
        int newIndex = currentIndex + direction;
        if (newIndex < 0)
        {
            newIndex = max - 1;
        }
        else if (newIndex >= max)
        {
            newIndex = 0;
        }
        return newIndex;
    }
}