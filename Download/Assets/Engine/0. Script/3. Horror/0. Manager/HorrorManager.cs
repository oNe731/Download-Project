using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorrorManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_cctvbuttons;
    [SerializeField] private Sprite[] m_buttonSprite;
    [SerializeField] private GameObject m_background;
    [SerializeField] private Sprite[] m_backgroundSprite;
    [SerializeField] private GameObject m_ReportPanel;
    [SerializeField] private GameObject m_HorrorPanel;

    private static HorrorManager m_instance = null;
    private LevelController m_levelController = null;

    private int m_currentCctv = 0;

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
        Change_CCTV(m_currentCctv);
        GameManager.Instance.UI.Start_FadeIn(1f, Color.black);
    }

    private  void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            m_ReportPanel.SetActive(!m_ReportPanel.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            m_HorrorPanel.SetActive(!m_HorrorPanel.activeSelf);
        }
    }

    public void Change_CCTV(int index)
    {
        m_currentCctv = index;
        for (int i = 0; i < m_cctvbuttons.Length; ++i)
        {
            if (i == m_currentCctv)
                m_cctvbuttons[i].GetComponent<Image>().sprite = m_buttonSprite[0]; // On
            else
                m_cctvbuttons[i].GetComponent<Image>().sprite = m_buttonSprite[1]; // Off
        }

        m_background.GetComponent<Image>().sprite = m_backgroundSprite[m_currentCctv];
    }

    public void Button_Report()
    {
        m_ReportPanel.SetActive(true);
    }
}
