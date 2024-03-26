using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Object/Player", fileName = "ClassData")]
public class ClassData : ObjectData
{
    [Header("초기 장비")]
    [SerializeField]
    private List<ItemData> _equips;
    public List<ItemData> Equips
    {
        get { return _equips; }
    }
}
