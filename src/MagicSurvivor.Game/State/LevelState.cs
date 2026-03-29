namespace MagicSurvivor.Game.State;

public class LevelState
{
    public string CurrentLevelId = string.Empty;
    public bool ShouldLoadLevel;

    public StaticHandle CurrentWave = StaticHandle.InvalidHandle;
    public bool ShouldSpawnWave;
    public int EnemyCount;
}
