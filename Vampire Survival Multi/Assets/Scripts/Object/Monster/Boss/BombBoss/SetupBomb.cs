using Photon.Pun;
using UnityEngine;

public class SetupBomb : MonoBehaviourPun
{
    [Header("폭탄 정보")]
    [SerializeField] private float bombRadius;
    [SerializeField] private float bombDelay;
    [SerializeField] private float bombDamage;

    private float delay;

    private void OnEnable()
    {
        delay = bombDelay;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (delay <= 0)
            {
                // 일정 시간이 되면 터짐
                OnBomb();
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
    }

    private void OnBomb()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius);

        // 터지면서 주변 플레이어에게 데미지
        foreach (Collider2D collider in colliders)
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
}