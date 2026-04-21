using System.IO;
using UnityEngine;
using DataXO;

public static class SaveManager
{
    static readonly string StatsPath = Application.persistentDataPath + "/stats.json";
    static readonly string SettingsPath = Application.persistentDataPath + "/settings.json";

    public static StatsData LoadStats() => Load<StatsData>(StatsPath) ?? new StatsData();
    public static SettingsData LoadSettings() => Load<SettingsData>(SettingsPath) ?? new SettingsData();

    public static void SaveStats(StatsData data) => Save(StatsPath, data);
    public static void SaveSettings(SettingsData data) => Save(SettingsPath, data);

    static void Save<T>(string path, T data) =>
        File.WriteAllText(path, JsonUtility.ToJson(data, prettyPrint: true));

    static T Load<T>(string path) where T : class
    {
        if (!File.Exists(path)) return null;
        return JsonUtility.FromJson<T>(File.ReadAllText(path));
    }
}