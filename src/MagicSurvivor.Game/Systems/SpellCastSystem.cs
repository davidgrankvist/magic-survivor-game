using System.Numerics;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Systems;

public class SpellCastSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        if (!state.SpellState.ShouldAttemptCast)
        {
            return;
        }
        state.SpellState.ShouldAttemptCast = false;

        var spell = state.Spells.Get(state.SpellState.SelectedSpell);
        if (spell.Elapsed >= spell.Cooldown)
        {
            CastSpawnSpell(state, spell);
            spell.Elapsed = 0;
            return;
        }

        spell.Elapsed += deltaTime;
    }

    private void CastSpawnSpell(GameState state, Spell spell)
    {
        var entityDefinition = state.EntityDefinitions.Get(spell.SpawnEntity);
        var playerEntity = state.Entities.GetEntity(state.PlayerEntityHandle)!;
        var velocity = new Vector3(1, 0, 0) * entityDefinition.Speed;

        var entity = new Entity
        {
            DefinitionHandle = spell.SpawnEntity,
            Position = playerEntity.Position,
            Velocity = velocity,
        };
        state.Entities.AddEntity(entity);
    }
}