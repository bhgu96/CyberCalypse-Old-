using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHPBar : MonoBehaviour 
{
	/* 필드  */
	//플레이어 HP
	//FrontHP 오브젝트와 BackHP 오브젝트
	//플레이어 BackHp 다는 시간
	public static float reduceHP = 10.0f;
	public static float maxHP = 100.0f;
	public Image FrontHP;
	public Image BackHp;



	/* 메소드 */
	//oncollision2d 메소드를 이용
	//FrontHP는 바로 감소
	//BackHP는 서서히 감소(코루틴 이용)
	//FrontHP와 BackHP는 서로 감소하는 양이 똑같음
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "SpearHit")
		{
			maxHP -= reduceHP;
			FrontHP.fillAmount -= reduceHP * 0.01f;

			StartCoroutine(BackHPReduce());
		}
	}

	IEnumerator BackHPReduce()
	{
		if(BackHp.fillAmount <= FrontHP.fillAmount)
		{
			yield break;
		}

		yield return new WaitForSeconds(0.15f);
		BackHp.fillAmount -= reduceHP * 0.001f;
		StartCoroutine(BackHPReduce());
	}
}
