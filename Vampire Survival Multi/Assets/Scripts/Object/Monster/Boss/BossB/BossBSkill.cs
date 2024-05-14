using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BossBSkill : MonoBehaviour
{
    [Header("돌진 설정")]
    [SerializeField] private float shortDistance;
    [SerializeField] private float shortChargingDelay;
    [SerializeField] private float longChargingDelay;
    [SerializeField] private float rushSpeed;

    [Header("사용 오브젝트")]
    [SerializeField] private GameObject rushRoute;

    public void OnRush(GameObject boss, Vector3 targetPos)
    {
        // 돌진 길이 및 각도 조정
        Vector2 direction = targetPos - boss.transform.position;
        float distance = direction.magnitude;
        
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator RushCharging(float distance)
    {
        float delay = (distance > shortDistance) ? longChargingDelay : shortChargingDelay;

        // 차지 이팩트
        DOTween.Sequence()
            .OnStart(() =>
            {
                rushRoute.SetActive(true);
                rushRoute.transform.localScale = 
            })
    }
}