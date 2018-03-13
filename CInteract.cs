using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInteract : MonoBehaviour
{
    bool isCollideInteractObject;
    Collider2D col;

    //Equipment 객체에 해당하도록 할당을 한다.

    private void Start()
    {
        InputManager.instance.Interact += Interact;
    }

    public void Interact(bool isDownInteractKey)
    {
        if(!isCollideInteractObject)
        {
            return;
        }
        else
        {
            col.gameObject.SetActive(false);
            Debug.Log("Get Item or Interact with Object");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        this.col = col;

        if(col.gameObject.layer == 18 || col.gameObject.layer == 19) //상호작용 할 수 있는 아이템 또는 오브젝트에 부딪쳤을때
        {
            isCollideInteractObject = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        this.col = null;

        if (col.gameObject.layer == 18 || col.gameObject.layer == 19) //상호작용 할 수 있는 아이템 또는 오브젝트의 콜리더를 나왔을때
        {
            isCollideInteractObject = false;
        }
    }

}
