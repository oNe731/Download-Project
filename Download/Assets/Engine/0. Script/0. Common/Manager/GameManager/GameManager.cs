using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    private CameraManager m_cameraManager = null;
    private UIManager     m_uIManager     = null;
    private SoundManager  m_soundManager  = null;

    public static GameManager Instance => m_instance;
    public CameraManager Camera => m_cameraManager;
    public UIManager UI => m_uIManager;
    public SoundManager m_Sound => m_soundManager;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            m_cameraManager = gameObject.AddComponent<CameraManager>();
            m_uIManager     = gameObject.AddComponent<UIManager>();
            m_soundManager  = gameObject.AddComponent<SoundManager>();
            gameObject.AddComponent<FPS>();

            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
    }


public GameObject Create_GameObject(string path, Transform transform = null)
    {
        return Instantiate(Resources.Load<GameObject>(path), transform);
    }


    public void Save_JsonData<T>(string filePath, List<T> saveData)
    {
        var Result = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(filePath, Result);
    }

    public List<T> Load_JsonData<T>(string filePath)
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(filePath);

        if (jsonAsset != null)
            return JsonConvert.DeserializeObject<List<T>>(jsonAsset.text);
        else
            Debug.LogError($"Failed to load Jsondata : {filePath}");

        return null;
    }
}