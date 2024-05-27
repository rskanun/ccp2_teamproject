using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

public class WandSkillProjectile : MonoBehaviourPun
{
    public delegate void OnMove();
    private event OnMove onMoveEvent;

    private float radius;
    private string areaPrefab;
    private float effectDuration;

    public void ThrowProjectile(Vector2 targetPos, float speed, float angle, float effectRadius, float effectDuration, string areaPrefab)
    {
        // 투사체 각도 설정
        transform.eulerAngles = new Vector3(0, 0, angle);

        radius = effectRadius;
        this.areaPrefab = areaPrefab;
        this.effectDuration = effectDuration;

        onMoveEvent = () => MoveTo(targetPos, speed);
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            onMoveEvent?.Invoke();
        }
    }

    private void MoveTo(Vector2 targetPos, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if ((Vector2)transform.position == targetPos)
        {
            // 사거리 끝에 도달했을 경우 적중 효과 발생
            OnHit();
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
            // 닿은 대상이 몬스터일 경우 나아가는 것을 멈추고 적중 효과 발생
            OnHit();
        }
    }

    private void OnHit()
    {
        if (areaPrefab.IsNullOrEmpty() == false)
        {
            // 범위 오브젝트 생성
            GameObject areaObj = PhotonNetwork.Instantiate(areaPrefab, transform.position, Quaternion.identity);

            WandSkillArea areaEffect = areaObj.GetComponent<WandSkillArea>();
            areaEffect.EffectArea(radius, effectDuration);
        }

        photonView.RPC(nameof(DestroyProjectile), RpcTarget.All);
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