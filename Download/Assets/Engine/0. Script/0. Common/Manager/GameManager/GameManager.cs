using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;

    private string m_playerName = "";

    private List<StageManager> m_stages;
    private int m_curStage = -1;

    private bool m_isGame = false;

    private CameraManager   m_cameraManager   = null;
    private UIManager       m_uIManager       = null;
    private SoundManager    m_soundManager    = null;
    private SettingManager  m_settingManager  = null;
    private ResourceManager m_resourceManager = null;

    private AudioSource m_audioSource;

    public static GameManager Ins => m_instance;
    public string PlayerName { get => m_playerName; set => m_playerName = value; }
    public WindowManager Window => (WindowManager)m_stages[(int)StageManager.STAGE.LEVEL_WINDOW];
    public VisualNovelManager Novel => (VisualNovelManager)m_stages[(int)StageManager.STAGE.LEVEL_VISUALNOVEL];
    public WesternManager Western => (WesternManager)m_stages[(int)StageManager.STAGE.LEVEL_WESTERN];
    public HorrorManager Horror => (HorrorManager)m_stages[(int)StageManager.STAGE.LEVEL_HORROR];
    public StageManager CurStage => m_stages[m_curStage];
    public bool IsGame { get => m_isGame; set => m_isGame = value; }
    public CameraManager Camera => m_cameraManager;
    public UIManager UI => m_uIManager;
    public SoundManager Sound => m_soundManager;
    public SettingManager Setting => m_settingManager;
    public ResourceManager Resource => m_resourceManager;
    public AudioSource AudioSource => m_audioSource;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            m_cameraManager   = gameObject.AddComponent<CameraManager>();
            m_uIManager       = gameObject.AddComponent<UIManager>();
            m_soundManager    = gameObject.AddComponent<SoundManager>();
            m_settingManager  = gameObject.AddComponent<SettingManager>();
            m_resourceManager = gameObject.AddComponent<ResourceManager>();
            gameObject.AddComponent<FPS>();

            m_audioSource = GetComponent<AudioSource>();

            // ¸Å´ÏÀú
            m_stages = new List<StageManager>();
            m_stages.Add(new LoadingManager());
            m_stages.Add(new LoginManager());
            m_stages.Add(new WindowManager());
            m_stages.Add(new VisualNovelManager());
            m_stages.Add(new WesternManager());
            m_stages.Add(new HorrorManager());

            Scene currentScene = SceneManager.GetActiveScene();
            for(int i = 0; i < m_stages.Count; ++i)
            {
                if(m_stages[i].SceneName == currentScene.name)
                {
                    Change_Scene(m_stages[i].StageLevel);
                    break;
                }
            }

            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (m_curStage == -1)
            return;

        m_stages[(int)m_curStage].Update_Stage();
    }

    private void LateUpdate()
    {
        if (m_curStage == -1)
            return;

        m_stages[(int)m_curStage].LateUpdate_Stage();
    }

    public void Change_Scene(StageManager.STAGE stage)
    {
        if (m_curStage != -1)
            m_stages[(int)m_curStage].Exit_Stage();

        m_curStage = (int)stage;
        m_stages[(int)m_curStage].Enter_Stage();
    }

    public void Load_Scene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public virtual void Set_Pause(bool pause, bool Setcursur = true)
    {
        if (m_curStage == -1)
            return;

        m_stages[(int)m_curStage].Set_Pause(pause, Setcursur);
    }

    public bool Get_AnyKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == false)
        {
            if (Input.anyKeyDown)
            {
                if(Setting.IsOpen == false)
                    return true;
            }
        }

        return false;
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

    public void Save_JsonData<T>(string filePath, List<T> saveData)
    {
        var Result = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(filePath, Result);
    }

    public List<T> Load_JsonData<T>(string filePath)
    {
        TextAsset jsonAsset = Resource.Load<TextAsset>(filePath);

        if (jsonAsset != null)
            return JsonConvert.DeserializeObject<List<T>>(jsonAsset.text);
        else
            Debug.LogError($"Failed to load Jsondata : {filePath}");

        return null;
    }

    public void End_Game()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}