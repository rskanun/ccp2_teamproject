using TMPro;
using UnityEngine;

public class ExpUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private TextMeshProUGUI tmpUI;

    // 게임 데이터
    private GameData gameData;

    private void Start()
    {
        gameData = GameData.Instance;
    }

    public void UpdateUI()
    {
        int level = gameData.Level;
        int exp = gameData.Exp;
        int requireExp = gameData.RequireExp;

        tmpUI.text = "Exp : " + exp + " / " + requireExp;
    }
}