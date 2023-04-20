using UnityEngine;

namespace Styles.Common
{
public static class LogExtensions
{
    public static void Log(this GameObject gameObject, string message)
    {
        Debug.Log(message, gameObject);
    }
    
    public static void LogWarning(this GameObject gameObject, string message)
    {
        Debug.LogWarning(message, gameObject);
    }

    public static void LogError(this GameObject gameObject, string message)
    {
        Debug.LogError(message, gameObject);
    }
    
    public static void Log(this Transform transform, string message)
    {
        Debug.Log(message, transform);
    }
    
    public static void LogWarning(this Transform transform, string message)
    {
        Debug.LogWarning(message, transform);
    }
    
    public static void LogError(this Transform transform, string message)
    {
        Debug.LogError(message, transform);
    }
}
}