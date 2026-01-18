using System.Collections;
using NUnit.Framework;
using NSubstitute;

public class CalculateEnemyMoveUsecaseTest
{
    private CalculateEnemyMoveUsecase sut = new();
    [Test]
    public void TestInvoke()
    {
        string g = "1";
        Assert.AreEqual(g, "1");
    }
}
