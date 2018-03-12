using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyKeyText : MonoBehaviour 
{
    /* 필드  */
    //해당 Text GameObject
    //RectTransform 으로 위 아래 Position 지정
    //TitleDownFade의 titlePosition의 position을 저장할 필드
    Text text;

    public GameObject titlePosition;
    public float title;
    public bool isKeyDown;
    /* 메소드 */
    //코루틴 이용
    //타이틀 다 내려올때까지 enabled= false 하고 있다가 다 내려오면 enabled = true로 바뀌고 위 아래 계속 움직이는 코루틴
    //아무키나 누르면 enabled = false

    private void Start()
    {
        text = GetComponent<Text>(); 
        text.enabled = false;
        StartCoroutine(TextUpDown());  
    }

    private void Update()
    {
        title = titlePosition.GetComponent<RectTransform>().localPosition.y; 

        if(title <= 100.0f)
        {
            text.enabled = true;
             if(Input.anyKey)
             {
                 text.gameObject.SetActive(false);
                isKeyDown = true;
             }

        }
    }

    IEnumerator TextUpDown()
    {
            while(true)
            {
                if(isKeyDown == true)
                {
                    break;
                }
                    text.text = "";
                    yield return new WaitForSeconds (1.0f);

                    text.text = "Press Any Key";
                    yield return new WaitForSeconds (1.0f);
            }    
    }
	
}
