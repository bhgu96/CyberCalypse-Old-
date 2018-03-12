using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEaseliaAni : SingleTonManager<TestEaseliaAni> //Singleton 말고 딴거 쓸 것 수정
{
    private Animator ani;
    private ArcanePhsics phsics;

    private bool isAttackFinish;
    private bool isAttackKeyDown;
    private bool isSpecialAttackKeyDown;

    private IEnumerator attackAniExpiration;

    private new void Awake()
    {
        base.Awake();

        ani = GetComponent<Animator>();
        phsics = GetComponent<ArcanePhsics>();

        attackAniExpiration = AttackAniExpiration();
    }

    private void Start()
    {
        /* 인풋매니저의 인스턴스가 먼저 생성된 다음에 델리게이트로 등록 가능하다. 그래서 Start에서 인풋매니저의 델리게이트에 등록한다.*/
        InputManager.instance.PlayerHMove += EaselHMove;
        InputManager.instance.PlayerVMove += EaselVMove;
        InputManager.instance.Jump += EaselJump;
        InputManager.instance.Dash += EaselDash;
        InputManager.instance.Nomal_Attack += EaselNomal_Attack;
        InputManager.instance.Special_Attack += EaselSpecial_Attack;

        InputManager.instance.HRun += EaselHRun;
    }


    private void Update()
    {
        ani.SetFloat("gravitySpeed", phsics.gravity);
        ani.SetBool("isGrounded", phsics.isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(int.Equals(col.gameObject.layer,10) || int.Equals(col.gameObject.layer, 11) || int.Equals(col.gameObject.layer, 12) || int.Equals(col.gameObject.layer, 13))
        {
            ani.SetTrigger("Hit");
        }
    }

    /* delegate 메소드 */
    public void EaselHMove(float inputMoveValueInputManager)
    {
        ani.SetBool("isAttackKeyDown", isAttackKeyDown);
        ani.SetFloat("moveSpeed", Mathf.Abs(phsics.playerHVelocity));
    }

    public void EaselHRun(float inputMoveValueInputManager)
    {
        ani.SetBool("isAttackKeyDown", isAttackKeyDown);
        ani.SetFloat("moveSpeed", Mathf.Abs(phsics.playerHVelocity));
    }

    public void EaselVMove(float inputMoveValueInputManager)
    {

    }

    //점프
    public void EaselJump(bool isDownCharacterJumpKeyInputManager)
    {
        if(!phsics.isGrounded)
        {
            return;
        }
        
        ani.SetBool("isAttackKeyDown", isAttackKeyDown);
        ani.SetTrigger("Jump");
    }

    //점멸
    public void EaselDash(bool isDownCharacterBlinkKeyInputMananger)
    {
        //추가바람
    }

    //공격!
    public void EaselNomal_Attack(bool isDownCharacterNomalAttackKeyManager)
    {
        if(phsics.playerController.isCheckLastAttack) //공격 대기
        {
            ani.ResetTrigger("Attack1");
            ani.ResetTrigger("Attack2");
            ani.ResetTrigger("Attack3");
            ani.SetBool("isAttackFinish", true);
            return;
        }

        //isAttackKeyDown = isDownCharacterNomalAttackKeyManager;
        //attack1,2,3 인덱스 달아놓기
        //만료시간은 계속 체크 stopcoroutine, startcoroutine이용
        //attack2,3 이어서 계속
        //attack3 이후에는 잠시 딜레이

        isAttackKeyDown = isDownCharacterNomalAttackKeyManager;
        ani.SetBool("isAttackKeyDown", isAttackKeyDown);

        isAttackFinish = false;
        ani.SetBool("isAttackFinish", isAttackFinish);

        StopCoroutine(attackAniExpiration);
        attackAniExpiration = AttackAniExpiration();
        StartCoroutine(attackAniExpiration);

        if (phsics.playerController.isAttack1)
        {
            ani.SetTrigger("Attack1");
        }
        else if(phsics.playerController.isAttack2)
        {
            ani.SetTrigger("Attack2");
        }
        else if(phsics.playerController.isAttack3)
        {
            ani.SetTrigger("Attack3");
        }
    }

    //특수 공격
    public void EaselSpecial_Attack(bool isDownCharacterSpecialAttackKeyManager)
    {
        //추가바람
        isSpecialAttackKeyDown = isDownCharacterSpecialAttackKeyManager;

        if(!phsics.isGrounded)
        {
            ani.ResetTrigger("SpecialAttack");
            return;
        }

        ani.SetBool("isSpecialAttackKeyDown", isSpecialAttackKeyDown);
        ani.SetTrigger("SpecialAttack");
        ani.ResetTrigger("Attack1");
        ani.ResetTrigger("Attack2");
        ani.ResetTrigger("Attack3");
        ani.SetBool("isAttackFinish", true);
        ani.SetBool("isAttackKeyDown", false);

        StartCoroutine(AttackAniExpiration());
    }

    public IEnumerator AttackAniExpiration() //공격 멈추고 약진 애니메이션 수정
    {
        yield return new WaitForSeconds(0.3f);
        ani.ResetTrigger("Attack1");
        ani.ResetTrigger("Attack2");
        ani.ResetTrigger("Attack3");
        isAttackKeyDown = false;
        isAttackFinish = true;
        ani.SetBool("isAttackFinish", isAttackFinish);
        ani.SetBool("isAttackKeyDown", isAttackKeyDown);
        ani.SetBool("isSpecialAttackKeyDown", false);
    }
}
