using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftBulletUI : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }
    
    void LateUpdate()
    {
        text.text = $"남은 총알 : {GameManager.Instance.MaxBullets[PrivateSceneManager.Manager.nowStage - 1]}";
    }
}
