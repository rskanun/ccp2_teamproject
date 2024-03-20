using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [Header("사용 오브젝트")]
    [SerializeField]
    private GameObject[] selectWindows;

    [Header("아이템 선택 관련 변수")]
    [SerializeField]
    private ItemData[] selectableItems;

    private void Update()
    {
        // 임시 아이템 선택창 띄우기
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SetActive(false);
        }
    }

    public void SetActive(bool isActive)
    {
        if (isActive)
        {
            OpenWindow();

        }
    }

    private void OpenWindow()
    {
        // 선택할 수 있는 아이템 랜덤 뽑기
        selectableItems = GetRandomItems(selectWindows.Length);

        // 아이템 선택창 띄우기
        int count = 0;
        foreach (GameObject selectWindow in selectWindows)
        {
            selectWindow.SetActive(true);

            // 선택된 아이템 띄우기
            SelectedItem selectedItem = selectWindow.GetComponent<SelectedItem>();
            ItemData item = selectableItems[count++];

            selectedItem.SetItem(item);
        }
    }

    private ItemData[] GetRandomItems(int count)
    {
        ItemData[] items = new ItemData[count];

        // count 만큼 아이템 뽑기
        for(int i = 0;  i < count; i++)
        {
            items[i] = GetRandomItem();
        }

        return items;
    }

    private ItemData GetRandomItem()
    {
        // ItemResource 안의 사용가능한 아이템 중 하나 랜덤으로 뽑기
        List<ItemData> items = ItemResource.Instance.ItemDatas;

        int randomNum = Random.Range(0, items.Count);
        ItemData randomItem = items[randomNum];

        return randomItem;
    }
}