using System;
using UnityEngine;

namespace Styles.Common
{
public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private bool _dontDestroyOnLoad;
    
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance)
                return _instance;
            
            Debug.LogError("singleton instance does not exists!");
            return null;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            return;
        }

        _instance = GetComponent<T>();
        
        if(_dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
}