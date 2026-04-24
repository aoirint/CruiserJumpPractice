#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.BaseGame.Controllers.Server.Cruiser;

class CruiserMagnetControllerException : System.Exception
{
    public CruiserMagnetControllerException(string message) : base(message) { }
}

class CruiserMagnetController
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    protected VehicleController cruiser;

    public CruiserMagnetController(VehicleController cruiser)
    {
        this.cruiser = cruiser;
    }

    /// <summary>
    /// Gets whether the cruiser is currently magneted to the ship.
    /// </summary>
    /// <returns>True if the cruiser is magneted to the ship, false otherwise.</returns>
    /// <exception cref="CruiserMagnetControllerException"></exception>
    public bool GetMagnetedToShip()
    {
        try
        {
            return cruiser.magnetedToShip;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'magnetedToShip': {error}");
            throw new CruiserMagnetControllerException($"Exception while getting 'magnetedToShip': {error}");
        }
    }
}
