#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;

namespace CruiserJumpPractice.Core.UseCases.Client;

/// <summary>
/// Maps save results to player-facing HUD feedback.
/// </summary>
/// <remarks>
/// Save result wording belongs with practice behavior, while displaying the tip
/// remains a game operation behind IGameInterop.
/// </remarks>
internal sealed class PresentSaveCruiserStateResultUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginLogger logger;

    public PresentSaveCruiserStateResultUseCase(IGameInterop gameInterop, IPluginLogger logger)
    {
        this.gameInterop = gameInterop;
        this.logger = logger;
    }

    public void Execute(SaveCruiserStateResult result)
    {
        if (result == SaveCruiserStateResult.Success)
        {
            DisplayTip(HudTipMessage.SaveSuccess);
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            DisplayTip(HudTipMessage.SaveNoCruiser);
        }
        else
        {
            // Unexpected results are diagnostics, not player-facing practice
            // states. Known failures get HUD tips; unknown values stay in logs.
            logger.LogError($"Unknown SaveCruiserStateResult: {result}");
        }
    }

    private void DisplayTip(HudTipMessage message)
    {
        gameInterop.DisplayTip(message);
    }
}
