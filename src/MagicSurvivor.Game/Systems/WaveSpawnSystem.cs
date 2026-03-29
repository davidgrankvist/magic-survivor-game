
using System.Numerics;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;
using Raylib_cs;

namespace MagicSurvivor.Game.Systems;

/// <summary>
/// Spawns enemies in waves as defined in the level configuration.
/// </summary>
public class WaveSpawnSystem : ISystem
{
    private readonly Random random = new(1234);

    public void Update(GameState state, float deltaTime)
    {
        if (state.Level.ShouldSpawnWave)
        {
            SpawnWaveEntities(state);
            state.Level.ShouldSpawnWave = false;
        }
    }

    private void SpawnWaveEntities(GameState state)
    {
        var wave = state.Waves.Get(state.Level.CurrentWave);
        var distance = state.WaveSettings.EnemySpawnDistance;
        var player = state.Entities.Get(state.PlayerEntityHandle)!;
        var center = player.Position;
        var enemyCount = 0;

        foreach (var spawn in wave.Spawns)
        {
            var entityDef = state.EntityDefinitions.Get(spawn.Handle);
            enemyCount += spawn.Count;
            for (var i = 0; i < spawn.Count; i++)
            {
                // Spawn along a circle around the player with randomimzed angles and offsets.
                var distanceTweak = random.NextSingle() * 5 + random.NextSingle() * -5;
                var baseOffset = new Vector3(1, 0, 0) * (distance + distanceTweak);
                var angle = random.NextSingle() * 2 * MathF.PI;
                var offset = Raymath.Vector3RotateByAxisAngle(baseOffset, GameConstants.UpAxis, angle);
                var pos = center + offset;

                var entity = new Entity
                {
                    DefinitionHandle = entityDef.Handle,
                    Position = pos,
                    Health = entityDef.Health,
                };
                state.Entities.Add(entity);
            }
        }

        state.Level.EnemyCount = enemyCount;
    }
}