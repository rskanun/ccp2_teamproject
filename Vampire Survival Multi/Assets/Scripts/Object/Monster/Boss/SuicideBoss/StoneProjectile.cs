using Photon.Pun;
using UnityEngine;

public class StoneProjectile : MonoBehaviourPun
{
    [Header("투사체 정보")]
    [SerializeField] private float distance;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private Vector2 targetPos;

    public void SetAngle(float angle)
    {
        // 투사체 각도 조정
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 투사체 목표 지점 설정
        float radius = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radius), Mathf.Sin(radius)) * distance;
        targetPos = (Vector2)transform.position + direction;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if ((Vector2)transform.position == targetPos)
            {
                if (PhotonNetwork.IsMasterClient)
                    photonView.RPC(nameof(DestroyProjectile), RpcTarget.All);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        // 오브젝트에 닿은 게 플레이어일 경우
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            // 데미지 주기
            player.OnTakeDamage(damage);

            // 투과하지 않고 파괴
            photonView.RPC(nameof(DestroyProjectile), RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestroyProjectile()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}