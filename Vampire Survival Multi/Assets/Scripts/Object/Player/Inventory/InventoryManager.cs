using UnityEngine;

[RequireComponent(typeof(InventoryUI))]
public class InventoryManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private InventoryUI ui;

    // 참조 데이터
    private PlayerEquip inventory;

    private void Start()
    {
        inventory = PlayerEquip.Instance;
    }

    private void OnDisable()
    {
        ui.CloseInventory();
    }

    public void UpdateInventory()
    {
        ui.AddItem(inventory.LastItem);
    }
}