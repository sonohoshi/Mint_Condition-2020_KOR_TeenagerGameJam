using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrivateSceneManager : MonoBehaviour
{
    public static PrivateSceneManager Manager;
    public GameObject thisObj;
    public int nowStage = 1;

    private float nowTime;
    
    void Awake()
    {
        thisObj = GameObject.FindWithTag("SceneManager");
        if (thisObj == null)
        {
            gameObject.tag = "SceneManager";
            nowStage = 2;
            thisObj = gameObject;
            Manager = this;
            DontDestroyOnLoad(thisObj);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene($"Stage_{nowStage}");
        }
    }
}
