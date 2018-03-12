using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCrowHitted : MonoBehaviour 
{
	/* 필드 */
	//Swaying 애니메이션을 발동할 animator 필드
	//hitted할 게임오브젝트
	//10대 이상 때리면 destroy 할 count 필드
	public Animator scare_animator;
	public float crowHP;
	public float crowReduceHP;

	/* 메소드 */
	// player에 hitted 될때마다 Swaying 애니메이션 발동

	void Start()
	{
		crowReduceHP = PlayerHPBar.reduceHP;
		crowHP = PlayerHPBar.maxHP;
		scare_animator = GetComponent<Animator>();
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "SpearHit")
		{
			crowHP -= crowReduceHP;
			scare_animator.SetTrigger("Swaying");
		}
		
		if(crowHP <= 0.0f)
		{
			Destroy(this.gameObject);
		}
	}

}
