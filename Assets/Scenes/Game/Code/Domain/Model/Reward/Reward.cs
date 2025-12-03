using System.Collections.Generic;

public class Reward
{
    public string Title { get; }
    public string Description { get; }
    public RarityEnum RarityEnum { get; }
    private List<int> values;
    private int currentValueIndex;

    public Reward(
            RarityEnum rarityEnum,
            string title,
            string description,
            List<int> values
        )
    {
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

    public (int, int) CurrentAndFutureValuePair()
    {
        return (Value(currentValueIndex), Value(currentValueIndex + 1));
    }

    private int Value(int index)
    {
        if (index >= 0 && index < values.Count)
        {
            return values[index];
        }
        return 0;
    }

    public override string ToString()
    {
        (int,int) values = CurrentAndFutureValuePair();
        return $"{Title}{values.Item1}->{values.Item2}";
    }
}