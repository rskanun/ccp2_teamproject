using UnityEngine;

[RequireComponent(typeof(SelectedItemUI))]
public class SelectedItem : MonoBehaviour
{
    private SelectedItemUI ui;
    private RewardManager manager;
    private ItemData item;

    private void Awake()
    {
        ui = GetComponent<SelectedItemUI>();
        manager = GetComponentInParent<RewardManager>();
    }

    public void SetItem(ItemData item)
    {
        this.item = item;
        ui.SetItem(item);
    }

    public void SelectItem()
    {
        PlayerEquip.Instance.EquipItem(item);
        PlayerInventory.Instance.AddItem(item); // 플레이어 인벤토리에 아이템 추가
        manager.CloseWindow();
    }

    public void HandOverItem()
    {
        manager.HandOverItem(item);
    }
}