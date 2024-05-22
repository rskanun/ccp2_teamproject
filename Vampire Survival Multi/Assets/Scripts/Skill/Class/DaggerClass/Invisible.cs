using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/DaggerClass/ActiveSkill", fileName = "Invisible")]
public class Invisible : Skill
{
    // 스킬 세부 사항
    [SerializeField] private float duration;

    public override void UseSkill(Player caster, Vector2 direction)
    {
        caster.AddBuff(Buff.Invisible, duration);
    }
}