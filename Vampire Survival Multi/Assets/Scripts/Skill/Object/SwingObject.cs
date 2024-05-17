using DG.Tweening;
using UnityEngine;

public class SwingObject : MonoBehaviour
{
    [Header("타격 각도")]
    [SerializeField] private float swingAngle;

    public delegate void OnHit(Monster monster);
    private event OnHit onHitEvent;

    public void OnSwing(float attackAngle, float swingSpeed, bool isRightSwing, OnHit onHitListener)
    {
        onHitEvent = onHitListener;

        float halfSwingAngle = swingAngle / 2.0f;
        float startAngle = (isRightSwing) ? attackAngle + halfSwingAngle : attackAngle - halfSwingAngle;
        float endAngle = (isRightSwing) ? attackAngle - halfSwingAngle : attackAngle + halfSwingAngle;

        // 휘두르기 애니메이션
        DOTween.Sequence()
            .OnStart(() =>
            {
                // 각도 조절
                transform.localRotation = Quaternion.Euler(0, 0, attackAngle - swingAngle / 2.0f);
            })
            .Append(transform.DORotate(new Vector3(0, 0, attackAngle + swingAngle / 2.0f), swingSpeed))
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Monster monster = collision.GetComponent<Monster>();

            onHitEvent?.Invoke(monster);
        }
    }
}