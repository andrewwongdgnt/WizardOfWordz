
public class LetterEntity
{

    public char Value { get;  }
    public int Count { get;  }
    public int Score { get;  }

    public LetterEntity(
        char value,
        int count,
        int score
        )
    {
        Value = value;
        Count = count;
        Score = score;
    }
}
