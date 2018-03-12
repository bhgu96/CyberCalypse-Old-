using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingleTonManager<InputManager>
{
    /* 필드 */
#region private
    bool isDownMenuUniqueKey;
    bool isDownCharacterJumpKey;
    bool isDownCharacterDashKey;
    bool isDownCharacterNomal_AttackKey;
    bool isDownCharacterSpecial_AttackKey;
    bool isDownTestSpellKey;
    int runCount;
    float inputHMoveValue;
    float inputVMoveValue;

   

    bool isDownShootingKey;

    bool isMenuActive; // Command 메소드 OR 클래스를 통해 구별

    // Switch 를 위한 변수 필요
    // AI와 Player를 구별하기 위한 메소드 필요 ( 보류 )
    // 반환 값을 통해 Switch 로 구분할 값 할당
    // 

    private  PlayerCommand playerCommand;
    #endregion

#region delegate
    /* 델리게이트 */
    //입력 받는 객체에 따라 달라지는 메소드
    //고유 기능이 있는 입력 델리게이트(선택,공격,점프,대쉬 등등)
    //이동 기능이 있는 입력 델리게이트(메뉴 이동, 캐릭터 이동 등등)
    public delegate void UniqueInput(bool isDownUniqueKey);
    public delegate void MoveInput(float inputHMoveValue);
    public delegate bool Command(bool isCheckInput);
    #endregion

#region public
    public event UniqueInput Jump;
    public event UniqueInput Dash;
    public event UniqueInput Nomal_Attack;
    public event UniqueInput Special_Attack;
    public event UniqueInput TestSpell;

    public event UniqueInput Shooting;

    public event MoveInput Menumove;
    public event MoveInput PlayerHMove;
    public event MoveInput HRun;
    public event MoveInput PlayerVMove;
    public event Command JumpCommand;
    public event Command DashCommand;
    public event Command NomalAttackCommand;
    public event Command SpecialAttackCommand;
    public event Command SpellCommand;

    public event Command ShootingCommand;
    #endregion
    /* 메소드 */
#region private Method
    private new void Awake()
    {
        base.Awake();
        playerCommand = new PlayerCommand(); //InputManager가 먼저 싱글톤 인스턴스를 생성한 다음 PlayerCommand로 넘어가기 때문에 Awake에 있어도 된다.
    }


    //캐릭터 이동과 메뉴 이동을 위한 업데이트
    void Update()
    {
        /* 플레이어 */
        inputHMoveValue = playerCommand.HMove();

        runCount = playerCommand.HRun();

        inputVMoveValue = playerCommand.VMove();
        isDownCharacterJumpKey = JumpCommand(isDownCharacterJumpKey);

        //공격키
        isDownCharacterNomal_AttackKey = NomalAttackCommand(isDownCharacterNomal_AttackKey);
        isDownCharacterSpecial_AttackKey = SpecialAttackCommand(isDownCharacterSpecial_AttackKey);

        /* 총 발사 공격 */
        isDownShootingKey = ShootingCommand(isDownShootingKey);

        isDownTestSpellKey = SpellCommand(isDownTestSpellKey);
        isDownCharacterDashKey = DashCommand(isDownCharacterDashKey);

        /* 메뉴 */
        isMenuActive = playerCommand.IsMenuKeyInput();

        try
        {
            switch (isMenuActive)
            {
                case true:
                    //Menu Acting
                    break;

                case false:
                    //Menu unActing
                    if(runCount < 2)
                    {
                        PlayerHMove(inputHMoveValue);
                    }
                    else if(runCount >= 2)
                    {
                        HRun(inputHMoveValue);
                    }
                    PlayerVMove(inputVMoveValue); //수직이동
                    
                    if(isDownCharacterJumpKey)
                    {
                        Jump(isDownCharacterJumpKey);
                    }
                    else if(isDownCharacterDashKey)
                    {
                        Dash(isDownCharacterDashKey);
                    }
                    else if(isDownTestSpellKey)
                    {
                        TestSpell(isDownTestSpellKey);
                    }
                    /*else if(isDownCharacterNomal_AttackKey)
                    {
                        Nomal_Attack(isDownCharacterNomal_AttackKey);
                    }*/
                    else if(isDownCharacterSpecial_AttackKey)
                    {
                        Special_Attack(isDownCharacterSpecial_AttackKey);
                    }

                    else if(isDownShootingKey) /* 총 발사 */
                    {
                        Shooting(isDownShootingKey);
                    }
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
}
#endregion

#region Abstract
abstract class AbsCommand
{
    //값을 반환시켜 InputManager에 보낸다.
    public abstract bool IsMenuKeyInput();
    public abstract float HMove();

    public abstract int HRun(); // 몇초안에 2번 누르면 달리기 실행

    public abstract float VMove();
    public abstract bool isJumpInput(bool isDownJumpKey);
    public abstract bool isDashInput(bool isDownDashKey);
    public abstract bool isSpellInput(bool isDownSpellKey);
    public abstract bool isNomalAttackInput(bool isDownNomalAttackKey);
    public abstract bool isSpecialAttackInput(bool isDownSpecialAttackKey);

    public abstract bool isShootingInput(bool isDownShootingKey);
}
#endregion

class PlayerCommand : AbsCommand
{
#region private
    private bool isMenuActing;
    private float HmoveValue;
    private int rightRunCount;
    private int leftRunCount;
    private float VmoveValue;
    private bool isJumpActing;
    private bool isDashActing;
    private bool isSpellActing;
    private bool isNomalAttackActing;
    private bool isSpecialAttackActing;

    private bool isShootingActing;

    private IEnumerator stopLeftCount;
    private IEnumerator stopRightCount;
#endregion

    public PlayerCommand()
    {
        InputManager.instance.JumpCommand += isJumpInput;
        InputManager.instance.DashCommand += isDashInput;
        InputManager.instance.NomalAttackCommand += isNomalAttackInput;
        InputManager.instance.SpecialAttackCommand += isSpecialAttackInput;
        InputManager.instance.SpellCommand += isSpellInput;

        InputManager.instance.ShootingCommand += isShootingInput;

        stopLeftCount = LeftRunCount();
        stopRightCount = RightRunCount();
    }
#region override Method
    public override bool IsMenuKeyInput()
    {
        if(Input.GetKeyDown(KeyCode.P) && !isMenuActing)
        {
            isMenuActing = true;
            return isMenuActing;
        }
        else if (Input.GetKeyDown(KeyCode.P) && isMenuActing)
        {
            isMenuActing = false;
            return isMenuActing;
        }
        else
        {
            return isMenuActing;
        }
    }

    public override float HMove()
    {
        HmoveValue = Input.GetAxisRaw("Horizontal Move");
        return HmoveValue;
    }

    public override int HRun() //매니저에서 코루틴을 쓰지 않는 방법
    {
        //키를 다르게 눌렸을때 가속을 못하게 한다.(한쪽 키만 빠르게 따닥 눌렸을때만 뛰기)
        bool isDownLeftKey = Input.GetKeyDown(KeyCode.A);
        bool isDownRightKey = Input.GetKeyDown(KeyCode.D);
        bool isDownRightKeyUp = Input.GetKeyUp(KeyCode.D);
        bool isDownLeftKeyUp = Input.GetKeyUp(KeyCode.A);
        bool isRightKey = Input.GetKey(KeyCode.D);
        bool isLeftKey = Input.GetKey(KeyCode.A);

        if(isDownRightKey)
        {
            rightRunCount++;
            return rightRunCount;
        }
        else if(isDownLeftKey)
        {
            leftRunCount++;
            return leftRunCount;
        }

        if(isDownRightKeyUp)
        {
            CoroutineHandler.instance.StartCoroutine(stopRightCount);
            return rightRunCount;
        }
        else if(isDownLeftKeyUp)
        {
            CoroutineHandler.instance.StartCoroutine(stopLeftCount);
            return leftRunCount;
        }

        if(isLeftKey)
        {
            CoroutineHandler.instance.StopCoroutine(stopLeftCount);
            stopLeftCount = LeftRunCount();
            return leftRunCount;
        }
        else if(isRightKey)
        {
            CoroutineHandler.instance.StopCoroutine(stopRightCount);
            stopRightCount = RightRunCount();
            return rightRunCount;
        }
        else
        {        
            return 0;
        }
    }

    IEnumerator LeftRunCount()
    {
        yield return new WaitForSeconds(0.2f);
        leftRunCount = 0;
    }

    IEnumerator RightRunCount()
    {
        yield return new WaitForSeconds(0.2f);
        rightRunCount = 0;
    }


    public override float VMove()
    {
        VmoveValue = Input.GetAxisRaw("Vertical Move");
        return VmoveValue;
    }

    public override bool isJumpInput(bool isDownJumpKey)
    {
        isDownJumpKey = Input.GetButtonDown("Jump");
        isJumpActing = isDownJumpKey;
        return isJumpActing;
    }

    public override bool isDashInput(bool isDownDashKey)
    {
        isDownDashKey = Input.GetButtonDown("Dash");
        isDashActing = isDownDashKey;
        return isDashActing;
    }

    public override bool isSpellInput(bool isDownSpellKey)
    {
        isDownSpellKey = Input.GetButtonDown("Private Skill1");
        isSpellActing = isDownSpellKey;
        return isSpellActing;
    }

    public override bool isShootingInput(bool isDownShootingKey)
    {
        isDownShootingKey = Input.GetButtonDown("Nomal Attack");
        isShootingActing = isDownShootingKey;
        return isShootingActing;
    }

    public override bool isNomalAttackInput(bool isDownNomalAttackKey)
    {
        isDownNomalAttackKey = Input.GetButtonDown("Nomal Attack");//Input.GetKeyDown(KeyCode.Z);
        isNomalAttackActing = isDownNomalAttackKey;
        return isNomalAttackActing;
    }

    public override bool isSpecialAttackInput(bool isDownSpecialAttackKey)
    {
        isDownSpecialAttackKey = Input.GetButtonDown("Special Attack"); //Input.GetKeyDown(KeyCode.X);
        isSpecialAttackActing = isDownSpecialAttackKey;
        return isSpecialAttackActing;
    }
#endregion
}
