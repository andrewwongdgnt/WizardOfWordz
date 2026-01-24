using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Enemy;

public class GetNextEnemyMoveUsecaseTest
{
    private GetNextEnemyMoveUsecase sut;

    public static GetNextEnemyMoveUsecase GenerateMock()
    {
        return GenerateMock(_ => { });
    }

    public static GetNextEnemyMoveUsecase GenerateMock(Action<GetNextEnemyMoveUsecase> action)
    {
        GetNextEnemyMoveUsecase mock = Substitute.For<GetNextEnemyMoveUsecase>();
        action(mock);
        return mock;
    }
}
