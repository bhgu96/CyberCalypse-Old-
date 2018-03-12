using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  CloudMoving : MonoBehaviour 
{
	/* 필드 */
	//구름이 움직일 속도
	//구름 Rigidbody2D
	//일정시간동안 코루틴 중지 시키는 필드
	//구름이 움직일 경우의 수 2가지
	public float cloudSpeed;
	public float dirCount;
	public float delayTime = 0;
	Rigidbody2D cloudrigid;

	/* 메소드  */
	//코루틴으로 계속 움직이게 하기
	//코루틴 - 처음 시작할때 방향을 제시해 주고 딜레이타임을 중가시킨다. 딜레이 타임이 5초보다 클때까지 방향이 바뀌지 않는다. 5초가 지나면 다시 0초로 딜레이타임을 초기화 시켜주고 방향을 바꿔준다
	private void Start()
	{
		cloudrigid = GetComponent<Rigidbody2D>();
	}
	private void Update()
	{
			StartCoroutine(cloudMoveSpeed());	
	}

	IEnumerator cloudMoveSpeed()
	{
		if(delayTime == 0 || delayTime > 5.0f) // 처음 코루틴 시작하여 방향 제시, 5초동안 방향 안바뀜
		{
			delayTime = 0;
			while(true) //0이 나오면 다시 랜덤을 돌려서 -1또는 1이 나오게 함
			{
				dirCount = Random.Range(-1,2);

				if(dirCount != 0)
				{
					break;
				}

			}	
		}

		delayTime += Time.deltaTime;

		if(dirCount == -1)
		{
			cloudrigid.velocity = new Vector2(+cloudSpeed,cloudrigid.velocity.y);
		}
		else if(dirCount == 1)
		{
			cloudrigid.velocity = new Vector2(-cloudSpeed,cloudrigid.velocity.y);
		}	

		yield return null;	
	}
}
