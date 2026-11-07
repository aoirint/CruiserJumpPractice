using CruiserJumpPractice.Core.UseCases;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Tests.Fakes;
using Xunit;

namespace CruiserJumpPractice.Tests;

public sealed class PresentationUseCaseTests
{
    [Theory]
    [InlineData((int)SaveCruiserStateResult.Success, "save_success")]
    [InlineData((int)SaveCruiserStateResult.NoCruiserFound, "save_no_cruiser")]
    [InlineData((int)SaveCruiserStateResult.UnexpectedState, null)]
    [InlineData(999, null)]
    public void PresentSave_WhenResultReceived_SelectsTipOrLogsDiagnostic(int value, string? tipToken)
    {
        var game = new FakeGameInterop();
        var logger = new RecordingPluginLogger();

        new PresentSaveCruiserStateResultUseCase(game, logger).Execute((SaveCruiserStateResult)value);

        if (tipToken != null)
        {
            Assert.Equal(tipToken, Assert.Single(game.Tips).Token);
            Assert.Empty(logger.Errors);
        }
        else
        {
            Assert.Empty(game.Tips);
            Assert.Contains($"Unknown SaveCruiserStateResult: {(SaveCruiserStateResult)value}", Assert.Single(logger.Errors));
        }
    }

    [Theory]
    [InlineData((int)LoadCruiserStateResult.Success, "load_success")]
    [InlineData((int)LoadCruiserStateResult.NoCruiserFound, "load_no_cruiser")]
    [InlineData((int)LoadCruiserStateResult.NoSavedState, "load_no_saved_state")]
    [InlineData((int)LoadCruiserStateResult.MagnetedToShip, "load_magneted_to_ship")]
    [InlineData((int)LoadCruiserStateResult.UnexpectedState, null)]
    [InlineData(999, null)]
    public void PresentLoad_WhenResultReceived_SelectsTipOrLogsDiagnostic(int value, string? tipToken)
    {
        var game = new FakeGameInterop();
        var logger = new RecordingPluginLogger();

        new PresentLoadCruiserStateResultUseCase(game, logger).Execute((LoadCruiserStateResult)value);

        if (tipToken != null)
        {
            Assert.Equal(tipToken, Assert.Single(game.Tips).Token);
            Assert.Empty(logger.Errors);
        }
        else
        {
            Assert.Empty(game.Tips);
            Assert.Contains($"Unknown LoadCruiserStateResult: {(LoadCruiserStateResult)value}", Assert.Single(logger.Errors));
        }
    }
}
