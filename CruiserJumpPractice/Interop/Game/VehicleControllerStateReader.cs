// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using System.Reflection;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game;

internal static class VehicleControllerStateReader
{
    private static readonly FieldInfo? turboBoostsField = typeof(VehicleController).GetField(
        name: "turboBoosts",
        bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance
    );

    public static int GetTurboBoosts(VehicleController cruiser)
    {
        if (turboBoostsField == null)
        {
            throw new GameInteropException(
                message: "Failed to get 'turboBoosts' field from VehicleController."
            );
        }

        var turboBoostsValue = turboBoostsField.GetValue(obj: cruiser);
        if (turboBoostsValue is int turboBoosts)
        {
            return turboBoosts;
        }

        throw new GameInteropException(message: "'turboBoosts' field is not of type int.");
    }
}
