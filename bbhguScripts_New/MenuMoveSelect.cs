using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoveSelect : MonoBehaviour 
{
	/* 필드 */
	public static MenuMoveSelect MenuInstance;

	/* 메소드 */
	public void Awake()
	{
		MenuInstance = this;

		/*InputManager.move += MenuMove;
		InputManager.unique += MenuSelect;*/
	}

	public void MenuSelect(bool isDownMenuUniqueKey)
	{

	}

	public void MenuMove(float inputMoveValue)
	{
		Debug.Log("Menu Moving");
	}
}
