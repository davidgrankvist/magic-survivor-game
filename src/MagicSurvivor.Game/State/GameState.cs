using MagicSurvivor.Game.State.Collections;

namespace MagicSurvivor.Game.State;

public class GameState
{
    public EntityRepository Entities = new();
    public StaticRepository<EntityDefinition> EntityDefinitions = new();
    public GameCamera Camera = new();
    public EditorState Editor = new();
    public LevelState Level = new();
}
