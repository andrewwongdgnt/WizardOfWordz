public class MockWordUtil
{
    public static Word GenerateWord(
        string value = "",
        string tag = ""
        )
    {
        return new Word(
            value: value,
            tag: tag
            );
    }
}