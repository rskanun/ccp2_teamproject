using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/ShieldClass/ActiveSkill", fileName = "Shield Rush")]
public class ShieldRush : Skill
{
    // 스킬 세부 사항
    [SerializeField] private float rushDistance;
    [SerializeField] private float rushSpeed;
    [SerializeField] private float knockbackPower;

    public override void UseSkill(Player caster)
    {
        // 캐릭터 적용 후 제작
    }
}