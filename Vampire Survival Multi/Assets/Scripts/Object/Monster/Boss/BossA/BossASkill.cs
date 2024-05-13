using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BossASkill : MonoBehaviour
{
    [Header("레이저 설정")]
    [SerializeField] private float castTime;
    [SerializeField] private float remaintime;
    [SerializeField] private float width;
    [SerializeField] private float height;

    [Header("사용 오브젝트")]
    [SerializeField] private GameObject warningArea;
    [SerializeField] private GameObject laser;

    public void OnShoot(float rotate)
    {
        // 쏠 방향 설정
        transform.rotation = Quaternion.Euler(0, 0, rotate);

        // 레이저 발사
        StartCoroutine(ShootLaser());
    }

    private IEnumerator ShootLaser()
    {
        // 레이저 사격 경고 애니메이션
        DOTween.Sequence()
            .OnStart(() =>
            {
                warningArea.SetActive(true);
                warningArea.transform.localScale = new Vector3(width, height);
            })
            .Append(warningArea.transform.DOScaleY(0.0f, castTime))
            .OnComplete(() => warningArea.SetActive(false));

        yield return new WaitForSeconds(castTime);

        // 레이저 발사
        laser.SetActive(true);
        yield return new WaitForSeconds(remaintime);
        laser.SetActive(false);
    }
}