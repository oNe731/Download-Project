using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VisualNovel
{
    public class ShootCountStart : MonoBehaviour
    {
        [Header("GameObject")]
        [SerializeField] private GameObject m_count;

        [Header("Resource")]
        [SerializeField] private Sprite[] m_image;

        private bool m_click = false;
        private int m_Index = 0;
        private float m_updatTime = 1.0f;
        private float m_time = 0.0f;

        private GameObject m_methodObj;
        private Image m_countImage;

        private void Start()
        {
            m_countImage = m_count.GetComponent<Image>();

            // 방법창 생성
            m_methodObj = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Common/Panel_Method", transform.parent);
            if (m_methodObj == null) return;
            m_methodObj.GetComponent<Image>().sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/Method/Method_Shoot");
        }

        private void Update()
        {
            if (GameManager.Ins.IsGame == false)
                return;

            if (!m_click)
            {
                if (m_methodObj == null)
                {
                    m_click = true;
                    Start_Count();
                }
            }
            else
            {
                Update_Count();
            }

        }

        private void Start_Count()
        {
            m_count.SetActive(true);
            m_countImage.sprite = m_image[m_Index];
        }

        private void Update_Count()
        {
            // 3/ 2/ 1 카운트 다운 후 게임 진행
            m_time += Time.deltaTime;
            if (m_time >= m_updatTime)
            {
                m_time = 0f;
                m_Index++;
                if (m_Index > 2)
                    Finish_Count();
                else
                    m_countImage.sprite = m_image[m_Index];
            }
        }

        private void Finish_Count()
        {
            GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Day3Shoot>().Play_Level();
            Destroy(gameObject);
        }
    }
}

