using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Novel_Begin : Level
{
    public Novel_Begin(LevelController levelController) : base(levelController)
    {
    }

    public override void Enter_Level()
    {
        VisualNovelManager.Instance.Dialog.SetActive(true);
        VisualNovelManager.Instance.Dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog1_SchoolWay.json"));
        
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_2D);
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            VisualNovelManager.Instance.Active_Popup();
    }

    public override void Exit_Level()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
