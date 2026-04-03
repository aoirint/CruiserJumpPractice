#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.BaseGame.Controllers.Server.Cruiser;

class CruiserTurboBoostControllerException : System.Exception
{
    public CruiserTurboBoostControllerException(string message) : base(message) { }
}

class CruiserTurboBoostController
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    protected VehicleController cruiser;
    protected int localPlayerId;

    public CruiserTurboBoostController(
        VehicleController cruiser,
        int localPlayerId
    )
    {
        this.cruiser = cruiser;
        this.localPlayerId = localPlayerId;
    }

    /// <summary>
    /// Gets the current number of turbo boosts for the cruiser.
    /// </summary>
    /// <returns>The current number of turbo boosts.</returns>
    /// <exception cref="CruiserTurboBoostControllerException"></exception>
    public int GetCruiserTurboBoosts()
    {
        try
        {
            var turboBoostsField = typeof(VehicleController).GetField("turboBoosts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (turboBoostsField == null)
            {
                throw new CruiserTurboBoostControllerException("Failed to get 'turboBoosts' field from VehicleController.");
            }

            var turboBoostsValue = turboBoostsField.GetValue(cruiser);
            if (turboBoostsValue is int turboBoosts)
            {
                return turboBoosts;
            }
            else
            {
                throw new CruiserTurboBoostControllerException("'turboBoosts' field is not of type int.");
            }
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'turboBoosts': {error}");
            throw new CruiserTurboBoostControllerException($"Exception while getting 'turboBoosts': {error}");
        }
    }

    /// <summary>
    /// Sets the number of turbo boosts for the cruiser.
    /// </summary>
    /// <param name="turboBoosts">The number of turbo boosts to set.</param>
    /// <exception cref="CruiserTurboBoostControllerException"></exception>
    public void SetCruiserTurboBoosts(int turboBoosts)
    {
        try {
            // Set for the local client
            cruiser.AddTurboBoostOnLocalClient(turboBoosts);

            // Sync with the vanilla Server RPC
            cruiser.AddTurboBoostServerRpc(localPlayerId, turboBoosts);
        } catch (System.Exception error) {
            Logger.LogError($"Exception while setting 'turboBoosts': {error}");
            throw new CruiserTurboBoostControllerException($"Exception while setting 'turboBoosts': {error}");
        }
    }
}
