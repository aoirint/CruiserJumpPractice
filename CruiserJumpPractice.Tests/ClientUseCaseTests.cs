using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.UseCases;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Tests.Fakes;
using Xunit;

namespace CruiserJumpPractice.Tests;

public sealed class ClientUseCaseTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RequestSave_WhenExecuted_EnforcesHostAuthority(bool host)
    {
        var game = new FakeGameInterop { Host = host };
        var validation = new RecordingValidationLogger();
        game.OnSaveRequest = () => Assert.Equal("success", Assert.Single(validation.Records).Fields!["result"]);
        var useCase = new RequestSaveCruiserStateUseCase(game, validation);

        var result = useCase.Execute();

        Assert.Equal(host ? RequestSaveCruiserStateResult.Success : RequestSaveCruiserStateResult.HostOnly, result);
        Assert.Equal(host ? new[] { "IsHost", "RequestSaveCruiserState" } : new[] { "IsHost", "DisplayTip" }, game.Calls);
        if (host)
        {
            Assert.Empty(game.Tips);
        }
        else
        {
            Assert.Same(HudTipMessage.SaveHostOnly, Assert.Single(game.Tips));
        }
        var record = Assert.Single(validation.Records);
        Assert.Equal("request_save_result", record.EventName);
        Assert.Equal(host ? "host" : "client", record.Fields!["role"]);
        Assert.Equal(host ? "success" : "host_only", record.Fields["result"]);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RequestLoad_WhenExecuted_EnforcesHostAuthority(bool host)
    {
        var game = new FakeGameInterop { Host = host };
        var validation = new RecordingValidationLogger();
        game.OnLoadRequest = () => Assert.Equal("success", Assert.Single(validation.Records).Fields!["result"]);
        var useCase = new RequestLoadCruiserStateUseCase(game, validation);

        var result = useCase.Execute();

        Assert.Equal(host ? RequestLoadCruiserStateResult.Success : RequestLoadCruiserStateResult.HostOnly, result);
        Assert.Equal(host ? new[] { "IsHost", "RequestLoadCruiserState" } : new[] { "IsHost", "DisplayTip" }, game.Calls);
        if (host)
        {
            Assert.Empty(game.Tips);
        }
        else
        {
            Assert.Same(HudTipMessage.LoadHostOnly, Assert.Single(game.Tips));
        }
        var record = Assert.Single(validation.Records);
        Assert.Equal("request_load_result", record.EventName);
        Assert.Equal(host ? "host" : "client", record.Fields!["role"]);
        Assert.Equal(host ? "success" : "host_only", record.Fields["result"]);
    }

    [Fact]
    public void ToggleMagnet_WhenCallerIsNotHost_DoesNotReadOrToggleMagnet()
    {
        var game = new FakeGameInterop { Host = false };
        var validation = new RecordingValidationLogger();

        var result = new ToggleMagnetUseCase(game, validation).Execute();

        Assert.Equal(ToggleMagnetResult.HostOnly, result);
        Assert.Equal(new[] { "IsHost", "DisplayTip" }, game.Calls);
        Assert.Same(HudTipMessage.MagnetHostOnly, Assert.Single(game.Tips));
        var record = Assert.Single(validation.Records);
        Assert.Equal("toggle_magnet_result", record.EventName);
        Assert.Equal("client", record.Fields!["role"]);
        Assert.Equal("host_only", record.Fields["result"]);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ToggleMagnet_WhenHost_UsesExpectedStateBeforeSynchronization(bool beforeOn)
    {
        var game = new FakeGameInterop { MagnetOn = beforeOn };
        var validation = new RecordingValidationLogger();

        var result = new ToggleMagnetUseCase(game, validation).Execute();

        Assert.Equal(beforeOn ? ToggleMagnetResult.MagnetOff : ToggleMagnetResult.MagnetOn, result);
        Assert.Equal(new[] { "IsHost", "IsShipMagnetOn", "ToggleShipMagnet", "DisplayTip" }, game.Calls);
        Assert.Same(beforeOn ? HudTipMessage.MagnetOff : HudTipMessage.MagnetOn, Assert.Single(game.Tips));
        Assert.Collection(validation.Records,
            record =>
            {
                Assert.Equal("magnet_toggle", record.EventName);
                Assert.Equal(beforeOn ? "on" : "off", record.Fields!["before"]);
                Assert.Equal(beforeOn ? "off" : "on", record.Fields["expected_after"]);
                Assert.Equal("unknown", record.Fields["observed_after"]);
            },
            record =>
            {
                Assert.Equal("toggle_magnet_result", record.EventName);
                Assert.Equal(beforeOn ? "magnet_off" : "magnet_on", record.Fields!["result"]);
            });
    }
}
