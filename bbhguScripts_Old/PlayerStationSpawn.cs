using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStationSpawn : MonoBehaviour 
{
	/* 필드 */
	//플레이어 게임오브젝트
	//JumpTutorialPanel 게임오브젝트
	//TrapTutorialPanel 게임오브젝트

	public GameObject Player;
	public GameObject JumpTutorialPanel;
	public Vector2 JumpTutorialPanelPosition;
	public Vector2 TrapTutorialPanelPosition;
	public GameObject TrapTutorialPanel;

	/* 메소드 */
	//OnTriggerEnter2D로 각각의 Sign 충돌 처리
	//부딪히면 각 Panel의 Position.y +10 만큼의 position으로 Player 재생성

	void Start()
	{
		JumpTutorialPanelPosition = new Vector2(JumpTutorialPanel.transform.position.x, JumpTutorialPanel.transform.position.y +10.0f);
		TrapTutorialPanelPosition = new Vector2(TrapTutorialPanel.transform.position.x, TrapTutorialPanel.transform.position.y +10.0f);
	}

	/* 스폰되는걸 position으로 바꾸는건 어떨까 */
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject == Player)
		{
			if(SignPanelTutorial.isCollisionJumpPanel == true)
			{
				Player.transform.position = JumpTutorialPanelPosition;
			}
			else if(SignPanelTutorial.isCollisionJumpPanel == false && SignPanelTutorial.isCollisionTrapPanel == true)
			{
				Player.transform.position = TrapTutorialPanelPosition;
			}
		}
	}

}
