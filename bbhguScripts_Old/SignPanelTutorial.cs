using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SignKinds // 튜토리얼 표지판 종류의 열거형
{
    AttackSign,
    JumpSign,
    DashSign,
    TrapSign
}

public class SignPanelTutorial : MonoBehaviour 
{
	/* 필드  */
    //표지판의 열거형을 이용해서 표지판 각각의 특징을 갖게 함
    //collision 대상인 player를 담는 player 게임오브젝트
    //tutorial 이미지들 1. AttackSignImage 2. JumpSignImage 3. DashSignImage 4. TrapSignImage
    public SignKinds signKinds;
    public GameObject player;
    public SpriteRenderer AttackSignImage;
    public SpriteRenderer JumpSignImage;
    public SpriteRenderer DashSignImage;
    public SpriteRenderer TrapSignImage;

    public GameObject AttackSignPanelPosition;
    public GameObject JumpSignPanelPosition;
    public GameObject DashSignPanelPosition;
    public GameObject TrapSignPanelPosition;

    public static bool isCollisionJumpPanel;
	public static bool isCollisionTrapPanel;


    /* 메소드 */
    //열거형에 따라 메소드를 다르게 한다(1.AttackTutorial, 2. JumpTutorial 3. DashSignImage 4. TrapSignImage)
    //Player와 부딪혔을떄 Oncollision2d를 통해 표지판 Instantiate

    void Start()
    {
        AttackSignImage.enabled = false;
        JumpSignImage.enabled = false;
        DashSignImage.enabled = false;
        TrapSignImage.enabled = false;

        isCollisionJumpPanel = false;
        isCollisionTrapPanel = false;   
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       if(signKinds == SignKinds.AttackSign)
       {
           if(col.gameObject == player)
           {
               AttackSignImage.enabled = true;
           }
        
       }

       else if(signKinds == SignKinds.JumpSign)
       {
           if(col.gameObject == player)
           {
               if(isCollisionTrapPanel == true) //trap부분에 체크포인트 찍었으면 이전 체크포인트인 jump에서 리스폰 되면 안된다
               {
                   return;
               }
               isCollisionJumpPanel = true;
               JumpSignImage.enabled = true;
           }
       }

       else if(signKinds == SignKinds.DashSign)
       {
           if(col.gameObject == player)
           {
               DashSignImage.enabled = true;
           }
       }

       else if(signKinds == SignKinds.TrapSign)
       {
           if(col.gameObject == player)
           {
               isCollisionJumpPanel = false;
               isCollisionTrapPanel = true;
               TrapSignImage.enabled = true;
           }
       }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        AttackSignImage.enabled = false;
        JumpSignImage.enabled = false;
        DashSignImage.enabled = false;
        TrapSignImage.enabled = false;
    }

}
