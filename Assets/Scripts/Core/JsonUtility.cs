using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public static class JsonUtility
{
    public static T FromJson<T>(string json)
    {
        Type targetType = typeof(T);

        if (targetType == typeof(GameState))
        {
            return (T)(object)ParseGameState(json);
        }

        if (targetType == typeof(UpgradeCatalog))
        {
            return (T)(object)ParseUpgradeCatalog(json);
        }

        if (targetType == typeof(MissionCatalog))
        {
            return (T)(object)ParseMissionCatalog(json);
        }

        if (targetType == typeof(StageCatalog))
        {
            return (T)(object)ParseStageCatalog(json);
        }

        throw new NotSupportedException("JsonUtility does not support type " + targetType.Name);
    }

    public static string ToJson(object value)
    {
        return ToJson(value, false);
    }

    public static string ToJson(object value, bool prettyPrint)
    {
        if (value is GameState gameState)
        {
            return SerializeGameState(gameState, prettyPrint);
        }

        throw new NotSupportedException("JsonUtility does not support type " + value.GetType().Name);
    }

    private static GameState ParseGameState(string json)
    {
        GameState state = new GameState
        {
            coins = GetDouble(json, "coins", 0),
            totalCoinsEarned = GetDouble(json, "totalCoinsEarned", 0),
            clickPower = GetDouble(json, "clickPower", 1),
            coinsPerSecond = GetDouble(json, "coinsPerSecond", 0),
            globalMultiplier = GetDouble(json, "globalMultiplier", 1),
            dailyRewardStreak = GetInt(json, "dailyRewardStreak", 0),
            lastDailyRewardUtc = GetString(json, "lastDailyRewardUtc", null),
            lastSavedUtc = GetString(json, "lastSavedUtc", null),
            upgrades = new List<UpgradeSaveData>(),
            missions = new List<MissionSaveData>()
        };

        foreach (string item in GetObjectsFromArray(json, "upgrades"))
        {
            state.upgrades.Add(new UpgradeSaveData
            {
                upgradeId = GetString(item, "upgradeId", string.Empty),
                level = GetInt(item, "level", 0)
            });
        }

        foreach (string item in GetObjectsFromArray(json, "missions"))
        {
            state.missions.Add(new MissionSaveData
            {
                missionId = GetString(item, "missionId", string.Empty),
                progress = GetDouble(item, "progress", 0),
                completed = GetBool(item, "completed", false),
                claimed = GetBool(item, "claimed", false)
            });
        }

        return state;
    }

    private static UpgradeCatalog ParseUpgradeCatalog(string json)
    {
        List<UpgradeDefinition> upgrades = new List<UpgradeDefinition>();

        foreach (string item in GetObjectsFromArray(json, "upgrades"))
        {
            upgrades.Add(new UpgradeDefinition
            {
                id = GetString(item, "id", string.Empty),
                displayName = GetString(item, "displayName", string.Empty),
                description = GetString(item, "description", string.Empty),
                type = GetString(item, "type", string.Empty),
                baseCost = GetDouble(item, "baseCost", 0),
                costGrowth = GetDouble(item, "costGrowth", 1),
                clickPowerBonus = GetDouble(item, "clickPowerBonus", 0),
                coinsPerSecondBonus = GetDouble(item, "coinsPerSecondBonus", 0),
                globalMultiplierBonus = GetDouble(item, "globalMultiplierBonus", 0),
                unlockAtTotalCoins = GetInt(item, "unlockAtTotalCoins", 0),
                iconResource = GetString(item, "iconResource", string.Empty)
            });
        }

        return new UpgradeCatalog { upgrades = upgrades.ToArray() };
    }

    private static MissionCatalog ParseMissionCatalog(string json)
    {
        List<MissionDefinition> missions = new List<MissionDefinition>();

        foreach (string item in GetObjectsFromArray(json, "missions"))
        {
            missions.Add(new MissionDefinition
            {
                id = GetString(item, "id", string.Empty),
                displayName = GetString(item, "displayName", string.Empty),
                description = GetString(item, "description", string.Empty),
                type = (MissionType)GetInt(item, "type", 0),
                target = GetDouble(item, "target", 0),
                rewardCoins = GetDouble(item, "rewardCoins", 0)
            });
        }

        return new MissionCatalog { missions = missions.ToArray() };
    }

    private static StageCatalog ParseStageCatalog(string json)
    {
        List<StageDefinition> stages = new List<StageDefinition>();

        foreach (string item in GetObjectsFromArray(json, "stages"))
        {
            stages.Add(new StageDefinition
            {
                id = GetString(item, "id", string.Empty),
                displayName = GetString(item, "displayName", string.Empty),
                lore = GetString(item, "lore", string.Empty),
                unlockAtTotalCoins = GetDouble(item, "unlockAtTotalCoins", 0),
                visualMood = GetString(item, "visualMood", string.Empty),
                primaryColor = GetString(item, "primaryColor", "#101827"),
                accentColor = GetString(item, "accentColor", "#26D9FF"),
                backgroundResource = GetString(item, "backgroundResource", string.Empty)
            });
        }

        return new StageCatalog { stages = stages.ToArray() };
    }

    private static string SerializeGameState(GameState state, bool prettyPrint)
    {
        string indent = prettyPrint ? "  " : string.Empty;
        string newline = prettyPrint ? "\n" : string.Empty;
        string separator = prettyPrint ? ": " : ":";
        StringBuilder builder = new StringBuilder();

        builder.Append("{").Append(newline);
        AppendNumber(builder, "coins", state.coins, indent, separator, true, newline);
        AppendNumber(builder, "totalCoinsEarned", state.totalCoinsEarned, indent, separator, true, newline);
        AppendNumber(builder, "clickPower", state.clickPower, indent, separator, true, newline);
        AppendNumber(builder, "coinsPerSecond", state.coinsPerSecond, indent, separator, true, newline);
        AppendNumber(builder, "globalMultiplier", state.globalMultiplier, indent, separator, true, newline);
        AppendNumber(builder, "dailyRewardStreak", state.dailyRewardStreak, indent, separator, true, newline);
        AppendString(builder, "lastDailyRewardUtc", state.lastDailyRewardUtc, indent, separator, true, newline);
        AppendString(builder, "lastSavedUtc", state.lastSavedUtc, indent, separator, true, newline);

        builder.Append(indent).Append("\"upgrades\"").Append(separator).Append("[");
        for (int index = 0; index < state.upgrades.Count; index++)
        {
            UpgradeSaveData upgrade = state.upgrades[index];
            if (index > 0) builder.Append(",");
            builder.Append("{\"upgradeId\":\"").Append(Escape(upgrade.upgradeId)).Append("\",\"level\":")
                .Append(upgrade.level.ToString(CultureInfo.InvariantCulture)).Append("}");
        }
        builder.Append("],").Append(newline);

        builder.Append(indent).Append("\"missions\"").Append(separator).Append("[");
        for (int index = 0; index < state.missions.Count; index++)
        {
            MissionSaveData mission = state.missions[index];
            if (index > 0) builder.Append(",");
            builder.Append("{\"missionId\":\"").Append(Escape(mission.missionId)).Append("\",\"progress\":")
                .Append(mission.progress.ToString(CultureInfo.InvariantCulture)).Append(",\"completed\":")
                .Append(mission.completed ? "true" : "false").Append(",\"claimed\":")
                .Append(mission.claimed ? "true" : "false").Append("}");
        }
        builder.Append("]").Append(newline);
        builder.Append("}");

        return builder.ToString();
    }

    private static IEnumerable<string> GetObjectsFromArray(string json, string key)
    {
        string array = GetArrayContent(json, key);
        if (string.IsNullOrEmpty(array)) yield break;

        MatchCollection matches = Regex.Matches(array, "\\{[\\s\\S]*?\\}");
        foreach (Match match in matches)
        {
            yield return match.Value;
        }
    }

    private static string GetArrayContent(string json, string key)
    {
        Match keyMatch = Regex.Match(json, "\\\"" + Regex.Escape(key) + "\\\"\\s*:\\s*\\[");
        if (!keyMatch.Success) return string.Empty;

        int start = keyMatch.Index + keyMatch.Length;
        int depth = 1;

        for (int index = start; index < json.Length; index++)
        {
            if (json[index] == '[') depth++;
            if (json[index] == ']') depth--;
            if (depth == 0) return json.Substring(start, index - start);
        }

        return string.Empty;
    }

    private static string GetString(string json, string key, string defaultValue)
    {
        Match nullMatch = Regex.Match(json, "\\\"" + Regex.Escape(key) + "\\\"\\s*:\\s*null");
        if (nullMatch.Success) return defaultValue;

        Match match = Regex.Match(json, "\\\"" + Regex.Escape(key) + "\\\"\\s*:\\s*\\\"((?:\\\\.|[^\\\"])*)\\\"");
        return match.Success ? Unescape(match.Groups[1].Value) : defaultValue;
    }

    private static double GetDouble(string json, string key, double defaultValue)
    {
        Match match = Regex.Match(json, "\\\"" + Regex.Escape(key) + "\\\"\\s*:\\s*([-+0-9.eE]+)");
        return match.Success && double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double value) ? value : defaultValue;
    }

    private static int GetInt(string json, string key, int defaultValue)
    {
        Match match = Regex.Match(json, "\\\"" + Regex.Escape(key) + "\\\"\\s*:\\s*([-+0-9]+)");
        return match.Success && int.TryParse(match.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value) ? value : defaultValue;
    }

    private static bool GetBool(string json, string key, bool defaultValue)
    {
        Match match = Regex.Match(json, "\\\"" + Regex.Escape(key) + "\\\"\\s*:\\s*(true|false)");
        return match.Success ? match.Groups[1].Value == "true" : defaultValue;
    }

    private static void AppendNumber(StringBuilder builder, string key, double value, string indent, string separator, bool comma, string newline)
    {
        builder.Append(indent).Append("\"").Append(key).Append("\"").Append(separator).Append(value.ToString(CultureInfo.InvariantCulture));
        if (comma) builder.Append(",");
        builder.Append(newline);
    }

    private static void AppendString(StringBuilder builder, string key, string value, string indent, string separator, bool comma, string newline)
    {
        builder.Append(indent).Append("\"").Append(key).Append("\"").Append(separator);
        builder.Append(value == null ? "null" : "\"" + Escape(value) + "\"");
        if (comma) builder.Append(",");
        builder.Append(newline);
    }

    private static string Escape(string value)
    {
        return string.IsNullOrEmpty(value) ? string.Empty : value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
    }

    private static string Unescape(string value)
    {
        return string.IsNullOrEmpty(value) ? string.Empty : value.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\\"", "\"").Replace("\\\\", "\\");
    }
}