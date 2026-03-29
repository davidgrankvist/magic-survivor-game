using System.Numerics;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;
using Raylib_cs;

namespace MagicSurvivor.Game.Systems;

/// <summary>
/// Checks for entity pairwise interactions such as spells hitting enemies or enemies performing attacks.
/// </summary>
public class EntityInteractionSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        // Naive pairwise checks for now. OK as long as there are few entities. May need a spatial index later.
        for (var i = 0; i < state.Entities.Count; i++)
        {
            var first = state.Entities[i];
            if (first.IsDeleted)
            {
                continue;
            }

            for (var j = i + 1; j < state.Entities.Count; j++)
            {
                var second = state.Entities[j];
                if (second.IsDeleted)
                {
                    continue;
                }

                var firstDef = state.EntityDefinitions.Get(first.DefinitionHandle);
                var secondDef = state.EntityDefinitions.Get(second.DefinitionHandle);
                HandleInteraction(state, first, firstDef, second, secondDef);
            }
        }
    }

    private void HandleInteraction(GameState state,
        Entity first, EntityDefinition firstDef,
        Entity second, EntityDefinition secondDef)
    {
        switch (firstDef.Category)
        {
            case EntityCategory.Player:
                if (secondDef.Category == EntityCategory.Npc)
                {
                    HandlePlayerNpcInteraction(state, first, firstDef, second, secondDef);
                }
                break;
            case EntityCategory.Npc:
                if (secondDef.Category == EntityCategory.Projectile)
                {
                    HandleNpcProjectileHit(state, first, firstDef, second, secondDef);
                }
                break;
        }
    }

    private void HandlePlayerNpcInteraction(GameState state,
        Entity first, EntityDefinition firstDef,
        Entity second, EntityDefinition secondDef)
    {
        if (state.SpellState.AoeIsActive)
        {
            ApplyPlayerAoe(state, first, second);
        }

        if (secondDef.AttackType == EntityAttackType.Melee)
        {
            AttemptNpcMelee(state, first, firstDef, second, secondDef);
        }
    }

    private void AttemptNpcMelee(GameState state,
        Entity first, EntityDefinition firstDef,
        Entity second, EntityDefinition secondDef)
    {
        // Begin strike
        var strikeElapsed = state.CurrenTime - second.StrikeBegin;
        if (strikeElapsed >= secondDef.StrikeCooldown)
        {
            // Begin strike if close
            var distance = Vector3.Distance(first.Position, second.Position);
            if (distance <= secondDef.MeleeRange)
            {
                second.StrikeBegin = state.CurrenTime;
                second.IsStriking = true;
            }
        }

        // Do strike
        var strikeWindupElapsed = state.CurrenTime - second.StrikeBegin;
        if (second.IsStriking && strikeWindupElapsed >= secondDef.StrikeWindup)
        {
            // Attempt to hit
            var distance = Vector3.Distance(first.Position, second.Position);
            if (distance <= secondDef.MeleeRange)
            {
                first.Health -= secondDef.Damage;
                // TODO: handle player defeated
            }

            // Back on cooldown
            second.StrikeBegin = state.CurrenTime;
            second.IsStriking = false;
        }
    }

    private void ApplyPlayerAoe(GameState state, Entity first, Entity second)
    {
        var spell = state.Spells.Get(state.SpellState.ActiveAoe);
        if (!spell.TickActive)
        {
            return;
        }

        var distance = Vector3.Distance(first.Position, second.Position);
        if (distance >= spell.Range)
        {
            return;
        }

        second.Health -= spell.Damage;
        if (second.Health <= 0)
        {
            HandleEnemyDead(state, second.Handle);
        }
    }

    private void HandleNpcProjectileHit(GameState state,
        Entity first, EntityDefinition firstDef,
        Entity second, EntityDefinition secondDef)
    {
        var firstBox = new BoundingBox
        {
            Min = first.Position - firstDef.Collider / 2f,
            Max = first.Position + firstDef.Collider / 2f,
        };
        var secondBox = new BoundingBox
        {
            Min = second.Position - secondDef.Collider / 2f,
            Max = second.Position + secondDef.Collider / 2f,
        };
        var didCollide = Raylib.CheckCollisionBoxes(firstBox, secondBox);
        if (!didCollide)
        {
            return;
        }

        first.Health -= secondDef.Damage;
        state.Entities.Remove(second.Handle);
        if (first.Health <= 0)
        {
            HandleEnemyDead(state, first.Handle);
        }
    }

    private void HandleEnemyDead(GameState state, EntityHandle handle)
    {
        state.Entities.Remove(handle);
        state.Level.EnemyCount--;
        if (state.Level.EnemyCount > 0)
        {
            return;

        }

        var isFinalWave = state.Level.CurrentWave.Index == state.Waves.Count - 1;
        if (!isFinalWave)
        {
            // Signal to wave spawn system to load the next wave
            state.Level.CurrentWave.Index++;
            state.Level.ShouldSpawnWave = true;
        }

        // Do nothing for now if it was the final wave.
    }
}