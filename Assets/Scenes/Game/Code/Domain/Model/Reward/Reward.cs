using System.Collections.Generic;

public class Reward
{
    public string Title { get; }
    public string Description { get; }
    public RarityEnum RarityEnum { get; }
    public RewardEnum RewardEnum { get; }
    private readonly List<int> values;
    private int currentValueIndex;

    public Reward(
            RewardEnum rewardEnum,
            RarityEnum rarityEnum,
            string title,
            string description,
            List<int> values
        )
    {
        RewardEnum = rewardEnum;
        RarityEnum = rarityEnum;
        Title = title;
        Description = description;
        this.values = values;
        currentValueIndex = -1;
    }

    public void Pick()
    {
        currentValueIndex++;
    }

    public bool Pickable()
    {
       return currentValueIndex < values.Count;
    }

    public int GetFutureValue()
    {
        return Value(currentValueIndex + 1);
    }

    private int Value(int index)
    {
        if (index >= 0 && index < values.Count)
        {
            return values[index];
        }
        return 0;
    }
}