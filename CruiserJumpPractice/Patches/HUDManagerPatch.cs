#nullable enable

using BepInEx.Logging;
using HarmonyLib;
using CruiserJumpPractice.BaseGame.Controllers;
using CruiserJumpPractice.BaseGame.Controllers.Client;
using CruiserJumpPractice.BaseGame.Finders;
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
        var networkManagerFinder = new NetworkManagerFinder();
        var networkManager = networkManagerFinder.GetNetworkManager();
        var networkStateController = new NetworkStateController(networkManager);
        if (!networkStateController.IsClient())
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

        CruiserJumpPractice.CruiserStateService.RequestSaveCruiserState(hudManager);
    }

    internal static void UpdateLoadCruiser(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        CruiserJumpPractice.CruiserStateService.RequestLoadCruiserState(hudManager);
    }

    internal static void UpdateToggleMagnet(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.ToggleMagnetKey?.triggered ?? false))
        {
            return;
        }

        CruiserJumpPractice.MagnetService.ToggleMagnet(hudManager);
    }

}
