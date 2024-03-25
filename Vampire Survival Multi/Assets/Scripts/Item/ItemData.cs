using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    [Header("아이템 정보")]
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private Sprite _itemImage;
    public Sprite ItemImage
    {
        get { return _itemImage; }
    }

    [SerializeField]
    [TextArea]
    private string _description;
    public string Description
    {
        get { return _description; }
    }

    [Header("아이템 효과")]
    [SerializeField]
    private int _addHP;
    public int IncreasedHP
    {
        get { return _addHP; }
    }

    [SerializeField]
    private int _addSTR;
    public int IncreasedSTR
    {
        get { return _addSTR; }
    }

    [SerializeField]
    private int _addDEF;
    public int IncreasedDEF
    {
        get { return _addDEF; }
    }

    [SerializeField]
    private int _addAGI;
    public int IncreasedAGI
    {
        get { return _addAGI; }
    }
}