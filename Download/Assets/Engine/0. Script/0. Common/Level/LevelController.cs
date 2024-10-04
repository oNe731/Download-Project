using System.Collections.Generic;

public class LevelController
{
    private int m_curlevel = -1;
    private int m_prelevel = -1;
    private List<Level> m_levels;

    public int Curlevel { get => m_curlevel; }
    public int Prelevel { get => m_prelevel; }
    public List<Level> Levels { get => m_levels; }

    public void Initialize_Level(List<Level> levels, int startlevels = -1)
    {
        m_levels = levels;
        Change_Level(startlevels);
    }

    public void Update_Level()
    {
        if (m_curlevel == -1)
            return;

        m_levels[(int)m_curlevel].Update_Level();
    }

    public void LateUpdate_Level()
    {
        if (m_curlevel == -1)
            return;

        m_levels[(int)m_curlevel].LateUpdate_Level();
    }
    public void Change_Level(int levelIndex, bool sameLevelUse = false)
    {
        if (levelIndex == -1 || m_curlevel == levelIndex && sameLevelUse == false)
            return;

        if (m_curlevel != -1)
            m_levels[(int)m_curlevel].Exit_Level();

        m_prelevel = m_curlevel;
        m_curlevel = levelIndex;

        m_levels[(int)m_curlevel].Enter_Level();
    }

    public void Change_NextLevel()
    {
        int nextlevel = m_curlevel + 1;
        if (nextlevel >= m_levels.Count)
            return;

        Change_Level(nextlevel);
    }

    public void OnDrawGizmos_Level()
    {
        if (m_curlevel == -1)
            return;

        m_levels[(int)m_curlevel].OnDrawGizmos();
    }

    public T Get_Level<T>(int level) where T : Level
    {
        return m_levels[(int)level] as T;
    }

    public T Get_CurrentLevel<T>() where T : Level
    {
        if (m_curlevel == -1)
            return null;

        return m_levels[(int)m_curlevel] as T;
    }

    public T Get_PreviousLevel<T>() where T : Level
    {
        if (m_prelevel == -1)
            return null;

        return m_levels[(int)m_prelevel] as T;
    }
}
