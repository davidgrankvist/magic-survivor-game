namespace MagicSurvivor.Game.State;

public class SpellState
{
    public bool ShouldAttemptCast;
    public StaticHandle SelectedSpell = StaticHandle.InvalidHandle;
}
