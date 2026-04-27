public static class JsonUtility
{
    public static T FromJson<T>(string json)
    {
        return UnityEngine.JsonUtility.FromJson<T>(json);
    }

    public static string ToJson(object value)
    {
        return UnityEngine.JsonUtility.ToJson(value);
    }

    public static string ToJson(object value, bool prettyPrint)
    {
        return UnityEngine.JsonUtility.ToJson(value, prettyPrint);
    }
}