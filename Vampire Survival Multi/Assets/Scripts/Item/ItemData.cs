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
    private float _hpPercent;
    public float MultiplyHP
    {
        get { return _hpPercent; }
    }

    [SerializeField]
    private int _addSTR;
    public int IncreasedSTR
    {
        get { return _addSTR; }
    }
    [SerializeField]
    private float _strPercent;
    public float MultiplySTR
    {
        get { return _strPercent; }
    }

    [SerializeField]
    private int _addDEF;
    public int IncreasedDEF
    {
        get { return _addDEF; }
    }
    [SerializeField]
    private float _defPercent;
    public float MultiplyDEF
    {
        get { return _defPercent; }
    }

    [SerializeField]
    private int _addAGI;
    public int IncreasedAGI
    {
        get { return _addAGI; }
    }
    [SerializeField]
    private float _agiPercent;
    public float MultiplyAGI
    {
        get { return _agiPercent; }
    }

    [SerializeField]
    private float _attackSpeed;
    public float AttackSpeed
    {
        get { return _attackSpeed; }
    }

    [SerializeField]
    private float _lifeSteal;
    public float ListSteal
    {
        get { return _lifeSteal; }
    }
}