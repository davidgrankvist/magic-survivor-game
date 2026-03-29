using System.Numerics;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Systems;

/// <summary>
/// Manages spell casting and cooldowns.
/// </summary>
public class SpellCastSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        // Reset Aoe ticks and let timer based logic activate them
        foreach (var spell in state.Spells)
        {
            spell.TickActive = false;
        }

        if (state.SpellState.ShouldAttemptCast)
        {
            CastSelectedSpell(state);
        }

        // Assume only one active Aoe
        if (state.SpellState.AoeIsActive)
        {
            UpdateAoe(state);
        }
    }

    private void UpdateAoe(GameState state)
    {
        var spell = state.Spells.Get(state.SpellState.ActiveAoe);

        // End Aoe
        var spellElapsed = state.CurrenTime - spell.CastBegin;
        if (spellElapsed >= spell.Duration)
        {
            state.SpellState.ActiveAoe = StaticHandle.InvalidHandle;
            return;
        }

        // Update tick
        var tickElapsed = state.CurrenTime - spell.TickBegin;
        if (tickElapsed >= spell.TickCooldown)
        {
            // Signal to other systems to apply Aoe damage
            spell.TickActive = true;
            spell.TickBegin = state.CurrenTime;
        }
    }

    private void CastSelectedSpell(GameState state)
    {
        // Consume selection to only cast once
        state.SpellState.ShouldAttemptCast = false;

        var spell = state.Spells.Get(state.SpellState.SelectedSpell);
        var spellElapsed = state.CurrenTime - spell.CastBegin;
        if (spellElapsed >= spell.Cooldown)
        {
            CastSpell(state, spell);
            spell.CastBegin = state.CurrenTime;
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
        var playerEntity = state.Entities.Get(state.PlayerEntityHandle)!;
        var velocity = Vector3.Normalize(state.SpellState.AimPos - playerEntity.Position) * entityDefinition.Speed;

        var entity = new Entity
        {
            DefinitionHandle = spell.SpawnEntity,
            Position = playerEntity.Position,
            Velocity = velocity,
        };
        state.Entities.Add(entity);
    }

    private void StartAoe(GameState state, Spell spell)
    {
        state.SpellState.ActiveAoe = spell.Handle;
    }
}