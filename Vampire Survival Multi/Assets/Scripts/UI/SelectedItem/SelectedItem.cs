using UnityEngine;

[RequireComponent (typeof(SelectedItemUI))]
public class SelectedItem : MonoBehaviour
{
    private SelectedItemUI ui;

    private void Awake()
    {
        ui = GetComponent<SelectedItemUI>();
    }

    public void SetItem(ItemData item)
    {
        ui.SetItem(item);
    }
}