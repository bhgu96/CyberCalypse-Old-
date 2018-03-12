using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDownFade : MonoBehaviour
{
	/* 필드  */
	//title 내려오는 time, 그리고 fade 효과를 주기위한 alpha값 필드, title을 넣을 인스턴스
	RectTransform rect;
	public Image title;
	public float downSpeed;
	public static float titleAlpha;
	public bool isKeyDown;

	/* 메소드 */

	//코루틴 2개
	//1.title 내려오는 코루틴, 2.title fade 효과 코루틴
	//1. downSpeed 만큼 계속 내려오기
	//2. 1.끝나고 -time.deltatime 만큼 fade 효과
	private void Start()
	{
		titleAlpha = 1.0f;
		rect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if(Input.anyKey)
		{
			isKeyDown = true;
		}

		StartCoroutine(TitleDown());	
	}

	IEnumerator TitleDown()
	{	
		if(rect.localPosition.y > 100)
		{	
		rect.localPosition += Vector3.down*downSpeed;
		}
		
		else if(rect.localPosition.y <= 100.0f && isKeyDown == true)
		{
			StartCoroutine(TitleFadeOut());
			yield break;
		}
	}

	IEnumerator TitleFadeOut()
	{
		if(titleAlpha > 0.0f)
		{
		titleAlpha -= Time.deltaTime;
		title.color = new Color(1,1,1,titleAlpha);
		}
		
		else
		{
			yield break;
		}
	}

}
