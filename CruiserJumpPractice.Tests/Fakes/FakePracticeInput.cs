using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Tests.Fakes;

internal sealed class FakePracticeInput : IPracticeInput
{
    public bool Save { get; set; }
    public bool Load { get; set; }
    public bool Toggle { get; set; }
    public int SaveReads { get; private set; }
    public int LoadReads { get; private set; }
    public int ToggleReads { get; private set; }
    public bool SaveCruiserTriggered { get { SaveReads++; return Save; } }
    public bool LoadCruiserTriggered { get { LoadReads++; return Load; } }
    public bool ToggleMagnetTriggered { get { ToggleReads++; return Toggle; } }
}
