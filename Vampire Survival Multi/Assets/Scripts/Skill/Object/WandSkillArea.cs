using Photon.Pun;
using UnityEngine;

public class WandSkillArea : MonoBehaviourPun
{
    public void EffectArea(float radius, float effectDuration)
    {
        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D obj in objs)
        {
            if (obj.CompareTag("Monster"))
            {
                Monster monster = obj.GetComponent<Monster>();

                monster.AddEffect(StatusEffect.Weakness, effectDuration);
            }
        }

        // 1초 뒤 오브젝트 삭제
        Invoke(nameof(DestroyObj), 0.3f);
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