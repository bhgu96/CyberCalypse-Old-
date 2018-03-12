using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStopInStationStart : MonoBehaviour 
{
	/* 필드 */
	//자식인 HP Bar도 비활성화 되어있어야 한다.
	//SetActive를 이용하여 자식인 HP는 처음엔 비활성화
	public float titleAlphaColor;
	Rigidbody2D rigid2d;
	SpriteRenderer playerRender;
	public GameObject  inactiveChildObject;

	/* 메서드 */

	//title의 alpha가 0이 되면 그때 player의 sprite 활성화, gravityscale = 2.5, HPBar 활성화
	private void Start()
	{
		playerRender = GetComponent<SpriteRenderer>();
		rigid2d = GetComponent<Rigidbody2D>();

		rigid2d.isKinematic = true;
		inactiveChildObject.gameObject.SetActive(false);
	}
	private void Update()
	{
		titleAlphaColor = TitleDownFade.titleAlpha;
		if(TitleDownFade.titleAlpha > 0.0f)
		{
			playerRender.enabled = false;
			rigid2d.gravityScale = 0f;	
		}
		else if(TitleDownFade.titleAlpha <= 0.0f)
		{
			playerRender.enabled = true;
			inactiveChildObject.gameObject.SetActive(true);
			rigid2d.isKinematic = false;
			rigid2d.gravityScale = 2.5f;	
		}
	}
	
}
