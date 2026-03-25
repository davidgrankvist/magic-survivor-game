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

            // The player velocity is updated in the input system.
            if (entity.Handle == state.PlayerEntityHandle)
            {
                continue;
            }

            // Follow player.
            var entityDefinition = state.EntityDefinitions.Get(entity.DefinitionHandle);
            var target = playerPosition;
            var direction = Vector3.Normalize(target - entity.Position);
            var velocity = direction * entityDefinition.Speed;
            entity.Velocity = velocity;
        }
    }
}