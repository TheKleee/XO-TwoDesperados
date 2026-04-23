using DataXO;
using UnityEngine;

public static class SaveManager
{
    const string StatsKey = "stats";
    const string SettingsKey = "settings";

    public static StatsData LoadStats()
    {
        if (!PlayerPrefs.HasKey(StatsKey)) return new StatsData();
        return JsonUtility.FromJson<StatsData>(PlayerPrefs.GetString(StatsKey));
    }

    public static SettingsData LoadSettings()
    {
        if (!PlayerPrefs.HasKey(SettingsKey)) return new SettingsData();
        return JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString(SettingsKey));
    }

    public static void SaveStats(StatsData data)
    {
        PlayerPrefs.SetString(StatsKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save(); // critical on WebGL
    }

    public static void SaveSettings(SettingsData data)
    {
        PlayerPrefs.SetString(SettingsKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save(); // critical on WebGL
    }
}