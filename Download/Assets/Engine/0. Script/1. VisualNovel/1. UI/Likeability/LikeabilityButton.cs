using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LikeabilityButton : MonoBehaviour
{
    public void Active_Button()
    {
        GameManager.Ins.Novel.Active_Popup();
    }
}
