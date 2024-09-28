using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public abstract class StageManager
{
    public enum STAGE { LEVEL_LOADING, LEVEL_LOGIN, LEVEL_WINDOW, LEVEL_VISUALNOVEL, LEVEL_WESTERN, LEVEL_HORROR, LEVEL_END };

    protected STAGE  m_stageLevel = STAGE.LEVEL_END;
    protected string m_sceneName;
    protected bool   m_isVisit = false;

    protected LevelController m_levelController = null;


    public STAGE StageLevel => m_stageLevel;
    public string SceneName => m_sceneName;
    public LevelController LevelController => m_levelController;


    public StageManager()
    {
        Load_Resource();
    }

    protected abstract void Load_Resource();
    

    public virtual void Enter_Stage()
    {
        GameManager.Ins.Load_Scene(m_sceneName);
        GameManager.Ins.StartCoroutine(Load_SceneAndRun());
    }

    protected IEnumerator Load_SceneAndRun()
    {
        // 비동기로 씬을 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_sceneName);

        // 씬이 완전히 로드될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 씬 전환 시 초기화 셋팅
        GameManager.Ins.Setting.Active_Panel(false);

        Load_Scene();
    }

    protected abstract void Load_Scene();

    protected virtual void In_Game()
    {
        GameManager.Ins.IsGame = true;
    }

    public virtual void Update_Stage()
    {
    }

    public virtual void LateUpdate_Stage()
    {
    }

    public virtual void Exit_Stage()
    {
        GameManager.Ins.IsGame = false;
        GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_END);

        m_levelController = null;
    }

    public virtual void Set_Pause(bool pause, bool Setcursur)
    {
        GameManager.Ins.IsGame = !pause;

        if (Setcursur == true)
            GameManager.Ins.Camera.Set_CursorLock(GameManager.Ins.IsGame);
    }
}
