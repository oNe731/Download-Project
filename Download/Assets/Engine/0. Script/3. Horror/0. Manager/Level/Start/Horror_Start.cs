using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horror
{
    public class Horror_Start : MonoBehaviour
    {
        public void Button_Start()
        {
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Start_Game(), 1f, false);
        }

        private void Start_Game()
        {
            Destroy(gameObject);
            HorrorManager.Instance.Start_Game();
        }

        public void Button_Exit()
        {
            SceneManager.LoadScene("Window");
        }
    }
}