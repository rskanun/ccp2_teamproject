using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSearchBar : MonoBehaviour
{
    public TMP_InputField roomSearchBar;
    public GameObject txtDeleteBtn;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void DeleteTxt()
    {
        roomSearchBar.text = "";
    }
}
