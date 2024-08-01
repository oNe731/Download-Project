using Horror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGetItem : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    public void Initialize_UI(NoteItem.ITEMTYPE weaponId)
    {
        switch(weaponId)
        {
            case NoteItem.ITEMTYPE.TYPE_PIPE:
                m_text.text = "ÆÄÀÌÇÁ È¹µæ";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Melee", HorrorManager.Instance.Player.transform).GetComponent<Weapon<HorrorPlayer>>());
                break;
            case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                m_text.text = "¼ÕÀüµî È¹µæ";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Flashlight", HorrorManager.Instance.Player.transform).GetComponent<Weapon<HorrorPlayer>>());
                break;
            case NoteItem.ITEMTYPE.TYPE_GUN:
                m_text.text = "ÃÑ È¹µæ";
                HorrorManager.Instance.Player.WeaponManagement.Add_Weapon(GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Character/Weapon/Gun", HorrorManager.Instance.Player.transform).GetComponent<Weapon<HorrorPlayer>>());
                break;

            case NoteItem.ITEMTYPE.TYPE_NOTE:
                m_text.text = "µ¿·áÀÇ ¼öÃ¸ È¹µæ";
                HorrorManager.Instance.Player.Acquire_Note();
                break;
        }    
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            HorrorManager.Instance.Set_Pause(false);
            Destroy(gameObject);
        }
    }
}
