public class TileInfo
{
    public char Value { get; }
    public int Count { get; }
    public int Score { get; }
    public bool pickable;

    public TileInfo(
        char value,
        int count,
        int score
        )
    {
        Value = value;
        Count = count;
        Score = score;
        pickable = true;
    }
}
