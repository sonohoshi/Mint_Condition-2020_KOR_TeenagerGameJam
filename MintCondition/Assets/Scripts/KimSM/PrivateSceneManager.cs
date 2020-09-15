using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateSceneManager : MonoBehaviour
{
    public static PrivateSceneManager SceneManager;
    public int nowStage;
    
    void Awake()
    {
        nowStage = 1;
        SceneManager = this;
        DontDestroyOnLoad(gameObject);
    }

    void LateUpdate()
    {
        
    }
}
