using Photon.Pun;
using UnityEngine;

public class BombBoss : BossMonster
{
    [Header("보스 유도탄 데이터")]
    [SerializeField] private float missileCooldown;
    [SerializeField] private GameObject missileObj;

    [Header("보스 폭탄 데이터")]
    [SerializeField] private float bombCooldown;
    [SerializeField] private GameObject bombObj;

    private float curMissileCooldown;
    private float curBombCooldown;

    protected override void OnCastSkill()
    {
        OnCastMissile();
        OnCastBomb();
    }

    private void OnCastMissile()
    {
        if (curMissileCooldown <= 0.0f)
        {
            float missileHP = Stat.HP / 10.0f;

            // 유도탄 생성
            GameObject missile = PhotonNetwork.Instantiate(OBJECT_DIRECTION + missileObj.name, transform.position, Quaternion.identity);
            missile.GetComponent<Missile>().InitMissileData(missileHP);

            // 쿨타임 적용
            curMissileCooldown = missileCooldown;
        }
    }

    private void OnCastBomb()
    {
        if (curBombCooldown <= 0.0f)
        {
            // 설치형 생성
            PhotonNetwork.Instantiate(OBJECT_DIRECTION + bombObj.name, transform.position, Quaternion.identity);

            // 쿨타임 적용
            curBombCooldown = bombCooldown;
        }
    }

    protected override void OnCooldown()
    {
        if (curMissileCooldown > 0.0f)
        {
            curMissileCooldown -= Time.deltaTime;
        }

        if (curBombCooldown > 0.0f)
        {
            curBombCooldown -= Time.deltaTime;
        }
    }
}