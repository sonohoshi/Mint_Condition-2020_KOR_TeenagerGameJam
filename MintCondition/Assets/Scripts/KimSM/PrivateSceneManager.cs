using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrivateSceneManager : MonoBehaviour
{
    public static PrivateSceneManager Manager;
    public AudioClip RealBGM;
    public AudioClip DreamBGM;
    public GameObject thisObj;
    public int nowStage = 1;
    public int nowCutScene = 1;
    public bool isStoryTelling;

    public AudioSource AudioSourceVar;
    private float nowTime;
    
    void Awake()
    {
        isStoryTelling = false;
        thisObj = GameObject.FindWithTag("SceneManager");
        AudioSourceVar = GetComponent<AudioSource>();
        if (thisObj == null)
        {
            AudioSourceVar.clip = RealBGM;
            gameObject.tag = "SceneManager";
            nowStage = 1;
            thisObj = gameObject;
            Manager = this;
            DontDestroyOnLoad(thisObj);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R) && !isStoryTelling)
        {
            switch (nowStage)
            {
                case 1 when nowCutScene == 3:
                case 2 when nowCutScene == 5:
                    nowCutScene--;
                    break;
            }
            AudioSourceVar.Pause();
            SetBGMReal();
            AudioSourceVar.Play();
            SceneManager.LoadScene($"Stage_{nowStage}");
        }
    }

    public void SetBGMDream()
    {
        AudioSourceVar.clip = DreamBGM;
    }

    public void SetBGMReal()
    {
        AudioSourceVar.clip = RealBGM;
    }
}
