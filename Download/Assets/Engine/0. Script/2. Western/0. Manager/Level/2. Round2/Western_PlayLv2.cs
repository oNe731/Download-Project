using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Western
{
    public class Western_PlayLv2 : Western_Round2
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
            base.Enter_Level();
        }

        public override void Play_Level() // Ʃ�丮�� ���� �� Ready Go UI ��� �� �ش� �Լ� ȣ��
        {
        }

        public override void Update_Level()
        {
            if (GameManager.Ins.IsGame == false)
                return;
        }

        public override void LateUpdate_Level()
        {
        }

        public override void Exit_Level()
        {
        }
    }
}