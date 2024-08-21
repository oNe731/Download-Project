using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Horror
{
    public class NoteSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private NoteItem.NOTETYPE m_slotType = NoteItem.NOTETYPE.TYPE_END;

        private Note m_note = null;
        private NoteItem m_item = null;
        private UINoteIcon m_uIItem = null;

        private bool m_drag = false;

        public NoteItem.NOTETYPE SlotType { get => m_slotType; }
        public NoteItem Item { get => m_item; set => m_item = value; }

        public void Initialize_Slot(Note note)
        {
            m_note = note;

            GameObject gameObject = Instantiate(Resources.Load<GameObject>("5. Prefab/3. Horror/UI/UI_NoteIcon"), transform);
            m_uIItem = gameObject.GetComponent<UINoteIcon>();
            m_uIItem.gameObject.SetActive(false);
        }

        public void Add_Item(NoteItem noteItem, bool reset)
        {
            if (m_item == null || reset)
                m_item = noteItem;
            else
            {
                // 개수 증가
                if (m_item.m_itemType == noteItem.m_itemType)
                    m_item.m_count += noteItem.m_count;
            }

            if (noteItem.m_count <= 0) // 초기화
            {
                Reset_Slot();
                return;
            }

            m_uIItem.Initialize_Icon(m_item);
        }

        public void Use_Item()
        {
        }

        public void Reset_Slot()
        {
            m_uIItem.gameObject.SetActive(false);
            m_item = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 정보창 활성화

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_slotType == NoteItem.NOTETYPE.TYPE_ITEM)
                m_drag = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_drag == false)
                return;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out localPoint);
            transform.GetChild(0).localPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData) // 드래그 시작한 곳의 끝 호출
        {
            if (m_drag == false)
                return;

            m_drag = false;
            transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);

            GameObject draggedObject = eventData.pointerEnter; // 놓은 곳에 UI가 존재하는지
            if (draggedObject == null)
            {
                Create_Item();
                return;
            }

            NoteSlot slot = draggedObject.GetComponent<NoteSlot>();
            if (slot == null) // 놓은 곳이 슬롯인지
                return;

            if (slot == this || slot.SlotType != NoteItem.NOTETYPE.TYPE_ITEM || slot.Item != Item)
                return;

            Reset_Slot(); // 초기화
            m_note.Sort_Item(); // 정렬
        }

        public void OnDrop(PointerEventData eventData) // OnEndDrag 보다 먼저 호출/ 놓여지는 슬롯 호출
        {
            if (m_slotType != NoteItem.NOTETYPE.TYPE_ITEM || m_item != null)
                return;

            GameObject draggedObject = eventData.pointerDrag; // 드래그 했던 오브젝트
            if (draggedObject == null)
                return;
            NoteSlot item = draggedObject.GetComponent<NoteSlot>();
            if (item == null)
                return;

            Add_Item(item.Item, true);
        }

        private void Create_Item()
        {
            if (m_note.BaseCamera == null) // Output 텍스처를 만드는 카메라는 사용x
                return;

            // 월드 아이템 생성
            GameObject worldObject = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/Item/Research_Item");

#region  위치 지정
            Ray ray = m_note.BaseCamera.ScreenPointToRay(Input.mousePosition);

            // 해당 벽 바닥위치 이동
            RaycastHit wallHit;
            RaycastHit groundHit;
            if (Physics.Raycast(ray, out wallHit, Mathf.Infinity, LayerMask.GetMask("Wall")))
            {
                if (Physics.Raycast(wallHit.point, Vector3.down, out groundHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                    worldObject.transform.position = groundHit.point;
                else // !
                    worldObject.transform.position = HorrorManager.Instance.Player.transform.position;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                    worldObject.transform.position = hit.point;
                else
                { // 바닥이 아닌 경우 Ex. 상단 방향 등

                    float dist = Random.Range(2f, 3f);
                    RaycastHit playerHit = GameManager.Instance.Start_Raycast(HorrorManager.Instance.Player.transform.position, HorrorManager.Instance.Player.transform.forward, dist, LayerMask.GetMask("Wall"));
                    if (playerHit.collider == null) // 벽이 아닌 경우 해당 방향으로 이동 가능
                        worldObject.transform.position = HorrorManager.Instance.Player.transform.position + (HorrorManager.Instance.Player.transform.forward * dist);
                    else // 벽 끝으로 이동
                    {
                        RaycastHit wallOutHit = GameManager.Instance.Start_Raycast(HorrorManager.Instance.Player.transform.position, HorrorManager.Instance.Player.transform.forward, Mathf.Infinity, LayerMask.GetMask("Wall"));
                        if (wallOutHit.collider != null)
                        {
                            worldObject.transform.position = wallOutHit.point;
                            RaycastHit groundOutHit;
                            if (Physics.Raycast(wallOutHit.point, Vector3.down, out groundOutHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                                worldObject.transform.position = groundOutHit.point;
                            else
                                worldObject.transform.position = wallOutHit.point;//HorrorManager.Instance.Player.transform.position;
                        }
                        else
                            worldObject.transform.position = HorrorManager.Instance.Player.transform.position;
                    }
                }
            }
#endregion

            Interaction_Research_Item interaction = worldObject.GetComponent<Interaction_Research_Item>();
            if (interaction == null)
                return;
            interaction.NoteItem = Item;

            Reset_Slot(); // 초기화
            m_note.Sort_Item(); // 정렬
        }
    }
}

