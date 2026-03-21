namespace MagicSurvivor.Game.State.Collections;

public class StaticRepository<TObject>
{
    private List<TObject> objects = [];

    public TObject Get(StaticHandle handle)
    {
        return objects[handle.Index]!;
    }

    public void Add(TObject obj)
    {
        objects.Add(obj);
    }
}