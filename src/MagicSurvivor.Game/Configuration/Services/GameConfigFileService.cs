using System.Text.Json;

namespace MagicSurvivor.Game.Configuration.Services;

public class GameConfigFileService
{
    private readonly bool shouldLoadFromSourceDir;
    public GameConfigFileService(bool shouldLoadFromSourceDir)
    {
        this.shouldLoadFromSourceDir = shouldLoadFromSourceDir;
    }

    public GameConfig ReadGameConfig()
    {
        var basePath = GetConfigBasePath();
        var gameConfigPath = Path.Combine(basePath, "game.json");
        return ParseJsonAtPath<GameConfig>(gameConfigPath);
    }

    public LevelConfig ReadLevelConfig(string levelId)
    {
        var basePath = GetConfigBasePath();
        var levelConfigPath = Path.Combine(basePath, "levels", $"{levelId}.json");
        return ParseJsonAtPath<LevelConfig>(levelConfigPath);
    }

    private string GetConfigBasePath()
    {
        string path;
        if (shouldLoadFromSourceDir)
        {
            path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "config"));
        }
        else
        {
            path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "config"));
        }

        return path;
    }

    private TConfig ParseJsonAtPath<TConfig>(string path)
    {
        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<TConfig>(json, new JsonSerializerOptions
        {
            IncludeFields = true,
        });
        return config!;
    }
}