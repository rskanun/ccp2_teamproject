using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void OnTakeDamage(int damage)
    {
        // 공격 받았을 때
        Debug.Log("피해를 입었습니다: " +  damage);
    }
}