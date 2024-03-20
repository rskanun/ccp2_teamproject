using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemUI : MonoBehaviour
{
    [Header("관련 오브젝트")]
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Image image;

    public void SetItem(ItemData item)
    {
        name.text = item.Name;
        image.sprite = item.ItemImage;
    }
}