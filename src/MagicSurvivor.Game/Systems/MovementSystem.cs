using System.Numerics;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Systems;

public class MovementSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        var playerEntity = state.Entities.GetEntity(state.PlayerEntityHandle)!;
        var playerPosition = playerEntity.Position;

        foreach (var entity in state.Entities)
        {
            if (entity.IsDeleted)
            {
                continue;
            }

            var entityDefinition = state.EntityDefinitions.Get(entity.DefinitionHandle);

            if (entityDefinition.Category != EntityCategory.Npc)
            {
                continue;
            }

            // Follow player.
            var target = playerPosition;
            var direction = Vector3.Normalize(target - entity.Position);
            var velocity = direction * entityDefinition.Speed;
            entity.Velocity = velocity;
        }
    }
}