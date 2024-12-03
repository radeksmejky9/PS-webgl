using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log($"Attempting to access {typeof(T)} instance...");
                _instance = FindExistingInstance() ?? CreateNewInstance();
            }
            Debug.Log($"Accessing {typeof(T)} instance: {_instance.name}");
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private static T FindExistingInstance()
    {
        T[] existingInstances = FindObjectsByType<T>(FindObjectsSortMode.None);

        if (existingInstances == null || existingInstances.Length == 0) return null;

        if (existingInstances.Length > 1)
        {
            Debug.LogError($"Multiple instances of {typeof(T)} detected. Using the first one found: {existingInstances[0].name}");
        }

        return existingInstances[0];
    }

    private static T CreateNewInstance()
    {
        var containerGO = new GameObject("__" + typeof(T).Name + " (Singleton)");
        return containerGO.AddComponent<T>();
    }

    /*private static T CreateNewInstance()
    {
        Debug.LogError($"No instance of {typeof(T)} exists in the scene, and dynamic creation is not allowed.");
        return null;
    }*/
}
