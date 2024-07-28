using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorrorManager : MonoBehaviour
{
    private static HorrorManager m_instance = null;
    private LevelController m_levelController = null;

    public static HorrorManager Instance => m_instance;
    public LevelController LevelController => m_levelController;

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;

        m_levelController = gameObject.AddComponent<LevelController>();

        //List<Level> levels = new List<Level>
        //{
        //};

        //m_levelController.Initialize_Level(levels);
    }

    private void Start()
    {
        Start_Game();
        GameManager.Instance.UI.Start_FadeIn(1f, Color.black);
    }

    private void Start_Game()
    {
        // 카메라 설정
        GameManager.Instance.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
        CameraFollow camera = (CameraFollow)GameManager.Instance.Camera.Get_CurCamera();
        camera.Set_FollowInfo(GameObject.FindWithTag("Player").transform.GetChild(0).GetChild(0).transform, false, false, new Vector3(0.0f, 1.3f, 0.0f), 200.0f, 100.0f, new Vector2(-45f, 45f), true, true);
    }

    private  void Update()
    {
    }
}
