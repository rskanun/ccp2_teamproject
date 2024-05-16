using Photon.Pun;
using UnityEngine;

public class BossC : BossMonster
{
    [Header("보스 분열 데이터")]
    [SerializeField] private int maxSplitConut;
    [SerializeField] private int splitBodyNum;
    [SerializeField] private float splitSpeed;

    // 분열체 정보
    private static int splitBodyCount = 1;
    private int splitCount;
    private float splitBodySpeed;

    protected override void OnDead(Player killPlayer)
    {
        if (splitCount < maxSplitConut)
        {
            // 분열수만큼 분열체 생성
            for (int i = 0; i < splitBodyNum; i++)
            {
                string name = RemoveCloneInName(gameObject.name);
                GameObject splitBody = PhotonNetwork.Instantiate(name, transform.position, Quaternion.identity);

                splitBody.GetComponent<BossC>().OnSplit(splitCount + 1);
            }
        }

        // 남은 분열체 수 설정
        splitBodyCount--;
        Debug.Log($"Dead -> remain: {splitBodyCount} / splitCount: {splitCount}");

        // 보스 정보 동기화
        photonView.RPC(nameof(AsyncSplitBodyCount), RpcTarget.Others, splitBodyCount);

        if (splitBodyCount <= 0)
        {
            // 모든 분열체가 죽으면 사망 판정
            base.OnDead(killPlayer);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string RemoveCloneInName(string name)
    {
        int index = name.IndexOf("(Clone)");
        if (index > 0)
            return name.Substring(0, index);

        return name;
    }

    public void OnSplit(int splitCount)
    {
        photonView.RPC(nameof(InitSplitBody), RpcTarget.All, splitCount);

        // 남은 분열체 수 설정
        splitBodyCount++;
        Debug.Log($"Split -> remain: {splitBodyCount} / splitCount: {splitCount}");

        // 보스 정보 동기화
        photonView.RPC(nameof(AsyncSplitBodyCount), RpcTarget.Others, splitBodyCount);
    }

    [PunRPC]
    private void InitSplitBody(int splitCount)
    {
        this.splitCount = splitCount;

        // 분열체 스텟 설정
        HP = Stat.HP / Mathf.Pow(2, splitCount);
        splitBodySpeed = Stat.HP + splitSpeed * splitCount;
    }

    [PunRPC]
    private void AsyncSplitBodyCount(int count)
    {
        splitBodyCount = count;
    }

    public override void OnMove(Vector2 targetPos)
    {
        float speed = ((splitCount > 0) ? splitBodySpeed : Stat.MoveSpeed) * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed);
    }
}