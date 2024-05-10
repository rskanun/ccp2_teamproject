using UnityEngine;

public class TrackingArrow : MonoBehaviour
{
    [Header("추적 플레이어")]
    [SerializeField] private PlayerData trackingPlayer;

    [Header("추적 화살표")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject text;

    private float distance = 500f;
    private bool isExitedCamera = false;

    private void Start()
    {
        if (trackingPlayer.IsPlaying == false)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        PlayerData localPlayer = LocalPlayerData.Instance.PlayerData;

        // 카메라 사이즈 계산
        Vector2 cameraHalfSize = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);

        // 플레이어와의 거리 계산
        Vector2 distanceVec = trackingPlayer.Position - localPlayer.Position;

        if (cameraHalfSize.x < Mathf.Abs(distanceVec.x) || cameraHalfSize.y < Mathf.Abs(distanceVec.y))
        {
            if (isExitedCamera == false)
            {
                isExitedCamera = true;

                // 해당 플레이어가 화면 밖으로 나가게 될 경우 화살표 표시
                arrow.SetActive(true);
                text.SetActive(true);
            }

            Vector2 direction = distanceVec.normalized;

            // 화살표 방향 전환
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // 화살표 이동
            transform.localPosition = direction * distance;
        }
        else if(isExitedCamera)
        {
             isExitedCamera = false;

            // 해당 플레이어가 다시 화면 안으로 들어올 경우 화살표 비활성화
            arrow.SetActive(false);
            text.SetActive(false);
        }
    }
}