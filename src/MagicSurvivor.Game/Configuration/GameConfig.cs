namespace MagicSurvivor.Game.Configuration;

public class GameConfig
{
    public CameraConfig Camera = new();
    public List<EntityDefinitionConfig> EntityDefinitions = [];
    public List<SpellConfig> Spells = [];
}
