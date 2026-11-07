using System.Linq;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases;
using CruiserJumpPractice.Core.UseCases.Server;
using CruiserJumpPractice.Tests.Fakes;
using Xunit;

namespace CruiserJumpPractice.Tests;

public sealed class ServerUseCaseTests
{
    private static CruiserSnapshot Snapshot(int hp) => new(
        new Vector3Value(1, 2, 3), new Vector3Value(11, 22, 33), 0.25f, 1200, hp, 4);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Save_WhenNoCruiser_PreservesPreviousState(bool hasSavedState)
    {
        var previous = hasSavedState ? Snapshot(7) : null;
        var store = new CruiserStateStore { SavedCruiserState = previous };
        var game = new FakeGameInterop();
        var logger = new RecordingPluginLogger();
        var validation = new RecordingValidationLogger();

        var result = new SaveCruiserStateUseCase(game, store, logger, validation).Execute();

        Assert.Equal(SaveCruiserStateResult.NoCruiserFound, result);
        Assert.Same(previous, store.SavedCruiserState);
        Assert.Equal(new[] { "CaptureCruiser" }, game.Calls);
        Assert.Equal("no_cruiser_found", Assert.Single(validation.Records).Fields!["result"]);
        Assert.Empty(logger.Errors);
    }

    [Fact]
    public void Save_WhenCaptureSucceeds_ReplacesSnapshot()
    {
        var captured = Snapshot(31);
        var game = new FakeGameInterop { CapturedSnapshot = captured };
        var store = new CruiserStateStore { SavedCruiserState = Snapshot(7) };
        var logger = new RecordingPluginLogger();
        var validation = new RecordingValidationLogger();

        var result = new SaveCruiserStateUseCase(game, store, logger, validation).Execute();

        Assert.Equal(SaveCruiserStateResult.Success, result);
        Assert.Same(captured, store.SavedCruiserState);
        Assert.Equal(new[] { "CaptureCruiser" }, game.Calls);
        var record = Assert.Single(validation.Records);
        Assert.Equal("save_result", record.EventName);
        Assert.Equal("success", record.Fields!["result"]);
        Assert.Equal(31, record.Fields["hp"]);
        Assert.Equal(4, record.Fields["turbo"]);
        Assert.Empty(logger.Errors);
    }

