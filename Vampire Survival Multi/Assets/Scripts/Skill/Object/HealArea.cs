using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour
{
    [Header("효과 적용 딜레이")]
    [SerializeField]
    private float delay;
    private float curDelay;

    // 현재 스킬 상세 정보
    private Player caster;

    private Dictionary<string, GameObject> inAreaObjs = new Dictionary<string, GameObject>();

    public void SetCaster(Player caster)
    {
        this.caster = caster;

        // 본인도 효과 적용
        inAreaObjs.Add(caster.name, caster.gameObject);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (caster != null && WaveData.Instance.IsRunning)
        {
            if (curDelay <= 0)
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
        }
    }
}