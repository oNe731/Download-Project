using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRIGERTYPE { TG_YANDERASTART, TG_END };

public class Trigerbox : MonoBehaviour
{
    [SerializeField] private TRIGERTYPE m_type = TRIGERTYPE.TG_END;

    private void OnTriggerEnter(Collider other)
    {
        switch(m_type)
        {
            case TRIGERTYPE.TG_YANDERASTART:
                TG_YANDERASTART(other);
                break;
        }
    }

    private void TG_YANDERASTART(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 달리면서 페이드 아웃
            UIManager.Instance.Start_FadeOut(1f, Color.black, () => VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Chase>().Create_Monster(), 1f, false);
            Destroy(gameObject);
        }
    }
}
