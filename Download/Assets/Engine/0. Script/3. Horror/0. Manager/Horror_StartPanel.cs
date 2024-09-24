using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horror
{
    public class Horror_StartPanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_font;

        private void Start()
        {
            StartCoroutine(Active_Font());
        }

        private IEnumerator Active_Font()
        {
            while (true)
            {
                m_font.SetActive(true);
                yield return new WaitForSeconds(1f);

                m_font.SetActive(false);
                yield return new WaitForSeconds(1f);
            }
        }

        private void Update()
        {
            if (GameManager.Ins.Get_AnyKeyDown())
                Button_Start();
        }

        private void Button_Start()
        {
            GameManager.Ins.UI.EventUpdate = true;
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Start_Game(), 1f, false);
        }

        public void Start_Game()
        {
            Destroy(gameObject);
            GameManager.Ins.Horror.Start_Game();
        }
    }
}