
public class WordEntity
{

    public string Value { get; }
    public int Index { get; }
    public string Tag { get; }

    public WordEntity(
        string value,
        int index,
        string tag
        )
    {
        Value = value;
        Index = index;
        Tag = tag;
    }
}
