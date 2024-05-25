using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    public delegate void OnHit(Monster monster);
    public delegate void OnMove();
    private event OnHit onHitEvent;
    private event OnMove onMoveEvent;

    private bool isPiercing = false;

    public void ThrowProjectile(Vector2 targetPos, float speed, bool isPiercing, OnHit onHitListener)
    {
        photonView.RPC(nameof(SetPiercing), RpcTarget.All, isPiercing);

        onHitEvent = onHitListener;
        photonView.RPC(nameof(SetOnMoveEvent), RpcTarget.All, targetPos, speed);
    }

    [PunRPC]
    private void SetPiercing(bool isPiercing)
    {
        this.isPiercing = isPiercing;
    }

    [PunRPC]
    private void SetOnMoveEvent(Vector2 targetPos, float speed)
    {
        onMoveEvent = () => MoveTo(targetPos, speed);
    }

    private void FixedUpdate()
    {
        onMoveEvent?.Invoke();
    }

    private void MoveTo(Vector2 targetPos, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if ((Vector2)transform.position == targetPos)
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(DestroyProjectile), RpcTarget.All);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (collision.CompareTag("Monster"))
        {
            Monster monster = collision.GetComponent<Monster>();

            onHitEvent?.Invoke(monster);

            if (isPiercing == false)
            {
                // 관통이 아닐 경우 파괴
                photonView.RPC(nameof(DestroyProjectile), RpcTarget.AllBuffered);
            }
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