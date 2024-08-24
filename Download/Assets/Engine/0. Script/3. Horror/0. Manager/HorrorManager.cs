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
    private GameObject m_popupUI = null;
    private GameObject m_instructionUI = null;
    private Dictionary<string, Sprite> m_noteElementIcon = new Dictionary<string, Sprite>();

    public static HorrorManager Instance => m_instance;
    public LevelController LevelController => m_levelController;
    public bool IsGame => m_isGame;
    public HorrorPlayer Player => m_player;
    public Dictionary<string, Sprite> NoteElementIcon => m_noteElementIcon;

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

        m_noteElementIcon.Add("Icon_Stick", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Stick"));
        m_noteElementIcon.Add("Icon_Gun", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Gun"));
        m_noteElementIcon.Add("Icon_Flashlight", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Flashlight"));
        m_noteElementIcon.Add("Icon_Bullet", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Bullet"));
        m_noteElementIcon.Add("Icon_Medicine", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_Medicine"));
        m_noteElementIcon.Add("Icon_clue", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_clue"));

        m_noteElementIcon.Add("Icon_None", Resources.Load<Sprite>("1. Graphic/2D/3. Horror/UI/Play/UI_horror_Item/Icon_None"));
    }

    private void Start()
    {
        GameManager.Instance.UI.Start_FadeIn(1f, Color.black, ()=> Start_Game());
    }

    private void Start_Game()
    {
        m_isGame = true;

        // 카메라 설정
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            return;
        GameManager.Instance.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
        CameraFollow camera = (CameraFollow)GameManager.Instance.Camera.Get_CurCamera();
        camera.Set_FollowInfo(player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(1).transform, player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).transform, false, false, new Vector3(0.0f, 1.3f, 0.0f), 200.0f, 100.0f, new Vector2(-45f, 45f), true, true);
    }

    public void Set_Pause(bool pause)
    {
        m_isGame = !pause;

        GameManager.Instance.Camera.Set_CursorLock(!pause);
        Player.StateMachine.Lock = pause;
    }

    public GameObject Create_WorldHintUI(UIWorldHint.HINTTYPE hinttype, Transform target, Vector3 m_uiOffset)
    {
        GameObject gameObject = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_WorldHint", LevelController.Get_CurrentLevel<Horror_Base>().Stage.transform);
        if (gameObject == null)
            return null;

        gameObject.GetComponent<UIWorldHint>().Initialize_UI(hinttype, target, m_uiOffset);
        return gameObject;
    }

    public void Active_Popup(UIPopup.TYPE type, NoteItem itemType)
    {
        if (m_popupUI == null)
            m_popupUI = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
        if (m_popupUI == null)
            return;

        m_popupUI.GetComponent<UIPopup>().Initialize_UI(type, itemType);
    }

    public void Active_InstructionUI(string text) // 안내 문구 출력
    {
        if (m_instructionUI == null)
            m_instructionUI = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Instruction", GameObject.Find("Canvas").transform.GetChild(2));
        if (m_instructionUI == null)
            return;

        UIInstruction.Expendables info = new UIInstruction.Expendables();
        info.text = text;
        m_instructionUI.GetComponent<UIInstruction>().Initialize_UI(info);
    }
}
