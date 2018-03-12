using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  GameExit : MonoBehaviour 
{
	public GameObject Player;

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject == Player)
		{
			Debug.Log("exit");
			Application.Quit();

			//유니티 에디터도 종료
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
	}

}
