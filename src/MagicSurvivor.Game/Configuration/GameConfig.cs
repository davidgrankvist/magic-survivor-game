namespace MagicSurvivor.Game.Configuration;

/// <summary>
/// Settings that are loaded from game.json.
/// </summary>
public class GameConfig
{
    public CameraConfig Camera = new();
    public List<EntityDefinitionConfig> EntityDefinitions = [];
    public List<SpellConfig> Spells = [];
    public WaveSettingsConfig WaveSettings = new();
}
