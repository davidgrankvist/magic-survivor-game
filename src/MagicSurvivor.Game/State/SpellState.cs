using System.Numerics;

namespace MagicSurvivor.Game.State;

public class SpellState
{
    public bool ShouldAttemptCast;
    public StaticHandle SelectedSpell = StaticHandle.InvalidHandle;

    // Projectiles
    public bool AimEnabled;
    public Vector3 AimPos;

    // Aoe
    public StaticHandle ActiveAoe = StaticHandle.InvalidHandle;
    public bool AoeIsActive => ActiveAoe != StaticHandle.InvalidHandle;
}
