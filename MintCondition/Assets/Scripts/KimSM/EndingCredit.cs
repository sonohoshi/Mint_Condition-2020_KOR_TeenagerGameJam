using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingCredit : MonoBehaviour
{
    private readonly string[] endingStrings = new[] {"기획 : 정윤수", "", "프로그래밍 : 강동현, 김선민", "", "일러스트 : 고민지", "", "인게임 스프라이트 : 이영제", "", "Thank you for playing."};

    private Text uiText;
    
    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<Text>();
    }

    IEnumerator RollingCredit()
    {
        foreach (var str in endingStrings)
        {
            
        }
        yield return null;
    }
}
