using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Configuration.Services;

public class GameConfigLoader
{
    public GameConfigLoader(bool loadFromSourceDir)
    {
        configFileService = new(loadFromSourceDir);
    }
    private readonly GameConfigFileService configFileService;
    private readonly Dictionary<string, int> stringIdToIndexMap = [];
    private const string PlayerEntityType = "ENTITY_TYPE_PLAYER";
    
    public void LoadGameConfig(GameState state)
    {
        stringIdToIndexMap.Clear();

        var gameConfig = configFileService.ReadGameConfig();
        LoadCameraConfig(state.Camera, gameConfig.Camera);
        LoadEntityDefinitions(state, gameConfig.EntityDefinitions);

        state.PlayerEntityDefinitionHandle = new StaticHandle
        {
            Index = stringIdToIndexMap[PlayerEntityType],
        };
    }

    public void LoadLevel(GameState state, string levelId)
    {
        state.Camera.Reset();

        var levelConfig = configFileService.ReadLevelConfig(levelId);
        LoadEntitites(state, levelConfig.Entities);

        state.PlayerEntityHandle = new EntityHandle
        {
            Index = stringIdToIndexMap[PlayerEntityType],
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
                Index = stringIdToIndexMap[ec.Type],
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
            stringIdToIndexMap[edc.Type] = i;
            
            var ed = new EntityDefinition
            {
                Speed = edc.Speed,
                Collider = edc.Collider,
            };
            state.EntityDefinitions.Add(ed);
        }
    }
}