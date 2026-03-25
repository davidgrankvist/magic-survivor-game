namespace MagicSurvivor.Game.State;

public struct StaticHandle
{
    public int Index;

    public static readonly StaticHandle InvalidHandle = new StaticHandle
    {
        Index = -1
    };

    public static bool operator ==(StaticHandle first, StaticHandle second)
    {
        return Equals(first, second);
    }

    public static bool operator !=(StaticHandle first, StaticHandle second)
    {
        return !(first == second);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is not StaticHandle handle)
        {
            return false;
        }

        return Index == handle.Index;
    }

    public override readonly int GetHashCode()
    {
        return Index.GetHashCode();
    }
}