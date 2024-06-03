using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("인벤토리")]
    [SerializeField] private Image invenIcon;
    [SerializeField] private Sprite invenOpenSprite;
    [SerializeField] private Sprite invenCloseSprite;
    [SerializeField] private GameObject invenWindow;
    [SerializeField] private RectTransform content;

    [Header("인벤토리 정보")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform slotContainPanel;
    [SerializeField] private int addItemSlots;

    [Header("초기 인벤토리 정보")]
    [SerializeField] private int initSlotCount;

    // 현재 인벤토리 정보
    private int slotNum;
    private int nextIndex;
    private bool isActive = false;

    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    private void Start()
    {
        AddSlots(initSlotCount);
    }

    public void AddItem(ItemData item)
    {
        if (nextIndex >= slotNum)
        {
            // 다음 넣을 아이템 칸이 없을 경우 새 슬롯 생성
            AddSlots(addItemSlots);
        }

        // 마지막 슬롯에 아이템 추가
        itemSlots[nextIndex++].SetItem(item);
    }

    private void AddSlots(int slotCount)
    {
        for(int i = 0; i < slotCount; i++)
        {
            // 추가 생성 슬롯 개수만큼 아이템 슬롯 추가
            GameObject itemSlot = Instantiate(itemPrefab, slotContainPanel);
            itemSlot.transform.SetAsFirstSibling();

            itemSlots.Add(itemSlot.GetComponent<ItemSlot>());
        }

        // 늘어난 슬롯만큼 높이 바꾸기
        float height = 0;
        if (slotNum <= 0)
        {
            // 처음 한 줄은 제외
            height = content.sizeDelta.y + ((slotCount / 6) - 1) * 250;
        }
        else
        {
            height = content.sizeDelta.y + (slotCount / 6) * 250;
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, height);

        // 최대 슬롯 개수 정보 최신화
        slotNum = itemSlots.Count;
    }

    public void SwitchingInven()
    {
        isActive = !isActive;

        if (isActive) OpenInventory();
        else CloseInventory();
    }

    public void OpenInventory()
    {
        invenIcon.sprite = invenOpenSprite;

        invenWindow.SetActive(true);
    }

    public void CloseInventory()
    {
        invenIcon.sprite = invenCloseSprite;

        invenWindow.SetActive(false);
    }
}