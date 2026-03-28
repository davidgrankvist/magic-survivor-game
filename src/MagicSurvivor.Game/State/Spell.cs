namespace MagicSurvivor.Game.State;

public class Spell
{
    public SpelLCategory Category = SpelLCategory.None;
    public float Cooldown;
    public float Elapsed;

    public StaticHandle SpawnEntity = StaticHandle.InvalidHandle;

    public float AimLength;
}
