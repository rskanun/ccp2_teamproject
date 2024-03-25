using UnityEngine;

[RequireComponent (typeof(SelectedItemUI))]
public class SelectedItem : MonoBehaviour
{
    private ItemData item;
    private SelectedItemUI ui;

    private void Awake()
    {
        ui = GetComponent<SelectedItemUI>();
    }

    public void SetItem(ItemData item)
    {
        this.item = item;

        ui.SetItem(item);
    }

    public void SelectItem()
    {

    }
}