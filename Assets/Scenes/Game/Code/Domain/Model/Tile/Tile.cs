public class Tile
{
    public char Value { get; }
    public int Score {get;}
    public bool pickable;

    public Tile(
        char value,
        int score
        )
    {
        Value = value;
        Score = score;
        pickable = true;
    }

    public Tile Clone()
    {
        return new Tile(
            Value,
            Score
            );
    }

    public override string ToString()
    {
        string valueAsString = Value.ToString();
        string valueWithVisualIndicator = pickable ? valueAsString.ToUpper() : valueAsString.ToLower();
        return valueWithVisualIndicator;
    }
}
