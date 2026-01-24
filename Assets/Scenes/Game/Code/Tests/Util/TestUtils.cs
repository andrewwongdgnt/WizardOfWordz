using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

public class TestUtils
{
    public static void ClearReceivedCalls<T>(IEnumerable<T> objects)
        where T : class
    {
        foreach (var o in objects)
        {
            o.ClearReceivedCalls();
        }
    }
}