using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [Header("사용 오브젝트")]
    [SerializeField] private GameObject selectWindow;
    [SerializeField] private GameObject container;

    [Header("아이템 선택 관련 변수")]
    [SerializeField] private int selectCount;

    // 생성된 아이템 선택창
    private List<GameObject> selectPrefabs = new List<GameObject>();

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
            // 게임 일시정지
            Time.timeScale = 0;

            OpenWindow();
        }
        else
        {
            // 게임 일시정지 해제
            Time.timeScale = 1f;

            CloseWindow();
        }
    }

    private void OpenWindow()
    {
        // 선택할 수 있는 아이템 랜덤 뽑기
        ItemData[] selectableItems = GetRandomItems(selectCount);

        // 아이템 선택창 띄우기
        for (int i = 0; i <selectCount; i++)
        {
            container.SetActive(true);

            // 아이템 선택창 생성
            GameObject prefeb = Instantiate(selectWindow, container.transform);

            selectPrefabs.Add(prefeb);

            // 선택된 아이템 띄우기
            SelectedItem selectedItem = prefeb.GetComponent<SelectedItem>();
            ItemData item = selectableItems[i];

            selectedItem.SetItem(item);
        }
    }

    private void CloseWindow()
    {
        // 아이템 선택창 지우기
        foreach(GameObject prefab in selectPrefabs)
        {
            Destroy(prefab);
        }

        container.SetActive(false);
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