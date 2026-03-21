namespace MagicSurvivor.Game.State;

public struct EntityHandle
{
    public int Index;
    public int Generation;

    public static readonly EntityHandle InvalidHandle = new EntityHandle
    {
        Index = -1,
        Generation = -1
    };
}
