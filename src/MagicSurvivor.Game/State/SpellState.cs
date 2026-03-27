using System.Numerics;

namespace MagicSurvivor.Game.State;

public class SpellState
{
    public bool ShouldAttemptCast;
    public StaticHandle SelectedSpell = StaticHandle.InvalidHandle;

    public bool AimEnabled;
    public Vector3 AimPos;
}
