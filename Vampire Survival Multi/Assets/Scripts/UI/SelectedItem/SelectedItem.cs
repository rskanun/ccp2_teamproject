using UnityEngine;

[RequireComponent (typeof(SelectedItemUI))]
public class SelectedItem : MonoBehaviour
{
    // 참조 컴포넌트
    private SelectedItemUI ui;
    private SelectManager manager;

    private ItemData item;

    private void Awake()
    {
        ui = GetComponent<SelectedItemUI>();
        manager = GetComponentInParent<SelectManager>();
    }

    public void SetItem(ItemData item)
    {
        this.item = item;

        ui.SetItem(item);
    }

    public void SelectItem()
    {
        PlayerEquip.Instance.EquipItem(item);

        manager.SetActive(false);
    }
}