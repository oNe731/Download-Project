using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Citizen : Person
    {
        public Citizen() : base()
        {
        }

        public void Initialize(int roundIndex)
        {
            base.Initialize();
            m_personType = PERSONTYPE.PT_CITIZEN;
            //m_meshRenderer.materials[0].SetTexture("_BaseMap", Resources.Load<Texture2D>("1. Graphic/3D/2. Western/Character/Texture/Person_02"));
            string path = "";
            switch (roundIndex)
            {
                case (int)WesternManager.LEVELSTATE.LS_PlayLv1:
                    path = "6. Animation/2. Western/Character/Round1/Citizen/AC_Citizen";
                    break;
                case (int)WesternManager.LEVELSTATE.LS_PlayLv2:
                    path = "6. Animation/2. Western/Character/Round2/Citizen/AC_Citizen";
                    break;
                case (int)WesternManager.LEVELSTATE.LS_PlayLv3:
                    path = "6. Animation/2. Western/Character/Round3/Citizen/AC_Citizen";
                    break;
            }
            m_animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(path);

            gameObject.SetActive(false);
        }

        private void Start()
        {
        }

        private void Update()
        {
        }
    }
}

