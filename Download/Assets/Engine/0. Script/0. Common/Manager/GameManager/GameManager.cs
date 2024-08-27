using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    private CameraManager m_cameraManager = null;
    private UIManager     m_uIManager     = null;
    private SoundManager  m_soundManager  = null;

    private string m_playerName = "ÀÌ¸§";

    private AudioSource m_audioSource;

    public static GameManager Instance => m_instance;
    public CameraManager Camera => m_cameraManager;
    public UIManager UI => m_uIManager;
    public SoundManager Sound => m_soundManager;

    public string PlayerName => m_playerName;

    public AudioSource AudioSource => m_audioSource;


    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            m_cameraManager = gameObject.AddComponent<CameraManager>();
            m_uIManager     = gameObject.AddComponent<UIManager>();
            m_soundManager  = gameObject.AddComponent<SoundManager>();
            gameObject.AddComponent<FPS>();

            m_audioSource = GetComponent<AudioSource>();

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

    public void Destroy_GameObject(ref GameObject gameObject)
    {
        Destroy(gameObject);
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


    public void Change_Scene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public RaycastHit Start_Raycast(Vector3 origin, Vector3 direction, float distance, int layerIndex)
    {
        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, distance, layerIndex);

#if UNITY_EDITOR
        Debug.DrawRay(origin, direction * distance, Color.red);
#endif

        return hit;
    }
}