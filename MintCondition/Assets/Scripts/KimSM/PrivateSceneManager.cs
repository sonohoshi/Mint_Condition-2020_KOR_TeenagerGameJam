using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateSceneManager : MonoBehaviour
{
    public static PrivateSceneManager SceneManager;
    public GameObject thisObj;
    public int nowStage = 1;
    
    void Awake()
    {
        thisObj = GameObject.FindWithTag("SceneManager");
        if (thisObj == null)
        {
            gameObject.tag = "SceneManager";
            nowStage = 1;
            thisObj = gameObject;
            SceneManager = this;
            DontDestroyOnLoad(thisObj);
        }
    }
}