    [Fact]
    public void Save_WhenCaptureThrows_LogsFailureAndPreservesSnapshot()
    {
        var previous = Snapshot(7);
        var game = new FakeGameInterop { ThrowOn = "CaptureCruiser" };
        var store = new CruiserStateStore { SavedCruiserState = previous };
        var logger = new RecordingPluginLogger();
        var validation = new RecordingValidationLogger();

        var result = new SaveCruiserStateUseCase(game, store, logger, validation).Execute();

        Assert.Equal(SaveCruiserStateResult.UnexpectedState, result);
        Assert.Same(previous, store.SavedCruiserState);
        Assert.Contains("Injected failure: CaptureCruiser", Assert.Single(logger.Errors));
        Assert.Equal("unexpected_state", Assert.Single(validation.Records).Fields!["result"]);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Load_WhenNoCruiser_RejectsBeforeOtherPreconditions(bool hasSavedState)
    {
        var saved = hasSavedState ? Snapshot(31) : null;
        var game = new FakeGameInterop { CruiserPresent = false, CruiserMagneted = true };
        var store = new CruiserStateStore { SavedCruiserState = saved };
        var validation = new RecordingValidationLogger();

        var result = new LoadCruiserStateUseCase(game, store, new RecordingPluginLogger(), validation).Execute();

        Assert.Equal(LoadCruiserStateResult.NoCruiserFound, result);
        Assert.Equal(new[] { "CruiserExists" }, game.Calls);
        Assert.Null(game.RestoredSnapshot);
        Assert.Same(saved, store.SavedCruiserState);
        var record = Assert.Single(validation.Records);
        Assert.Equal("no_cruiser_found", record.Fields!["result"]);
        Assert.Equal(hasSavedState, record.Fields["saved_state"]);
    }

    [Fact]
    public void Load_WhenNoSavedState_RejectsBeforeCheckingMagnet()
    {
        var game = new FakeGameInterop { CruiserMagneted = true };
        var validation = new RecordingValidationLogger();

        var result = new LoadCruiserStateUseCase(game, new CruiserStateStore(), new RecordingPluginLogger(), validation).Execute();

        Assert.Equal(LoadCruiserStateResult.NoSavedState, result);
        Assert.Equal(new[] { "CruiserExists" }, game.Calls);
        Assert.Null(game.RestoredSnapshot);
        Assert.Equal("no_saved_state", Assert.Single(validation.Records).Fields!["result"]);
    }

    [Fact]
    public void Load_WhenMagneted_DoesNotRestoreOrChangeSavedState()
    {
        var saved = Snapshot(31);
        var game = new FakeGameInterop { CruiserMagneted = true };
        var store = new CruiserStateStore { SavedCruiserState = saved };
        var validation = new RecordingValidationLogger();

        var result = new LoadCruiserStateUseCase(game, store, new RecordingPluginLogger(), validation).Execute();

        Assert.Equal(LoadCruiserStateResult.MagnetedToShip, result);
        Assert.Equal(new[] { "CruiserExists", "IsCruiserMagnetedToShip" }, game.Calls);
        Assert.Null(game.RestoredSnapshot);
        Assert.Same(saved, store.SavedCruiserState);
        Assert.Equal("magneted_to_ship", Assert.Single(validation.Records).Fields!["result"]);
    }

    [Fact]
    public void Load_WhenValid_RestoresSavedSnapshotAndRecordsObservationBeforeSuccess()
    {
        var saved = Snapshot(31);
        var game = new FakeGameInterop
        {
            RestoreObservation = new CruiserRestoreObservation(saved.CarPosition, saved.CarRotation,
                new Vector3Value(8, 9, 10), saved.CarPosition, 31, 7, 31, 4, 1, 4)
        };
        var store = new CruiserStateStore { SavedCruiserState = saved };
        var logger = new RecordingPluginLogger();
        var validation = new RecordingValidationLogger();

        var result = new LoadCruiserStateUseCase(game, store, logger, validation).Execute();

        Assert.Equal(LoadCruiserStateResult.Success, result);
        Assert.Same(saved, game.RestoredSnapshot);
        Assert.Same(saved, store.SavedCruiserState);
        Assert.Equal(new[] { "CruiserExists", "IsCruiserMagnetedToShip", "RestoreCruiser" }, game.Calls);
        Assert.Equal(new[] { "restore_applied", "load_result" }, validation.Records.Select(record => record.EventName));
        Assert.Equal(7, validation.Records[0].Fields!["before_hp"]);
        Assert.Equal(31, validation.Records[0].Fields!["after_hp"]);
        Assert.Equal("success", validation.Records[1].Fields!["result"]);
        Assert.Empty(logger.Errors);
    }

    [Theory]
    [InlineData("CruiserExists")]
    [InlineData("IsCruiserMagnetedToShip")]
    [InlineData("RestoreCruiser")]
    public void Load_WhenInteropThrows_LogsFailureWithoutSuccess(string operation)
    {
        var saved = Snapshot(31);
        var game = new FakeGameInterop { ThrowOn = operation };
        var store = new CruiserStateStore { SavedCruiserState = saved };
        var logger = new RecordingPluginLogger();
        var validation = new RecordingValidationLogger();

        var result = new LoadCruiserStateUseCase(game, store, logger, validation).Execute();

        Assert.Equal(LoadCruiserStateResult.UnexpectedState, result);
        Assert.Same(saved, store.SavedCruiserState);
        Assert.Contains($"Injected failure: {operation}", Assert.Single(logger.Errors));
        var record = Assert.Single(validation.Records);
        Assert.Equal("load_result", record.EventName);
        Assert.Equal("unexpected_state", record.Fields!["result"]);
    }
}
