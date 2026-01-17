using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class SelectLevelChoicesUseCase
{
    private readonly Random random = new();

    internal List<Level> Invoke(int levelChoiceIndex, List<World.LevelChoice> levelChoices)
    {

        if (levelChoiceIndex <0 || levelChoiceIndex >= levelChoices.Count)
        {
            return new();
        }

        World.LevelChoice levelChoice = levelChoices[levelChoiceIndex];

        List<Level> copy = new(levelChoice.Choices);

        // Fisher–Yates shuffle
        for (int i = copy.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (copy[i], copy[j]) = (copy[j], copy[i]); // swap
        }

        return copy.Take(levelChoice.Pick).ToList();

    }
}