using Photon.Pun;
using UnityEngine;

public class SuicideBoss : BossMonster
{
    [Header("보스 정보")]
    [SerializeField] private float suicideDelay;
    [SerializeField] private float throwCooldown;
    [SerializeField] private GameObject stonePrefab;

    // 현재 보스 상태
    private float curDelay;
    private float curCooldown;

    protected override void OnCastSkill()
    {
        OnThrowStone();
        OnSuicide();
    }

    private void OnThrowStone()
    {
        if (curCooldown <= 0)
        {
            // 8방향 동시에 돌 던지기
            for (float angle = 0; angle < 360; angle += 45)
            {
                // 해당 방향으로 돌 던지기
                ThrowStone(angle);
            }

            // 쿨타임 적용
            curCooldown = throwCooldown;
        }
    }

    private void ThrowStone(float angle)
    {
        string prefabName = OBJECT_DIRECTION + stonePrefab.name;
        GameObject stone = PhotonNetwork.Instantiate(prefabName, transform.position, Quaternion.identity);

        StoneProjectile projectile = stone.GetComponent<StoneProjectile>();
        projectile.SetAngle(angle);
    }

    private void OnSuicide()
    {
        // 데미지를 입지 않고 일정 시간 경과 시 보스 클리어
        if (curDelay >= suicideDelay)
        {
            OnDead(null);
        }
    }

    protected override void OnCooldown()
    {
        if (curCooldown > 0)
        {
            curCooldown -= Time.deltaTime;
        }

        if (curDelay < suicideDelay)
        {
            curDelay += Time.deltaTime;
        }
    }

    public override void OnTakeDamage(Player attacker, float damage)
    {
        // 데미지를 입을 시 딜레이 초기화
        curDelay = 0;

        // 기존 데미지 방식 적용
        base.OnTakeDamage(attacker, damage);
    }
}