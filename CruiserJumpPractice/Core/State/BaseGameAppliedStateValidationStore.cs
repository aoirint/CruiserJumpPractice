// SPDX-License-Identifier: MIT
#nullable enable

namespace CruiserJumpPractice.Core.State;

internal sealed class BaseGameAppliedStateValidationStore
{
    // The local apply helpers are also used by the initiating restore path. These depth markers
    // identify calls made from inside the base-game ClientRpc path so #100 logs receiver-side
    // applied state without duplicating #97 sender-side restore observations.
    private int engineOilClientRpcDepth;
    private int turboClientRpcDepth;

    public bool IsEngineOilClientRpcApplyActive => engineOilClientRpcDepth > 0;

    public bool IsTurboClientRpcApplyActive => turboClientRpcDepth > 0;

    public void EnterEngineOilClientRpc()
    {
        engineOilClientRpcDepth++;
    }

    public void ExitEngineOilClientRpc()
    {
        if (engineOilClientRpcDepth > 0)
        {
            engineOilClientRpcDepth--;
        }
    }

    public void EnterTurboClientRpc()
    {
        turboClientRpcDepth++;
    }

    public void ExitTurboClientRpc()
    {
        if (turboClientRpcDepth > 0)
        {
            turboClientRpcDepth--;
        }
    }
}
