using UnityEngine;

public abstract class ActiveSkill : Skill
{
    // 스킬 상세 정보
    [SerializeField] protected float _damageValue;
    [SerializeField] protected float _strengthRate;

    protected float SkillDamage
    {
        get
        {
            float damage = _damageValue + (CasterData.STR * _strengthRate);

            return damage;
        }
    }
}