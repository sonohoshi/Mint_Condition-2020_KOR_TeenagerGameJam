using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingCredit : MonoBehaviour
{
    private readonly string[] endingStrings = new[] {"기획 : 정윤수", "", "프로그래밍 : 강동현, 김선민", "", "일러스트 : 고민지", "", "인게임 스프라이트 : 이영제", "", "Thank you for playing."};

    private Text uiText;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        PrivateSceneManager.Manager.AudioSourceVar.Pause();
        uiText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(RollingCredit());
    }

    IEnumerator RollingCredit()
    {
        foreach (var str in endingStrings)
        {
            uiText.text = str;
            if (!str.Equals(""))
            {
                audioSource.Play();
                audioSource.pitch += 0.2f;
            }
            yield return new WaitForSeconds(2f);
        }

        Application.Quit();
    }
}
