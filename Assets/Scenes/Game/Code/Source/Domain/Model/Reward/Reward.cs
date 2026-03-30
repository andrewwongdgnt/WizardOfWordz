using System.Collections.Generic;

public class Reward
{
    public string Title { get; }
    public string Description { get; }
    public RewardEnum RewardEnum { get; }
    private readonly List<RewardValue> values;
    private int currentValueIndex;

    public Reward(
            RewardEnum rewardEnum,
            string title,
            string description,
            List<RewardValue> values
        )
    {
        RewardEnum = rewardEnum;
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
        return currentValueIndex < values.Count - 1;
    }

    public RewardValue GetCurrentValue()
    {
        return Value(currentValueIndex);
    }

    public RewardValue GetFutureValue()
    {
        return Value(currentValueIndex + 1);
    }

    private RewardValue Value(int index)
    {
        if (index >= 0 && index < values.Count)
        {
            return values[index];
        }
        return new(0, RarityEnum.Common);
    }

    public class RewardValue : IRarityHolder
    {
        public int Value { get; }
        public RarityEnum RarityEnum { get; }

        public RewardValue(
            int value,
            RarityEnum rarityEnum
            )
        {
            Value = value;
            RarityEnum = rarityEnum;
        }

    }
}