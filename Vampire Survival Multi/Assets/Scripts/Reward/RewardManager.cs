using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    [Header("사용 오브젝트")]
    [SerializeField] private GameObject selectPrefab;
    [SerializeField] private GameObject container;

    [Header("아이템 선택 관련 변수")]
    [SerializeField] private int selectCount;

    // 생성된 아이템 선택창
    private List<GameObject> selectWindows = new List<GameObject>();

    // 획득할 보상 수
    private int rewardCount = 0;

    public void GetReward()
    {
        // 이미 보상창이 열려 있다면 카운트 추가
        if (container.activeSelf) rewardCount++;
        else
        {
            // 게임 일시정지
            Time.timeScale = 0f;

            OpenWindow();
        }
    }

    private void OpenWindow()
    {
        // 선택할 수 있는 아이템 랜덤 뽑기
        ItemData[] selectableItems = GetRandomItems(selectCount);

        // 아이템 선택창 띄우기
        List<GameObject> items = new List<GameObject>();

        for (int i = 0; i <selectCount; i++)
        {
            container.SetActive(true);

            // 아이템 선택창 생성
            GameObject prefeb = Instantiate(selectPrefab, container.transform);

            // 선택된 아이템 띄우기
            SelectedItem selectedItem = prefeb.GetComponent<SelectedItem>();
            ItemData item = selectableItems[i];

            selectedItem.SetItem(item);

            items.Add(prefeb);
        }

        selectWindows = items;

        // 첫번째 요소 선택 상태로 전환
        SelectObject(selectWindows[0]);
    }

    private void SelectObject(GameObject SelectedItem)
    {
        Button button = SelectedItem.GetComponent<Button>();

        button.Select();
    }

    public void CloseWindow()
    {
        // 아이템 선택창 지우기
        foreach(GameObject prefab in selectWindows)
        {
            Destroy(prefab);
        }

        if (rewardCount > 0)
        {
            // 받을 보상이 남아있다면 창 다시 열기
            OpenWindow();

            rewardCount--;
        }
        else
        {
            container.SetActive(false);

            // 게임 일시정지 해제
            Time.timeScale = 1f;
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