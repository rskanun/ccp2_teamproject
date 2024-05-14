using UnityEngine;

[CreateAssetMenu(menuName = "Game Object/Monster", fileName = "MonsterData")]
public class MonsterData : ObjectData
{
    [Header("몬스터 데이터")]
    [SerializeField]
    private float _attackDistance;
    public float AttackDistance
    {
        get { return _attackDistance; }
    }
}