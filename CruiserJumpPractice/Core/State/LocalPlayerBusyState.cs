// SPDX-License-Identifier: Unlicense
#nullable enable

namespace CruiserJumpPractice.Core.State;

// Future validation logs need a suppression reason, but this state intentionally carries only
// closed booleans/tokens and never raw input, UI text, player identifiers, or Unity objects.
internal readonly struct LocalPlayerBusyState
{
    public const string MenuReasonToken = "menu";
    public const string TerminalReasonToken = "terminal";
    public const string ChatReasonToken = "chat";
    public const string MultipleReasonToken = "multiple";

    public LocalPlayerBusyState(bool isMenuOpen, bool isInTerminal, bool isTypingChat)
    {
        IsMenuOpen = isMenuOpen;
        IsInTerminal = isInTerminal;
        IsTypingChat = isTypingChat;
    }

    public bool IsMenuOpen { get; }

    public bool IsInTerminal { get; }

    public bool IsTypingChat { get; }

    public bool IsBusy => IsMenuOpen || IsInTerminal || IsTypingChat;

    public string? GetBusyReasonToken()
    {
        var busyReasonCount = 0;
        busyReasonCount += IsMenuOpen ? 1 : 0;
        busyReasonCount += IsInTerminal ? 1 : 0;
        busyReasonCount += IsTypingChat ? 1 : 0;

        if (busyReasonCount > 1)
        {
            return MultipleReasonToken;
        }

        if (IsMenuOpen)
        {
            return MenuReasonToken;
        }

        if (IsInTerminal)
        {
            return TerminalReasonToken;
        }

        if (IsTypingChat)
        {
            return ChatReasonToken;
        }

        return null;
    }
}
