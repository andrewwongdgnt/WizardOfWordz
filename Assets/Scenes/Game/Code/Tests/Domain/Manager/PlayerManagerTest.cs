using NSubstitute;
using System;

public class PlayerManagerTest
{
    public static PlayerManager GenerateMock()
    {
        return GenerateMock(_ => { });
    }

    public static PlayerManager GenerateMock(Action<PlayerManager> action)
    {
        PlayerManager mock = Substitute.For<PlayerManager>(
             Substitute.For<PlayerInfoRepository>()
             );
        action(mock);
        return mock;
    }
}