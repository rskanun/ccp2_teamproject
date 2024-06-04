using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemGrade
{
    Common,
    Rare,
    Epic,
    Legend
}
public class ItemResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Items";
    private const string FILE_PATH = "Assets/Resources/Items/ItemResource.asset";

    private static ItemResource _instance;
    public static ItemResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ItemResource>("Items/ItemResource");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }

                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<ItemResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ItemResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("사용 가능한 아이템 리스트")]
    [SerializeField]
    private List<ItemData> _commonDatas;
    public List<ItemData> CommonDatas
    {
        get { return _commonDatas; }
    }
    [SerializeField]
    private List<ItemData> _rareDatas;
    public List<ItemData> RareDatas
    {
        get { return _rareDatas; }
    }
    [SerializeField]
    private List<ItemData> _epicDatas;
    public List<ItemData> EpicDatas
    {
        get { return _epicDatas; }
    }
    [SerializeField]
    private List<ItemData> _legendDatas;
    public List<ItemData> LegendDatas
    {
        get { return _legendDatas; }
    }

    // 아이템 목록
    private Dictionary<int, ItemData> itemTable = new Dictionary<int, ItemData>();

    [ContextMenu("Reload Items")]
    private void OnValidate()
    {
        InitItemData();
    }

    public void InitItemData()
    {
        // Init Common Item Datas
        foreach (ItemData item in CommonDatas)
        {
            itemTable[item.ID] = item;
        }

        // Init Rare Item Datas
        foreach (ItemData item in RareDatas)
        {
            itemTable[item.ID] = item;
        }

        // Init Epic Item Datas
        foreach (ItemData item in EpicDatas)
        {
            itemTable[item.ID] = item;
        }

        // Init Legend Item Datas
        foreach (ItemData item in LegendDatas)
        {
            itemTable[item.ID] = item;
        }
    }

    public ItemData FindItem(int id)
    {
        if (itemTable.ContainsKey(id))
        {
            return itemTable[id];
        }

        return null;
    }

    public ItemData GetRandomItem(ItemGrade grade)
    {
        switch(grade)
        {
            case ItemGrade.Common:
                return GetRandomCommonItem();

            case ItemGrade.Rare:
                return GetRandomRareItem();

            case ItemGrade.Epic:
                return GetRandomEpicItem();

            case ItemGrade.Legend:
                return GetRandomLegendItem();

            default:
                return null;
        }
    }

    private ItemData GetRandomCommonItem()
    {
        int maxIndex = CommonDatas.Count;
        int randomIndex = Random.Range(0, maxIndex);

        return CommonDatas[randomIndex];
    }

    private ItemData GetRandomRareItem()
    {
        int maxIndex = RareDatas.Count;
        int randomIndex = Random.Range(0, maxIndex);

        return RareDatas[randomIndex];
    }

    private ItemData GetRandomEpicItem()
    {
        int maxIndex = EpicDatas.Count;
        int randomIndex = Random.Range(0, maxIndex);

        return EpicDatas[randomIndex];
    }

    private ItemData GetRandomLegendItem()
    {
        int maxIndex = LegendDatas.Count;
        int randomIndex = Random.Range(0, maxIndex);

        return LegendDatas[randomIndex];
    }
}