using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private void Start()
    {
    }

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    public GameObject LoadCreate(string path, Transform transform = null)
    {
        GameObject prefab = Load<GameObject>(path);      
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Instantiate(prefab, transform);
    }

    public GameObject LoadCreate(string path, Vector3 position, Quaternion quaternion, Transform transform = null)
    {
        GameObject prefab = Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Instantiate(prefab, position, quaternion, transform);
    }

    public GameObject Create(GameObject gameObject, Transform transform = null)
    {
        return Instantiate(gameObject, transform);
    }

    public GameObject Create(GameObject gameObject, Vector3 position, Quaternion quaternion, Transform transform = null)
    {
        return Instantiate(gameObject, position, quaternion, transform);
    }


    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        Object.Destroy(gameObject);
    }
}