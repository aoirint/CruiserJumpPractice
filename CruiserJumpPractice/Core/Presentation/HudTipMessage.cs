#nullable enable

namespace CruiserJumpPractice.Core.Presentation;

internal sealed class HudTipMessage
{
    private const string DefaultHeaderText = "CruiserJumpPractice";

    // Use cases select a closed token before display so future validation can
    // record stable intent without treating HUD text as an input boundary.
    public static readonly HudTipMessage SaveSuccess = new(
        "save_success",
        DefaultHeaderText,
        "Cruiser state saved."
    );

    public static readonly HudTipMessage SaveNoCruiser = new(
        "save_no_cruiser",
        DefaultHeaderText,
        "No cruiser found to save."
    );

    public static readonly HudTipMessage SaveHostOnly = new(
        "save_host_only",
        DefaultHeaderText,
        "Only the host can save the cruiser state."
    );

    public static readonly HudTipMessage LoadSuccess = new(
        "load_success",
        DefaultHeaderText,
        "Cruiser state loaded."
    );

    public static readonly HudTipMessage LoadNoCruiser = new(
        "load_no_cruiser",
        DefaultHeaderText,
        "No cruiser found to load."
    );

    public static readonly HudTipMessage LoadNoSavedState = new(
        "load_no_saved_state",
        DefaultHeaderText,
        "No saved cruiser state to load."
    );

    public static readonly HudTipMessage LoadMagnetedToShip = new(
        "load_magneted_to_ship",
        DefaultHeaderText,
        "Cannot load cruiser state while magneted to ship."
    );

    public static readonly HudTipMessage LoadHostOnly = new(
        "load_host_only",
        DefaultHeaderText,
        "Only the host can load the cruiser state."
    );

    public static readonly HudTipMessage MagnetHostOnly = new(
        "magnet_host_only",
        DefaultHeaderText,
        "Only the host can toggle the magnet."
    );

    public static readonly HudTipMessage MagnetOn = new(
        "magnet_on",
        DefaultHeaderText,
        "Magnet is now ON."
    );

    public static readonly HudTipMessage MagnetOff = new(
        "magnet_off",
        DefaultHeaderText,
        "Magnet is now OFF."
    );

    private HudTipMessage(string token, string headerText, string bodyText)
    {
        Token = token;
        HeaderText = headerText;
        BodyText = bodyText;
    }

    public string Token { get; }

    public string HeaderText { get; }

    public string BodyText { get; }
}
