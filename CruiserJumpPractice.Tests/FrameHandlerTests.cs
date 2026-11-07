using System.Linq;
using CruiserJumpPractice.Core.Handlers;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Tests.Fakes;
using Xunit;

namespace CruiserJumpPractice.Tests;

public sealed class FrameHandlerTests
{
    private static FrameHandler Handler(FakeGameInterop game, FakePracticeInput input, RecordingValidationLogger validation) => new(
        game, input, validation,
        new RequestSaveCruiserStateUseCase(game, validation),
        new RequestLoadCruiserStateUseCase(game, validation),
        new ToggleMagnetUseCase(game, validation));

    [Theory]
    [InlineData(true, false, false, "menu")]
    [InlineData(false, true, false, "terminal")]
    [InlineData(false, false, true, "chat")]
    [InlineData(true, true, false, "multiple")]
    [InlineData(true, false, true, "multiple")]
    [InlineData(false, true, true, "multiple")]
    [InlineData(true, true, true, "multiple")]
    public void HandleFrame_WhenBusy_SuppressesEveryRequestedAction(bool menu, bool terminal, bool chat, string reason)
    {
        var game = new FakeGameInterop { BusyState = new LocalPlayerBusyState(menu, terminal, chat) };
        var input = new FakePracticeInput { Save = true, Load = true, Toggle = true };
        var validation = new RecordingValidationLogger();

        Handler(game, input, validation).HandleFrame();

        Assert.DoesNotContain("RequestSaveCruiserState", game.Calls);
        Assert.DoesNotContain("RequestLoadCruiserState", game.Calls);
        Assert.DoesNotContain("IsShipMagnetOn", game.Calls);
        Assert.DoesNotContain("ToggleShipMagnet", game.Calls);
        Assert.Empty(game.Tips);
        Assert.Equal(new[] { "save", "load", "toggle_magnet" }, validation.Records.Select(record => record.Fields!["action"]));
        Assert.All(validation.Records, record =>
        {
            Assert.Equal("input_suppressed", record.EventName);
            Assert.Equal(reason, record.Fields!["reason"]);
        });
        Assert.Equal(1, input.SaveReads);
        Assert.Equal(1, input.LoadReads);
        Assert.Equal(1, input.ToggleReads);
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    [InlineData(true, true, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, true)]
    public void HandleFrame_WhenIdle_DispatchesOnlyTriggeredActionsOnce(bool save, bool load, bool toggle)
    {
        var game = new FakeGameInterop();
        var input = new FakePracticeInput { Save = save, Load = load, Toggle = toggle };
        var validation = new RecordingValidationLogger();

        Handler(game, input, validation).HandleFrame();

        Assert.Equal(save ? 1 : 0, game.Calls.Count(call => call == "RequestSaveCruiserState"));
        Assert.Equal(load ? 1 : 0, game.Calls.Count(call => call == "RequestLoadCruiserState"));
        Assert.Equal(toggle ? 1 : 0, game.Calls.Count(call => call == "ToggleShipMagnet"));
        Assert.Equal(save || load || toggle ? 1 : 0, game.Calls.Count(call => call == "GetLocalPlayerBusyState"));
        Assert.Equal((save ? 1 : 0) + (load ? 1 : 0) + (toggle ? 1 : 0),
            validation.Records.Count(record => record.EventName == "input_triggered"));
        Assert.DoesNotContain(validation.Records, record => record.EventName == "input_suppressed");
        Assert.Equal(1, input.SaveReads);
        Assert.Equal(1, input.LoadReads);
        Assert.Equal(1, input.ToggleReads);
        if (!save && !load && !toggle)
        {
            Assert.Empty(game.Calls);
            Assert.Empty(validation.Records);
        }
    }
}
