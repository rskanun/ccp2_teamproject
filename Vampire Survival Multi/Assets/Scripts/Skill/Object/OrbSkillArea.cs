using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class OrbSkillArea : MonoBehaviourPun
{
    [Header("각 버프별 오브젝트")]
    [SerializeField] private GameObject healZone;
    [SerializeField] private GameObject buffZone;

    // 현재 스킬 상세 정보
    private bool isHealZone;

    public void SetCaster(Player caster)
    {
        healZone.GetComponent<HealArea>().SetCaster(caster);
        buffZone.GetComponent<BuffArea>().SetCaster(caster);
    }

    public void InitParent(int parentViewID)
    {
        photonView.RPC(nameof(SetParent), RpcTarget.All, parentViewID);
    }

    [PunRPC]
    private void SetParent(int parentViewID)
    {
        PhotonView parentView = PhotonView.Find(parentViewID);
        if (parentView != null)
        {
            transform.SetParent(parentView.transform, true);
        }
    }

    public void SetBuffType(bool isHealZone)
    {
        this.isHealZone = isHealZone;

        SetActiveZone(isHealZone);
    }

    [PunRPC]
    private void SetActiveZone(bool isHealZone)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(SetActiveZone), RpcTarget.Others, isHealZone);
        }

        healZone.SetActive(isHealZone);
        buffZone.SetActive(!isHealZone);
    }

    public void OnSwitchBuff()
    {
        SetBuffType(!isHealZone);
    }
}