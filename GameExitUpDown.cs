using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  GameExitUpDown : MonoBehaviour 
{
	/* 필드 */
	//GameExit이 있는 Position 받을 필드
	//GameExit이 내려갔다 올라갔다 할 시간 upDownTime
	//올라가는것을 확인하는 bool 필드
	//내려가는것을 확인하는 bool 필드
	Transform exitPosition;
	public float upDownTime = 0;
	public float upDownSpeed;
	public bool isUp;
	public bool isDown;


	/* 메소드 */
	//GameExit이 계속 움직일 코루틴
	//upDownTime이 계속 작아져서 -0.05이 될때까지(이때는 밑으로 내려가기)
	//upDownTime이 계속 커져서 0이 될때까지(이때는 위로 올라가기)
	//-0.05보다 작아지면 올라가는 bool = true, 내려가는 bool = false;
	//0보다 커지면 올라가는 bool = false, 내려가는 bool = true;
	void Start()
	{
		isUp = false;
		isDown = true;

		exitPosition = GetComponent<Transform>();
	}

	void Update()
	{
		StartCoroutine(ExitUpDown());
	}

	IEnumerator ExitUpDown()
	{
		if(isDown)
		{
			upDownTime -= Time.deltaTime;
		}
		else if(isUp)
		{
			upDownTime += Time.deltaTime;
		}
		
		if(upDownTime < 0 && isDown)
		{
			exitPosition.localPosition += Vector3.down*upDownSpeed;
		}
		else if(upDownTime > -0.5 && isUp)
		{
			exitPosition.localPosition += Vector3.up*upDownSpeed;
		}

		if(upDownTime < -0.5f)
		{
			isDown = false;
			isUp = true;
			yield break;
		}

		else if(upDownTime > 0)
		{
			isDown = true;
			isUp = false;
			yield break;
		}		
	}
}
