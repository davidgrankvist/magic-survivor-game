using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Configuration.Services;

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

        state.PlayerEntityHandle = new EntityHandle
        {
            Index = EntityStringToHandle(PlayerEntityType).Index,
            Generation = 0,
        };
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
            };
            state.Entities.Add(entity);
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
     where TEnum: struct, Enum
    {
        if (Enum.TryParse<TEnum>(str, out var result))
        {
            return result;
        }

        return default;
    }
}