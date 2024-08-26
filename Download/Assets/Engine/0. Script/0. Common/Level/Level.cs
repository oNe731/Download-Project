using UnityEngine;

public class Level : MonoBehaviour
{
    protected int m_levelIndex = -1;
    protected bool m_IsVisit = false;

    protected LevelController m_levelController;

    public int LevelIndex => m_levelIndex;

    public virtual void Initialize_Level(LevelController levelController)
    {
        m_levelController = levelController;
    }

    public virtual void Enter_Level()
    {
    }

    public virtual void Play_Level()
    {
    }

    public virtual void Update_Level()
    {
    }

    public virtual void LateUpdate_Level()
    {
    }

    public virtual void Exit_Level()
    {
    }

    public virtual void OnDrawGizmos()
    {
    }
}
