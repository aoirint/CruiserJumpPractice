// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;

namespace CruiserJumpPractice.Core.UseCases.Client;

// A save key press is handled locally first so non-host players get immediate feedback. Only a
// host request crosses into the RPC path where the server captures the snapshot.
internal sealed class RequestSaveCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;

    public RequestSaveCruiserStateUseCase(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public RequestSaveCruiserStateResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip(HudTipMessage.SaveHostOnly);
            return RequestSaveCruiserStateResult.HostOnly;
        }

        gameInterop.RequestSaveCruiserState();
        return RequestSaveCruiserStateResult.Success;
    }
}
