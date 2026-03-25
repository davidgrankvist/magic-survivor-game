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

    public static bool operator ==(EntityHandle first, EntityHandle second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(EntityHandle first, EntityHandle second)
    {
        return !(first == second);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is not EntityHandle handle)
        {
            return false;
        }

        return Index == handle.Index && Generation == handle.Generation;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Index, Generation);
    }
}
