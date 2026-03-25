using MagicSurvivor.Game.State.Collections;

namespace MagicSurvivor.Game.State;

public class GameState
{
    public EntityRepository Entities = new();
    public StaticRepository<EntityDefinition> EntityDefinitions = new();
    public StaticRepository<Spell> Spells = new();

    public GameCamera Camera = new();
    public EditorState Editor = new();
    public LevelState Level = new();
    public SpellState SpellState = new();

    public EntityHandle PlayerEntityHandle = EntityHandle.InvalidHandle;
    public StaticHandle PlayerEntityDefinitionHandle = StaticHandle.InvalidHandle;
}