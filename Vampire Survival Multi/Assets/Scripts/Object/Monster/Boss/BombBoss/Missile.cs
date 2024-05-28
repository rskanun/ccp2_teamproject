using Photon.Pun;
using UnityEngine;

public class Missile : Monster
{
    [Header("유도탄 정보")]
    [SerializeField] private float bombRadius;
    [SerializeField] private float bombDelay;
    [SerializeField] private float bombDamage;

    private float delay;

    public void InitMissileData(float hp)
    {
        HP = hp;
        delay = bombDelay;
    }

    protected override void OnCooldown()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
    }

    protected override void OnCastSkill()
    {
        if (delay <= 0)
        {
            OnBomb();
        }
    }

    private void OnBomb()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius);

        // 터지면서 주변 플레이어에게 데미지
        foreach(Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<Player>().OnTakeDamage(bombDamage);
            }
        }

        // 터지고 난 후, 오브젝트 삭제
        DestroyObj();
    }

    [PunRPC]
    private void DestroyObj()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(DestroyObj), RpcTarget.Others);
        }

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    protected override void AttackedPlayer(GameObject target)
    {
        // 해당 오브젝트는 타격 판정이 없음
    }

    protected override void OnDead(Player killPlayer)
    {
        // 사망 시 그냥 사라짐
        DestroyObj();
    }
}