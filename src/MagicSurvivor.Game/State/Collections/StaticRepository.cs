using System.Collections;

namespace MagicSurvivor.Game.State.Collections;

public class StaticRepository<TObject> : IEnumerable<TObject>
{
    private List<TObject> objects = [];

    public int Count => objects.Count;

    public TObject this[int i]
    {
        get => objects[i];
    }

    public TObject Get(StaticHandle handle)
    {
        return objects[handle.Index]!;
    }

    public void Add(TObject obj)
    {
        objects.Add(obj);
    }

    public void Clear()
    {
        objects.Clear();
    }

    public IEnumerator<TObject> GetEnumerator()
    {
        return objects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}