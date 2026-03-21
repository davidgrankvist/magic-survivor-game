namespace MagicSurvivor.Game.State;

public struct StaticHandle
{
    public int Index;

    public static readonly StaticHandle InvalidHandle = new StaticHandle
    {
        Index = -1
    };
}