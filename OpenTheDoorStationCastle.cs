using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class  OpenTheDoorStationCastle : MonoBehaviour 
{
	void OnTriggerStay2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player" && Input.GetAxisRaw("Vertical") > 0.0f)
		{
			SceneManager.LoadScene("Lobby");
		}
	}
}
