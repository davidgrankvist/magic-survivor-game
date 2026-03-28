namespace MagicSurvivor.Game.State;

public class Spell
{
    public StaticHandle Handle  = StaticHandle.InvalidHandle;
    public string Name = string.Empty;
    public SpelLCategory Category = SpelLCategory.None;
    public float Cooldown;
    public float Elapsed;

    public StaticHandle SpawnEntity = StaticHandle.InvalidHandle;

    public float AimLength;
}
