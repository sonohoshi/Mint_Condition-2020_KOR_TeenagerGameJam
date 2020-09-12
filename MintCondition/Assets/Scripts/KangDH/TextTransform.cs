using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextTransform : MonoBehaviour
{

    public Text ChatText; // 실제 채팅이 나오는 텍스트
    public string writerText = "";
    void Start()
    {
        StartCoroutine(TextPractice());
    }

    IEnumerator NormalChat(string narration)
    {
        int a = 0;
        writerText = "";
        for (a = 0; a < narration.Length; a++)
        {
            if (Input.anyKey)
            {
                ChatText.text = narration;
                break;
            }
            else
            {
                writerText += narration[a];
                ChatText.text = writerText;
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    IEnumerator TextPractice()
    {
        yield return StartCoroutine(NormalChat("텍스트 1텍스트 1텍스트 1텍스트 1텍스트 1텍스트 1"));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(NormalChat("텍스트 2텍스트 2텍스트 2텍스트 2텍스트 2텍스트 2"));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(NormalChat("텍스트 3텍스트 3텍스트 3텍스트 3텍스트 3텍스트 3"));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(NormalChat("텍스트 4텍스트 4텍스트 4텍스트 4텍스트 4텍스트 4"));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(NormalChat(" "));
    }
}


//
//            if (isButtonClicked)
//            {
//                ChatText.text = narration;
//                a = narration.Length; // 버튼 눌리면 그냥 다 출력하게 함
//                isButtonClicked = false;
//            }