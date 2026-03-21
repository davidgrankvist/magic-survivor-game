using MagicSurvivor.Game.State.Collections;

namespace MagicSurvivor.Game.State;

public class GameState
{
    public EntityRepository Entities = new();
    public StaticRepository<EntityDefinition> EntityDefinitions = new();
}
