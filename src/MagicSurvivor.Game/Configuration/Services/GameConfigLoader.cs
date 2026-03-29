using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Configuration.Services;

/// <summary>
/// Loads the parsed game or level configuration into the game state.
/// </summary>
public class GameConfigLoader
{
    public GameConfigLoader(bool loadFromSourceDir)
    {
        configFileService = new(loadFromSourceDir);
    }

    private readonly GameConfigFileService configFileService;
    private readonly Dictionary<string, StaticHandle> entityStringToHandle = [];
    private const string PlayerEntityType = "Player";

    public void LoadGameConfig(GameState state)
    {
        entityStringToHandle.Clear();

        var gameConfig = configFileService.ReadGameConfig();
        LoadCameraConfig(state.Camera, gameConfig.Camera);
        LoadWaveConfig(state, gameConfig.WaveSettings);
        LoadEntityDefinitions(state, gameConfig.EntityDefinitions);
        LoadSpells(state, gameConfig.Spells);

        state.PlayerEntityDefinitionHandle = EntityStringToHandle(PlayerEntityType);
        state.SpellState.SelectedSpell = new StaticHandle
        {
            Index = 0,
        };
    }

    public void LoadLevel(GameState state, string levelId)
    {
        state.Camera.Reset();

        var levelConfig = configFileService.ReadLevelConfig(levelId);
        LoadEntitites(state, levelConfig.Entities);
        LoadWaves(state, levelConfig.Waves);

        // Convenience accessor
        state.PlayerEntityHandle = new EntityHandle
        {
            Index = EntityStringToHandle(PlayerEntityType).Index,
            Generation = 0,
        };

        // Start at first wave
        state.Level.CurrentWave = new StaticHandle
        {
            Index = 0,
        };
        state.Level.ShouldSpawnWave = true;
    }

    private void LoadWaveConfig(GameState state, WaveSettingsConfig waveSettings)
    {
        state.WaveSettings.EnemySpawnDistance = waveSettings.EnemySpawnDistance;
    }

    private void LoadEntitites(GameState state, List<EntityConfig> entities)
    {
        state.Entities.Clear();
        for (var i = 0; i < entities.Count; i++)
        {
            var ec = entities[i];
            var defHandle = EntityStringToHandle(ec.Type);
            var def = state.EntityDefinitions.Get(defHandle);
            var entity = new Entity
            {
                DefinitionHandle = defHandle,
                Position = ec.Position,
                Health = def.Health,
                StrikeBegin = float.MinValue,
            };
            state.Entities.Add(entity);
        }
    }

    private void LoadWaves(GameState state, List<WaveConfig> waves)
    {
        state.Waves.Clear();
        for (var i = 0; i < waves.Count; i++)
        {
            var wc = waves[i];
            var wave = new Wave();

            for (var j = 0; j < wc.Spawns.Count; j++)
            {
                var sc = wc.Spawns[j];
                var defHandle = EntityStringToHandle(sc.Type);
                var spawn = new EntitySpawnSettings
                {
                    Handle = defHandle,
                    Count = sc.Count,
                };

                wave.Spawns.Add(spawn);
            }

            state.Waves.Add(wave);
        }
    }

    private void LoadCameraConfig(GameCamera camera, CameraConfig cameraConfig)
    {
        var offset = cameraConfig.OffsetFromTarget;
        var target = camera.RayCamera.Target;
        var newPos = offset + target;

        camera.OffsetFromTarget = offset;
        camera.RayCamera.FovY = cameraConfig.FovY;
        camera.RayCamera.Position = newPos;
    }

    private void LoadEntityDefinitions(GameState state, List<EntityDefinitionConfig> entityDefinitionConfigs)
    {
        state.EntityDefinitions.Clear();
        for (var i = 0; i < entityDefinitionConfigs.Count; i++)
        {
            var edc = entityDefinitionConfigs[i];
            var handle = new StaticHandle
            {
                Index = i,
            };
            entityStringToHandle[edc.Type] = handle;

            var ed = new EntityDefinition
            {
                Handle = handle,
                Category = Enum.Parse<EntityCategory>(edc.Category),
                Speed = edc.Speed,
                Collider = edc.Collider,
                Health = edc.Health,
                AttackType = ParseEnum<EntityAttackType>(edc.AttackType),
                Damage = edc.Damage,
                MeleeRange = edc.MeleeRange,
                StrikeWindup = edc.StrikeWindup,
                StrikeCooldown = edc.StrikeCooldown,
            };
            state.EntityDefinitions.Add(ed);
        }
    }

    private void LoadSpells(GameState state, List<SpellConfig> spells)
    {
        state.Spells.Clear();

        for (var i = 0; i < spells.Count; i++)
        {
            var sc = spells[i];

            var spellHandle = new StaticHandle
            {
                Index = i,
            };
            var spell = new Spell
            {
                Handle = spellHandle,
                Name = sc.Name,
                Category = Enum.Parse<SpellCategory>(sc.Category),
                SpawnEntity = EntityStringToHandle(sc.SpawnEntity),
                Cooldown = sc.Cooldown,
                AimLength = sc.AimLength,
                Damage = sc.Damage,
                Range = sc.Range,
                Duration = sc.Duration,
                TickCooldown = sc.TickCooldown,
                CastBegin = float.MinValue,
                TickBegin = float.MinValue,
            };
            state.Spells.Add(spell);
        }
    }

    private StaticHandle EntityStringToHandle(string entityType)
    {
        if (entityStringToHandle.TryGetValue(entityType, out var handle))
        {
            return handle;
        }

        return StaticHandle.InvalidHandle;
    }

    private TEnum ParseEnum<TEnum>(string str)
     where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(str, out var result))
        {
            return result;
        }

        return default;
    }
}