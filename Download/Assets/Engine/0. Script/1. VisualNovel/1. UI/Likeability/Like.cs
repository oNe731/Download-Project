using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class Like : ParabolaUI
    {
        private VisualNovelManager.OWNERTYPE m_npcIndex = VisualNovelManager.OWNERTYPE.OT_END;
        private bool m_effectCreate = false;

        public VisualNovelManager.OWNERTYPE NpcIndex { set => m_npcIndex = value; }
        public bool EffectCreate { set => m_effectCreate = value; }

        private new void Start()
        {
            base.Start();
        }

        public void Shake_Heart()
        {
            Shake_Object(() => Create_Effect());
        }

        private void Create_Effect()
        {
            if (m_effectCreate == true)
            {
                string name = "";
                switch (m_npcIndex)
                {
                    case VisualNovelManager.OWNERTYPE.OT_BLUE:
                        name = "UI_LikeParticle_B";
                        break;

                    case VisualNovelManager.OWNERTYPE.OT_PINK:
                        name = "UI_LikeParticle_P";
                        break;

                    case VisualNovelManager.OWNERTYPE.OT_YELLOW:
                        name = "UI_LikeParticle_Y";
                        break;
                }

                GameObject particleObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/UI/" + name, GameObject.Find("Canvas").transform);
                particleObject.transform.position = gameObject.transform.position;
            }
        }
    }
}