namespace MagicSurvivor.Game.Configuration;

/// <summary>
/// Settings that are loaded from a level json.
/// </summary>
public class LevelConfig
{
    public List<EntityConfig> Entities = [];
    public List<WaveConfig> Waves = [];
}
