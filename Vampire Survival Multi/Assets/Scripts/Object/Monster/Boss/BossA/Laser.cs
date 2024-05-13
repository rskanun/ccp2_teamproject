using Photon.Pun;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("보스 데이터")]
    [SerializeField] private MonsterData data;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            float damage = data.STR;

            Player Player = collision.GetComponent<Player>();
            Player.OnTakeDamage(damage);
        }
    }
}