using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [Header("쿨타임")]
    [SerializeField]
    private float cooldown;
    private float curCooldown;

    public void OnUseSkill()
    {
        if (IsUseabled())
        {
            CastSkill();

            // 스킬 사용 후 쿨다운
            curCooldown = cooldown;
        }
    }

    protected virtual bool IsUseabled()
    {
        return curCooldown <= 0;
    }

    public void CooldownSkill()
    {
        if (curCooldown > 0)
            curCooldown -= Time.deltaTime;
    }

    protected abstract void CastSkill();
}