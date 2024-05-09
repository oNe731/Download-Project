using UnityEngine;

public class Level : MonoBehaviour
{
    protected LevelController m_levelController;

    public Level(LevelController levelController)
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
