using UnityEngine;

public class InvisibleBoss : BossMonster
{
    [Header("보스 정보")]
    [SerializeField] private float invisibleDelay;
    [SerializeField] private float throwCooldown;

    // 현재 보스 상태
    private bool isActiveState;
    private float curDelay;
    private float curCooldown;

    protected override void OnCastSkill()
    {
        if (isActiveState && curCooldown <= 0)
        {
            // 돌 던지기

            // 쿨타임 적용
            curCooldown = throwCooldown;
        }
    }


}