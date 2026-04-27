using System;
using System.Globalization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SaveKey = "cosmic_crystal_factory_save_v1";

    public GameState Load()
    {
        string json = PlayerPrefs.GetString(SaveKey, string.Empty);

        if (string.IsNullOrWhiteSpace(json))
        {
            GameState freshState = new GameState();
            freshState.lastSavedUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
            return freshState;
        }

        try
        {
            GameState state = JsonUtility.FromJson<GameState>(json);
            return state ?? new GameState();
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"Could not load save data. Starting new save. {exception.Message}");
            return new GameState();
        }
    }

    public void Save(GameState state)
    {
        state.lastSavedUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
        string json = JsonUtility.ToJson(state);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }
}
