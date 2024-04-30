using Photon.Pun;
using UnityEngine;

public class HUD : MonoBehaviourPun
{
    [Header("참조 스크립트")]
    [SerializeField] private HealthUI healthUI;

    // 참조 데이터
    private PlayerData _stat;
    private PlayerData Stat
    {
        get
        {
            if (_stat == null)
                _stat = LocalPlayerData.Instance.PlayerData;

            return _stat;
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateHP()
    {
        float currentHP = Stat.HP;
        float maxHP = Stat.MaxHP;

        healthUI.UpdateHP(currentHP, maxHP);
    }
}