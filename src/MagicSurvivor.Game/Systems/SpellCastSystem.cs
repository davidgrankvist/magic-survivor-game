using System.Numerics;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Systems;

public class SpellCastSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        // Make sure that all cooldowns are updated even if the active spell is changed
        foreach (var spell in state.Spells)
        {
            spell.Elapsed += deltaTime;
            spell.TickElapsed += deltaTime;
            spell.TickActive = false;
        }

        if (state.SpellState.ShouldAttemptCast)
        {
            CastSelectedSpell(state, deltaTime);
        }

        // Assume only one active Aoe
        if (state.SpellState.AoeIsActive)
        {
            UpdateAoe(state, deltaTime);
        }
    }

    private void UpdateAoe(GameState state, float deltaTime)
    {
        var spell = state.Spells.Get(state.SpellState.ActiveAoe);

        // End Aoe
        if (spell.Elapsed >= spell.Duration)
        {
            state.SpellState.ActiveAoe = StaticHandle.InvalidHandle;
            return;
        }

        // Update tick
        if (spell.TickElapsed >= spell.TickCooldown)
        {
            // Signal to other systems to apply Aoe damage
            spell.TickActive = true;
            spell.TickElapsed = 0;
        }
    }

    private void CastSelectedSpell(GameState state, float deltaTime)
    {
        state.SpellState.ShouldAttemptCast = false;

        var spell = state.Spells.Get(state.SpellState.SelectedSpell);
        if (spell.Elapsed >= spell.Cooldown)
        {
            CastSpell(state, spell);
            spell.Elapsed = 0;
            return;
        }
    }

    private void CastSpell(GameState state, Spell spell)
    {
        switch (spell.Category)
        {
            case SpellCategory.Projectile:
                SpawnProjectile(state, spell);
                break;
            case SpellCategory.Aoe:
                // Assume only one active Aoe
                if (!state.SpellState.AoeIsActive)
                {
                    StartAoe(state, spell);
                }
                break;
        }
    }

    private void SpawnProjectile(GameState state, Spell spell)
    {
        var entityDefinition = state.EntityDefinitions.Get(spell.SpawnEntity);
        var playerEntity = state.Entities.GetEntity(state.PlayerEntityHandle)!;
        var velocity = Vector3.Normalize(state.SpellState.AimPos - playerEntity.Position) * entityDefinition.Speed;

        var entity = new Entity
        {
            DefinitionHandle = spell.SpawnEntity,
            Position = playerEntity.Position,
            Velocity = velocity,
        };
        state.Entities.AddEntity(entity);
    }

    private void StartAoe(GameState state, Spell spell)
    {
        spell.TickElapsed = 0;
        state.SpellState.ActiveAoe = spell.Handle;
    }
}