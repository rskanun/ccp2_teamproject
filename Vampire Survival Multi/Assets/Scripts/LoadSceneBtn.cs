using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneBtn : MonoBehaviour
{
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void loadScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

}
