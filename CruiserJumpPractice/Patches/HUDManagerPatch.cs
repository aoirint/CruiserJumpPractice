#nullable enable

using BepInEx.Logging;
using HarmonyLib;
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

        gameObject.AddComponent<CruiserJumpPracticeNetworkBehaviour>();
    }

    [HarmonyPatch(nameof(HUDManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix(HUDManager __instance)
    {
        UpdateSaveCruiser();
        UpdateLoadCruiser();
    }

    internal static void UpdateSaveCruiser()
    {
        if (!NetworkUtils.IsServer())
        {
            return;
        }

        if (!(CruiserJumpPractice.InputActions?.SaveCruiserKey?.triggered ?? false))
        {
            return;
        }

        // Only the host can save the cruiser state
        if (!NetworkUtils.IsHost())
        {
            HUDManagerUtils.DisplayTip("CruiserJumpPractice", "Only the host can save the cruiser state.");
            return;
        }

        var cruiserJumpPracticeNetworkBehaviour = NetworkBehaviourUtils.GetCruiserJumpPracticeNetworkBehaviour();
        if (cruiserJumpPracticeNetworkBehaviour == null)
        {
            Logger.LogError("CruiserJumpPracticeNetworkBehaviour is null.");
            return;
        }

        cruiserJumpPracticeNetworkBehaviour.SaveCruiserStateServerRpc();
    }

    internal static void UpdateLoadCruiser()
    {
        if (!NetworkUtils.IsClient())
        {
            return;
        }

        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        // Only the host can load the cruiser state
        if (!NetworkUtils.IsHost())
        {
            HUDManagerUtils.DisplayTip("CruiserJumpPractice", "Only the host can load the cruiser state.");
            return;
        }

        var cruiserJumpPracticeNetworkBehaviour = NetworkBehaviourUtils.GetCruiserJumpPracticeNetworkBehaviour();
        if (cruiserJumpPracticeNetworkBehaviour == null)
        {
            Logger.LogError("CruiserJumpPracticeNetworkBehaviour is null.");
            return;
        }

        cruiserJumpPracticeNetworkBehaviour.LoadCruiserStateServerRpc();
    }
}
