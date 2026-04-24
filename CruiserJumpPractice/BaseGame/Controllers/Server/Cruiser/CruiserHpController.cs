#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.BaseGame.Controllers.Server.Cruiser;

class CruiserHpControllerException : System.Exception
{
    public CruiserHpControllerException(string message) : base(message) { }
}

class CruiserHpController
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    protected VehicleController cruiser;
    protected int localPlayerId;

    public CruiserHpController(
        VehicleController cruiser,
        int localPlayerId
    )
    {
        this.cruiser = cruiser;
        this.localPlayerId = localPlayerId;
    }

    /// <summary>
    /// Gets the current HP for the cruiser.
    /// </summary>
    /// <returns>The current HP value for the cruiser.</returns>
    /// <exception cref="CruiserHpControllerException"></exception>
    public int GetCruiserHP()
    {
        try
        {
            return cruiser.carHP;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'carHP': {error}");
            throw new CruiserHpControllerException($"Exception while getting 'carHP': {error}");
        }
    }

    /// <summary>
    /// Sets the HP for the cruiser.
    /// </summary>
    /// <param name="carHP">The new HP value to set for the cruiser.</param>
    /// <exception cref="CruiserHpControllerException"></exception>
    public void SetCruiserHP(int carHP)
    {
        try
        {
            // Set for the local client
            cruiser.AddEngineOilOnLocalClient(carHP);

            // Sync with the vanilla Server RPC
            cruiser.AddEngineOilServerRpc(localPlayerId, carHP);
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while setting 'carHP': {error}");
            throw new CruiserHpControllerException($"Exception while setting 'carHP': {error}");
        }
    }
}
