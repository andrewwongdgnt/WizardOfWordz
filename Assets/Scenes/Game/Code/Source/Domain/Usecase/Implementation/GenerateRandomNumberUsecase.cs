using System;

public class GenerateRandomNumberUsecase : IGenerateRandomNumberUsecase
{
    private readonly Random random = new();

    public int Invoke(int? maxValue = null)
    {
        return maxValue != null ? random.Next(maxValue ?? 0) : random.Next();
    }
}