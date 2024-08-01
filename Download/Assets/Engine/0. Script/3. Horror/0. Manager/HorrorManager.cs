using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Horror;

public class HorrorManager : MonoBehaviour
{
    public enum LEVEL { LV_1STAGE, LV_2STAGE, LV_END };

    private static HorrorManager m_instance = null;
    private LevelController m_levelController = null;

    private bool m_isGame = false;
    private HorrorPlayer m_player;

    public static HorrorManager Instance => m_instance;
    public LevelController LevelController => m_levelController;
    public bool IsGame => m_isGame;
    public HorrorPlayer Player => m_player;

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;

        GameObject playergameObject = GameObject.FindWithTag("Player");
        if (playergameObject != null)
            m_player = playergameObject.GetComponent<HorrorPlayer>();

        m_levelController = gameObject.AddComponent<LevelController>();

        List<Level> levels = new List<Level>
        {
            gameObject.AddComponent<Horror_1stage>(),
            gameObject.AddComponent<Horror_2stage>(),
        };

        gameObject.GetComponent<Horror_1stage>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Horror_2stage>().Initialize_Level(m_levelController);

        m_levelController.Initialize_Level(levels, (int)LEVEL.LV_1STAGE);
    }

    private void Start()
    {
        GameManager.Instance.UI.Start_FadeIn(1f, Color.black, ()=> Start_Game());
    }

    private void Start_Game()
    {
        m_isGame = true;

        // 카메라 설정
        GameManager.Instance.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);

        CameraFollow camera = (CameraFollow)GameManager.Instance.Camera.Get_CurCamera();
        camera.Set_FollowInfo(GameObject.FindWithTag("Player").transform.GetChild(0).GetChild(0).transform, false, false, new Vector3(0.0f, 1.3f, 0.0f), 200.0f, 100.0f, new Vector2(-45f, 45f), true, true);
    }

    private  void Update()
    {
    }

    public GameObject Create_WorldHintUI(UIWorldHint.HINTTYPE hinttype, Transform target, Vector3 m_uiOffset)
    {
        GameObject gameObject = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_WorldHint");
        if (gameObject == null)
            return null;

        gameObject.GetComponent<UIWorldHint>().Initialize_UI(hinttype, target, m_uiOffset);
        return gameObject;
    }

    public void Set_Pause(bool pause)
    {
        m_isGame = !pause;
        
        GameManager.Instance.Camera.Set_CursorLock(!pause);
        Player.StateMachine.Lock = pause;
    }
}
