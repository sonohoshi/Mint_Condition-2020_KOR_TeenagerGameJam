using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrivateSceneManager : MonoBehaviour
{
    public static PrivateSceneManager Manager;
    public GameObject thisObj;
    public int nowStage = 1;
    public int nowCutScene = 1;
    public bool isStoryTelling;

    private float nowTime;
    
    void Awake()
    {
        isStoryTelling = false;
        thisObj = GameObject.FindWithTag("SceneManager");
        if (thisObj == null)
        {
            gameObject.tag = "SceneManager";
            nowStage = 1;
            thisObj = gameObject;
            Manager = this;
            DontDestroyOnLoad(thisObj);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            switch (nowStage)
            {
                case 1 when nowCutScene == 3:
                case 2 when nowCutScene == 5:
                    nowCutScene--;
                    break;
            }

            SceneManager.LoadScene($"Stage_{nowStage}");
        }
    }
}
