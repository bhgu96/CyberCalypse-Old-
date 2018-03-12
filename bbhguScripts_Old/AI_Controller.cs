using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour 
{
//public GameObject playerDash; /*구용모 추가 대쉬 이펙트 게임 오브젝트 */
    public BoxCollider2D hitBox; /* 구용모 추가 hitbox */
    public static bool isClimbLadder; /* 구용모 추가 사다리 오르기 체크 */
    public float dashTime;  /*구용모 추가 대쉬 이펙트를 위한 dashtime. dashtime이 0보다 작아지면 dash 멈춤 */
	public float playerHP;
    public float playerReduceHP;

	public Vector2 PlayerPosition;
	public Vector2 AiPosition;
    public float moveSpeed;
    public float jumpSpeed;
	public int AiMovement; // ai이동(0이면 오른쪽으로, 1이면 왼쪽으로, 2이면 아래로, 3이면 위쪽으로(또는 점프), 4이면 대쉬)

    public Transform groundChecker;
	public GameObject Target;
    public float groundCheckerRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;
	public bool isTracingTarget;

    public Rigidbody2D m_rigidbody2D;
    public Animator m_animator;

    private void Start()
    {
        dashTime = 0.5f;
        moveSpeed = 5.0f;
        jumpSpeed = 12.0f;

        groundCheckerRadius = 0.25f;

		playerReduceHP = PlayerHPBar.reduceHP;
        playerHP = 100;

        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        hitBox.enabled = false;
        isClimbLadder = false;
    }

    private void Update()
    {
		PlayerPosition = Target.transform.position;
		AiPosition = this.transform.position;

		if(AiPosition.x > PlayerPosition.x) //왼쪽 이동
		{
			if(PlayerPosition.y - AiPosition.y > 1.0f) //점프 또는 위로 이동
			{
				AiMovement = 3;
				StartCoroutine(ActiveAI());
			}
			
			/*else if(AiPosition.x - PlayerPosition.x > 10.0f) //대쉬
			{
				AiMovement = 4;
				StartCoroutine(ActiveAI());
			}*/

			AiMovement = 1;
			StartCoroutine(ActiveAI());
		}
		
		else if(AiPosition.x < PlayerPosition.x) //오른쪽 이동
		{
			if(PlayerPosition.y - AiPosition.y > 1.0f) //점프 또는 위로 이동
			{
				AiMovement = 3;
				StartCoroutine(ActiveAI());
			}

			/*else if(AiPosition.x - PlayerPosition.x > 1.0f) //대쉬
			{
				AiMovement = 4;
				StartCoroutine(ActiveAI());
			}*/

			AiMovement = 0;
			StartCoroutine(ActiveAI());
		}
    }
	IEnumerator ActiveAI()
	{
		isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, whatIsGround);

        // 이동(Move)
        if(AiMovement == 0) //오른쪽
        {
            m_rigidbody2D.velocity = new Vector3(+moveSpeed, m_rigidbody2D.velocity.y, 0.0f);
            this.transform.localScale = new Vector3(+1.0f, 1.0f, 1.0f);
        }
        else if(AiMovement == 1) //왼쪽
        {
            m_rigidbody2D.velocity = new Vector3(-moveSpeed, m_rigidbody2D.velocity.y, 0.0f);
            this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);    
        }
        /*else if (AiMovement == 4 && isGrounded)//점프 대쉬 방지, 대쉬
        {
            StartCoroutine(PlayerDash()); // 코루틴으로 dash effect
            yield break;
        }*/
        else
        {
            m_rigidbody2D.velocity = new Vector3(0.0f, m_rigidbody2D.velocity.y, 0.0f);
        }

		//점프
        if(AiMovement == 3 && isGrounded)
        {
            m_rigidbody2D.velocity = new Vector3(m_rigidbody2D.velocity.x, jumpSpeed, 0.0f);
        }

        m_animator.SetFloat("moveSpeed", Mathf.Abs(m_rigidbody2D.velocity.x));
        m_animator.SetBool("isGrounded", isGrounded);

		yield break;
	}

    /*IEnumerator PlayerDash()
    {
        while(true)
        {
            if(dashTime < 0.0f) //dashtime이 0보다 작아지면 다시 0.5초로 갱신
            {
                dashTime = 0.5f;
                break;
            }
            dashTime -= Time.deltaTime;
            
            if(this.transform.localScale.x > 0.0f)
            {
                m_rigidbody2D.velocity = new Vector3(+moveSpeed*2, m_rigidbody2D.velocity.y, 0.0f);
                GameObject dash = Instantiate(playerDash,transform.position,transform.rotation) as GameObject; //플레이어의 dash effect clone 생성
            }

            else if(this.transform.localScale.x < 0.0f)
            {
                m_rigidbody2D.velocity = new Vector3(-moveSpeed*2, m_rigidbody2D.velocity.y, 0.0f);
                GameObject dash = Instantiate(playerDash,transform.position,transform.rotation) as GameObject; //플레이어의 dash effect clone 생성
            }

            yield return null;
        }
    }*/

    
    void OnCollisionEnter2D(Collision2D col)
    {
        /* 사다리 collision */
        

        /* 피격 collision */
        if(col.gameObject.tag == "Player")
        {
			m_animator.SetTrigger("UpperAttacking");
            hitBox.enabled = true;
            Physics2D.IgnoreLayerCollision(9,9);
        }

        else if(col.gameObject.tag == "SpearHit")
		{
			m_animator.SetTrigger("UpperAttacking");
            hitBox.enabled = true;
			playerHP -= playerReduceHP;
			//경직,무적 코루틴,hp감소
		}
    }

	void OnTriggerStay2D(Collider2D col) // 공격 범위 내로 들어오면 공격
	{
		if(col.gameObject.tag == "Player")
		{
			m_animator.SetTrigger("UpperAttacking");
            hitBox.enabled = true;
		}

		 if(playerHP <= 0.0f)
        {
            Destroy(this.gameObject);
        }
	}

	void OnTriggerExit2D(Collider2D other) //공격 범위 나가면 공격 안함
	{
		hitBox.enabled = false;
	}
	
}
