using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public abstract class Novel_Level : Level
    {
        public override void Initialize_Level(LevelController levelController)
        {
            base.Initialize_Level(levelController);
        }

        public override void Enter_Level()
        {
        }

        public override void Play_Level()
        {
        }

        public override void Update_Level()
        {
        }

        public override void Exit_Level()
        {
        }

        public override void OnDrawGizmos()
        {
        }

        public abstract List<ExcelData> Get_DialogData(int sheetIndex);
    }
}
