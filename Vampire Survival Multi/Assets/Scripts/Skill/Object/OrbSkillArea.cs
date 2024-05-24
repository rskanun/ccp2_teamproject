using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class OrbSkillArea : MonoBehaviourPun
{
    [Header("각 버프별 오브젝트")]
    [SerializeField] private GameObject healZone;
    [SerializeField] private GameObject buffZone;

    [Header("효과 적용 딜레이")]
    [SerializeField]
    private float delay;
    private float curDelay;

    // 현재 스킬 상세 정보
    private Player caster;
    private bool isHealZone;

    private Dictionary<string, GameObject> inAreaObjs = new Dictionary<string, GameObject>();

    public void SetCaster(Player caster)
    {
        this.caster = caster;

        // 본인도 효과 적용
        inAreaObjs.Add(caster.name, caster.gameObject);
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

        if (isHealZone == false)
        {
            caster.SetBuffSTR(caster.PlayerData.STR * 0.05f);
        }
        else
        {
            caster.SetBuffSTR(0);
        }
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

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (caster != null && WaveData.Instance.IsRunning)
        {
            if (curDelay <= 0 && isHealZone)
            {
                List<GameObject> targetList = new List<GameObject>(inAreaObjs.Values);

                float heal = caster.PlayerData.STR * 0.05f;

                foreach (GameObject target in targetList)
                {
                    Player player = target.GetComponent<Player>();

                    player.HealHP(heal);
                }

                curDelay = delay;
            }
            else
            {
                curDelay -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (WaveData.Instance.IsRunning && collision.CompareTag("Player"))
        {
            string name = collision.name;
            GameObject obj = collision.gameObject;

            inAreaObjs.Add(name, obj);

            if (isHealZone == false)
            {
                float increaseSTR = caster.PlayerData.STR * 0.05f;

                Player player = collision.GetComponent<Player>();
                player.SetBuffSTR(increaseSTR);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (WaveData.Instance.IsRunning && collision.CompareTag("Player"))
        {
            string name = collision.name;

            inAreaObjs.Remove(name);

            if (isHealZone == false)
            {
                Player player = collision.GetComponent<Player>();
                player.SetBuffSTR(0);
            }
        }
    }
}