using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextTransform : MonoBehaviour
{
    public Text ChatText; // 실제 채팅이 나오는 텍스트
    public Text charatorText;
    private string writerText = "";
    private string writerTextName = "";
    public int forText = 1;
    private int cutScene = 1;
    public AudioManager audioMg;
    public AudioClip textSound;

    public Image backGround;
    public Sprite[] cut1Image;
    public Sprite[] cut2Image;
    public Sprite[] cut3Image;
    public Sprite[] cut4Image;
    public Sprite[] endingImage;
    void Start()
    {
        cutScene = PrivateSceneManager.Manager.nowStage;
        StartCoroutine(TextPractice());
        Debug.Log(cutScene);
    }

    IEnumerator NormalChat(string narration, string narraitor = "- -")
    {
        int a = 0;
        writerText = "";
        writerTextName = narraitor;
        charatorText.text = writerTextName;
        for (a = 0; a < narration.Length; a++)
        {
            if (Input.anyKey)
            {
                ChatText.text = narration;

            }
            else
            {
                audioMg.PlayAudio(textSound);
                writerText += narration[a];
                ChatText.text = writerText;
                yield return new WaitForSeconds(0.07f);
            }
        }
    }

    IEnumerator TextPractice()
    {
        yield return new WaitForSeconds(0.5f);
        if (cutScene == 1)
        {
            backGround.sprite = cut1Image[0];
            yield return StartCoroutine(NormalChat("나는 X, 데니의 암살자다."));
            yield return new WaitForSeconds(1.0f);
            backGround.sprite = cut1Image[1];
            yield return StartCoroutine(NormalChat("세계는 매우 혼란한 상태다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("나는 혼란한 세계를 진정시키기 위해"));
            yield return new WaitForSeconds(1.0f);
            backGround.sprite = cut1Image[2];
            yield return StartCoroutine(NormalChat("많은 사람들의 목숨을 지키기 위해"));
            yield return new WaitForSeconds(1.0f);
            backGround.sprite = cut1Image[3];
            yield return StartCoroutine(NormalChat("이 모든 일을 끝내고 편해지기 위해"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("하루하루 죄책감과 함께"));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("암살자로써의 생활을 이어나가고 있다."));
            yield return new WaitForSeconds(1.0f);
            backGround.sprite = cut1Image[4];
            yield return StartCoroutine(NormalChat("그런 날들이 오늘도 시작된다."));
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Stage_1");
            // Scene 이동
        }
        if(cutScene == 2)
        {
            /*
 * Cut Scene 2.
"허억..허억.."/"ㅈ..잠깐만!"/타겟인 그가 말을 걸었다/"만약 네놈이 데니의 암살자라면, 네가 알고있는 것은 거짓이야!"/
무슨 이야기를 하는거지?/"너는 세계 평화를 위해서도, 사람들의 목숨을 지키기 위해서 싸우는 것도 아니야!"/"
너는 그저 어릴 적 거둬들여져 그들에게 세뇌당한거야!!!"/.../임무 완수를 위해 그를 죽였다*/
            backGround.sprite = cut2Image[0];
            yield return StartCoroutine(NormalChat("\"허억..허억..\"","-적1-"));
            yield return new WaitForSeconds(1.0f);
            backGround.sprite = cut2Image[1];
            yield return StartCoroutine(NormalChat("\"ㅈ..잠깐만!\"","-적1-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("타겟인 그가 말을 걸었다."));
            yield return new WaitForSeconds(1.0f);
            backGround.sprite = cut2Image[2];
            yield return StartCoroutine(NormalChat("\"만약 네놈이 데니의 암살자라면, \"","-적1-"));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("\"네가 알고있는 것은 거짓이야!\"", "-적1-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"무슨 이야기를 하는거지?\"","\"X\""));
            yield return new WaitForSeconds(1.0f);
            backGround.sprite = cut2Image[3];
            yield return StartCoroutine(NormalChat("\"너는 세계 평화를 위해서도, \"","\"적1\""));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("\"사람들의 목숨을 지키기 위해서 \"", "\"적1\""));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("\"싸우는 것도 아니야! \"", "-적1-"));
            yield return new WaitForSeconds(0.3f);
            
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"너는 그저 어릴 적 \"","\"적1\""));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("\"거둬들여져 그들에게 세뇌당한거야!!!\"", "-적1-"));
            yield return new WaitForSeconds(1.0f);
            backGround.sprite = cut2Image[4];
            yield return StartCoroutine(NormalChat("\"...\"","\"X\""));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("임무 완수를 위해 그를 죽였다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("..."));
            yield return new WaitForSeconds(.3f);
            yield return StartCoroutine(NormalChat("오늘 잠자리도 사납겠구나..."));
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Stage_2");
        }
        if(cutScene == 3)
        {

            /*
 * Cut Scene 3.
"허억..허억..."/어느 때와 같이 내가 저지른 일에 대한 악몽을 꾸었다/"세뇌..."/
타겟이 했던 말에 신뢰가 간다/이제껏 내가 의심해 왔던, 어딘가 이상했던 부분들이 모두 설명이 된다/
무엇보다 나는 이 지긋지긋한 일을 그만두고 싶다/나의 모든 것을 꿰뚫고 이용하는 그들의 족쇄에서 벗어나고싶다/
다음날 나는, 타겟이 아닌 거대 기업 데니로 향한다
*/
            backGround.sprite = cut3Image[0];
            yield return StartCoroutine(NormalChat("\"허억..허억...\"","X"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("어느 때와 같이 "));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("내가 저지른 일에 대한 악몽을 꾸었다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"세뇌...\"","X"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("타겟이 했던 말에 신뢰가 간다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("이제껏 내가 의심해 왔던,"));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(NormalChat("어딘가 이상했던 부분들이 모두 \n설명이 된다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("무엇보다 나는 이 지긋지긋한 일을 \n그만두고 싶다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("나의 모든 것을 꿰뚫고 이용하는 "));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("그들의 족쇄에서 벗어나고싶다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("다음날 나는, "));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("타겟이 아닌 거대 기업 데니로 향한다."));
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Stage_3");
        }
        if(cutScene == 4)
        {

            /*
 Cut Scene 4
 "X!!"/"무슨 짓이야"/"네 목표는 내가 아니라고!!"/나는 그에게 진실을 물었다/
 "ㅁ..뭘원하는거야 네가 아는 모든 것은 진실이야!"/
 "세계는 곧 멸망할거야, 네가 악한 자들의 목을 지금처럼 베어내지 않는다면!!!"/
 나는 그의 신체 일부를 잘라냈다/"으아아아악!!!!"/"알겠어, 알겠다고!"/그가 말을 이어나갔다/
 "넌 세계 평화를 위해서, 사람들을 지키기 위해서 싸우는게 아니야"/"돈, 그래. 돈"/
 "데니가 돈을 벌지 못하게 방해하는 자들을 없애는게 니가 해왔던 임무야"/
"그게 어떤 단체든, 군대든, 국가든말이야"/"이게 내가아는 전부야, 용서해줘 젠장"
*/
            backGround.sprite = cut4Image[0];
            yield return StartCoroutine(NormalChat("\"X!!\"", "-사장-"));
            yield return new WaitForSeconds(0.8f);
            yield return StartCoroutine(NormalChat("무슨 짓이야!", "-사장-"));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(NormalChat("\"네 목표는 내가 아니라고!!\"","-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("나는 그에게 진실을 물었다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"ㅁ..뭘원하는거야!? \"", "-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"네가 아는 모든 것은 진실이야!\"","-사장-"));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(NormalChat("\"세계는 곧 멸망할거야!\"","-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"네가 악한 자들의 목을\"","-사장-"));
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(NormalChat("\"지금처럼 베어내지 않는다면!!!\"","-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("나는 그의 신체 일부를 잘라냈다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"으아아아악!!!!\"","-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"알겠어, 알겠다고!\"","-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("그가 말을 이어나갔다.","-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"넌 세계 평화를 위해서,\"", "-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"사람들을 지키기 위해서 \n싸우는게 아니야.\"", "-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"돈, 그래..돈\"", "-사장-"));
            yield return new WaitForSeconds(0.7f);
            yield return StartCoroutine(NormalChat("\"데니가 돈을 벌지 못하게 방해하는 자들을\"", "-사장-"));
            yield return new WaitForSeconds(0.8f);
            yield return StartCoroutine(NormalChat("\"없애는게 니가 해왔던 임무야.\"", "-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"그게 어떤 단체든, 군대든, 국가든말이야.\"", "-사장-"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("\"이게 내가아는 전부야, 용서해줘 젠장!\"", "-사장-"));
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Stage_4");
        }
        if (cutScene == 5)
        {

            /*
 Ending Scene
 끝났다/몇십년간 도구로써 사용되었던 나는, 스스로 자유를 얻었다/
 더럽고 피비린내 세계와 등지고 걸어나간다/밝은 곳을 향해 걸어간다/
 나는 X다
 */
            backGround.sprite = endingImage[0];
            yield return StartCoroutine(NormalChat("끝났다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("몇십년간 도구로써 사용되었던 나는,"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("스스로 자유를 얻었다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("더럽고 피비린내 세계와"));
            yield return new WaitForSeconds(0.8f);
            yield return StartCoroutine(NormalChat("등지고 걸어나간다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("밝은 곳을 향해 걸어간다."));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("나는"));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(NormalChat("..X다."));
            yield return new WaitForSeconds(1.0f);
        }
    }
}






/* yield return StartCoroutine(NormalChat(""));
    yield return new WaitForSeconds(1.0f);*/


//
//            if (isButtonClicked)
//            {
//                ChatText.text = narration;
//                a = narration.Length; // 버튼 눌리면 그냥 다 출력하게 함
//                isButtonClicked = false;
//            }