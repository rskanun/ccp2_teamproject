using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    [TextArea]
    private string _description;
    public string Description
    {
        get { return _description; }
    }

    [SerializeField]
    private Sprite _itemImage;
    public Sprite ItemImage
    {
        get { return _itemImage; }
    }
}