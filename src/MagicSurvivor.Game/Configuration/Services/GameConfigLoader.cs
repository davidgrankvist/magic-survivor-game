using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Configuration.Services;

public class GameConfigLoader
{
    public GameConfigLoader(bool loadFromSourceDir)
    {
        configFileService = new(loadFromSourceDir);
    }
    private readonly GameConfigFileService configFileService;
    private readonly Dictionary<string, int> entityTypeStringToIndexMap = [];
    private const string PlayerEntityType = "Player";

    public void LoadGameConfig(GameState state)
    {
        entityTypeStringToIndexMap.Clear();

        var gameConfig = configFileService.ReadGameConfig();
        LoadCameraConfig(state.Camera, gameConfig.Camera);
        LoadEntityDefinitions(state, gameConfig.EntityDefinitions);
        LoadSpells(state, gameConfig.Spells);

        state.PlayerEntityDefinitionHandle = new StaticHandle
        {
            Index = entityTypeStringToIndexMap[PlayerEntityType],
        };

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
            Index = entityTypeStringToIndexMap[PlayerEntityType],
            Generation = 0,
        };
    }

    private void LoadEntitites(GameState state, List<EntityConfig> entities)
    {
        state.Entities.Clear();
        for (var i = 0; i < entities.Count; i++)
        {
            var ec = entities[i];
            var definitionHandle = new StaticHandle
            {
                Index = entityTypeStringToIndexMap[ec.Type],
            };
            var entity = new Entity
            {
                DefinitionHandle = definitionHandle,
                Position = ec.Position,
            };
            state.Entities.AddEntity(entity);
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
            entityTypeStringToIndexMap[edc.Type] = i;

            var ed = new EntityDefinition
            {
                Category = Enum.Parse<EntityCategory>(edc.Category),
                Speed = edc.Speed,
                Collider = edc.Collider,
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

            var definitionHandle = new StaticHandle
            {
                Index = entityTypeStringToIndexMap[sc.SpawnEntity],
            };
            var spell = new Spell
            {
                Category = Enum.Parse<SpelLCategory>(sc.Category),
                SpawnEntity = definitionHandle,
                Cooldown = sc.Cooldown,
                Elapsed = sc.Cooldown + 1,
                AimLength = sc.AimLength,
            };
            state.Spells.Add(spell);
        }
    }
}