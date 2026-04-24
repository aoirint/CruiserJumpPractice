#nullable enable

using BepInEx.Logging;
using HarmonyLib;
using CruiserJumpPractice.BaseGame.Controllers.Client;
using CruiserJumpPractice.BaseGame.Controllers.Server;
using CruiserJumpPractice.BaseGame.Finders;
using CruiserJumpPractice.Utils;
using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.Patches;

[HarmonyPatch(typeof(HUDManager))]
internal class HUDManagerPatch
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [HarmonyPatch(nameof(HUDManager.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix(HUDManager __instance)
    {
        var gameObject = __instance.gameObject;
        if (gameObject == null)
        {
            Logger.LogError("HUDManager.gameObject is null.");
            return;
        }

        gameObject.AddComponent<CruiserStateNetworkBehaviour>();
    }

    [HarmonyPatch(nameof(HUDManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix(HUDManager __instance)
    {
        if (!NetworkUtils.IsClient())
        {
            return;
        }

        var localPlayerFinder = new LocalPlayerFinder();
        var localPlayer = localPlayerFinder.GetLocalPlayer();
        var playerStatusController = new PlayerStatusController(localPlayer);
        if (playerStatusController.IsPlayerBusy())
        {
            return;
        }

        UpdateSaveCruiser(__instance);
        UpdateLoadCruiser(__instance);
        UpdateToggleMagnet(__instance);
    }

    internal static void UpdateSaveCruiser(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.SaveCruiserKey?.triggered ?? false))
        {
            return;
        }

        // Only the host can save the cruiser state
        if (!NetworkUtils.IsHost())
        {
            var tipController = new TipController(hudManager);
            tipController.DisplayTip("CruiserJumpPractice", "Only the host can save the cruiser state.");
            return;
        }

        var cruiserStateNetworkBehaviour = NetworkBehaviourUtils.GetCruiserStateNetworkBehaviour();
        if (cruiserStateNetworkBehaviour == null)
        {
            Logger.LogError("CruiserStateNetworkBehaviour is null.");
            return;
        }

        cruiserStateNetworkBehaviour.SaveCruiserStateServerRpc();
    }

    internal static void UpdateLoadCruiser(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        // Only the host can load the cruiser state
        if (!NetworkUtils.IsHost())
        {
            var tipController = new TipController(hudManager);
            tipController.DisplayTip("CruiserJumpPractice", "Only the host can load the cruiser state.");
            return;
        }

        var cruiserStateNetworkBehaviour = NetworkBehaviourUtils.GetCruiserStateNetworkBehaviour();
        if (cruiserStateNetworkBehaviour == null)
        {
            Logger.LogError("CruiserStateNetworkBehaviour is null.");
            return;
        }

        cruiserStateNetworkBehaviour.LoadCruiserStateServerRpc();
    }

    internal static void UpdateToggleMagnet(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.ToggleMagnetKey?.triggered ?? false))
        {
            return;
        }

        if (!NetworkUtils.IsHost())
        {
            var tipController = new TipController(hudManager);
            tipController.DisplayTip("CruiserJumpPractice", "Only the host can toggle the magnet.");
            return;
        }

        var startOfRoundFinder = new StartOfRoundFinder();
        var startOfRound = startOfRoundFinder.GetStartOfRound();
        var magnetController = new MagnetController(startOfRound);
        var newMagnetState = !magnetController.IsMagnetOn();

        // NOTE: This value will be synced with vanilla Server RPC
        magnetController.ToggleMagnet();

        var magnetStateText = newMagnetState ? "ON" : "OFF";
        var localTipController = new TipController(hudManager);
        localTipController.DisplayTip("CruiserJumpPractice", $"Magnet is now {magnetStateText}.");
    }
}
