using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class BuffArea : MonoBehaviour
{
    // 현재 스킬 상세 정보
    private Player caster;

    private Dictionary<string, GameObject> inAreaObjs = new Dictionary<string, GameObject>();

    public void SetCaster(Player caster)
    {
        this.caster = caster;
    }

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        caster.SetBuffSTR(caster.PlayerData.STR * 0.05f);
    }

    private void OnDisable()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        caster.SetBuffSTR(0);

        foreach (GameObject player in inAreaObjs.Values)
        {
            player.GetComponent<Player>().SetBuffSTR(0);
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
            inAreaObjs.Add(collision.name, collision.gameObject);

            float increaseSTR = caster.PlayerData.STR * 0.05f;

            Player player = collision.GetComponent<Player>();
            player.SetBuffSTR(increaseSTR);
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
            inAreaObjs.Remove(collision.name);

            Player player = collision.GetComponent<Player>();
            player.SetBuffSTR(0);
        }
    }
}