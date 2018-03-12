using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanePhsics : MonoBehaviour
{
    /* private */

#region inputCheck
    private bool isDownCharacterJumpKey;
    private bool isDownCharacterNomal_AttackKey;
    private bool isDownCharacterSpecial_AttackKey;
    private bool isDownCharacterDashKey;

    private float inputHMoveValue;
    private float inputVMoveValue;

    private bool isDownShootingKey;
    #endregion
#region CollideCheck
    public bool isGrounded;
    private float groundCheckerRadius;

    public bool isBoundaryWalled;
    private float boundaryWallCheckerRadius;

    public bool isWalled;
    private float wallCheckerRadius;

    public bool isRoofed;
    private float roofCheckerRadius;
    #endregion
#region Move
    /* Move */
    private float moveForce;
    private float moveAcceleration;
    private float verticalAcceleration;

    public float playerHVelocity;
    public float playerVVelocity;
    #endregion
#region Jump
    /* Jump */
    private float jumpForce;
    #endregion
#region Old Attack
    /* Attack*/
    private float nomal_Attack_Speed;

    public PlayerController playerController;

    private int hitLayer;

    private float HitDir;

    private HeroAttribute hero;
    #endregion
    /* public 인스펙터에서 조정 가능한 것*/

#region public
    public Transform groundChecker;
    public LayerMask whatIsGround;

    public Transform BoundaryWallChecker;
    public LayerMask whatisBoundaryWall;

    public Transform wallChecker;
    public LayerMask whatisWall;

    public Transform roofChecker;
    public LayerMask whatisRoof;

    public float gravityVelocity;
    public float gravity;

    public float playerMass;
    public float playerGravity;
    public float max_moveSpeed;

    /*public float blinkDistance;

    public float delayBlink;*/

    public float delaySpecialAttackTime;

    public GameObject attack1Hit;
    public GameObject attack2Hit;
    public GameObject attack3Hit;
    public GameObject specialAttackHit;
    public GameObject leftBoundaryWall;
    public GameObject rightBoundaryWall;

    //public GameObject prefab; //나중에 지울수도 있음, Skill prefab이기 때문에 인스펙터에서 등록 시키지 않을 예정,

    public GameObject playerObject; //자기 자신을 오브젝트로 넘겨주기 위함

    public MainCameraController cameraObject;
    #endregion

    #region YDJ Field
    /* 이곳부터 필드 작성 윤동준*/

    //필드 작성 부분

    /* 여기까지 */
#endregion

    #region private Unity Method
    /* 메소드 */
    void Awake()
    {
        hero = GetComponent<HeroAttribute>();

        playerController = new PlayerController(hero); // 인스턴스 생성자에 넘겨줄꺼 이곳에 차곡차곡 매개변수 할당하기
    }

    private void Start()
    {
        groundCheckerRadius = 0.05f;
        boundaryWallCheckerRadius = 0.3f;
        wallCheckerRadius = 0.3f;
        roofCheckerRadius = 0.3f;

        this.moveForce = hero.moveForce;
        this.jumpForce = hero.jumpForce;
        this.moveAcceleration = hero.horizontalAcceleration; //대쉬
        this.verticalAcceleration = hero.verticalAcceleration;


        InputManager.instance.PlayerHMove += CharacterHMove;
        InputManager.instance.HRun += CharacterRun;
        InputManager.instance.PlayerVMove += CharacterVMove;
        InputManager.instance.Jump += CharacterJump;
        InputManager.instance.Dash += CharacterDash;
        InputManager.instance.Nomal_Attack += CharacterNomal_Attack;
        InputManager.instance.Special_Attack += CharacterSpecial_Attack;
        InputManager.instance.Shooting += CharacterShoot;       
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, whatIsGround);
        isBoundaryWalled = Physics2D.OverlapCircle(BoundaryWallChecker.position, boundaryWallCheckerRadius, whatisBoundaryWall);//맵 바운더리 체크
        isWalled = Physics2D.OverlapCircle(wallChecker.position, wallCheckerRadius, whatisWall);
        isRoofed = Physics2D.OverlapCircle(roofChecker.position, roofCheckerRadius, whatisRoof);

        gravity = playerController.Gravity(isGrounded, isBoundaryWalled, gravityVelocity, playerObject);

        /* 바운더리 벽 체크해서 텔포해도 안넘어가게 했다.*/
        if(this.transform.position.x < leftBoundaryWall.transform.position.x && this.transform.localScale.x < 0.0f)
        {
            this.transform.position = new Vector3(leftBoundaryWall.transform.position.x + 1.6f, this.transform.position.y, this.transform.position.z);
        }
        else if(this.transform.position.x > rightBoundaryWall.transform.position.x && this.transform.localScale.x > 0.0f)
        {
            this.transform.position = new Vector3(rightBoundaryWall.transform.position.x - 1.6f, this.transform.position.y, this.transform.position.z);
        }
        else
        {
            //blinkDistance = 5.0f; --> 대쉬 관련으로
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        hitLayer = collision.gameObject.layer;

        if (int.Equals(hitLayer, 8) && inputVMoveValue < 0.0f)
        {
            float distance = 2.133f * 0.5f * 0.5f * 0.35f * 0.5773f; //0.5773 = Mathf.sqrt(3.0f) 역수
            this.transform.Translate(new Vector2(0, distance));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)//HitDelegate Box, Hit 사운드도 넘겨주기 수정
    {
        hitLayer = col.gameObject.layer;
        HitDir = col.offset.x;

        if(int.Equals(hitLayer, 15)) //아에 바운더리와 부딪치면 텔포를 못하게함
        {
            //blinkDistance = 0.0f; --> 대쉬 관련으로
        }

        playerController.Hit(playerObject, hitLayer,HitDir, col); //collider만 넘겨서 parent붙여서 보내버리자

        if(int.Equals(hitLayer,10) || int.Equals(hitLayer, 11) || int.Equals(hitLayer, 12) || int.Equals(hitLayer, 13)) //PlayerController에서 처리할지는 나중에 수정
        {
            cameraObject.CameraShake(); // 수정사항
        }
    }
    #endregion

#region delegate Method
    /* delegate 메소드 */
    public void CharacterHMove(float inputMoveValueInputManager)
    {
        inputHMoveValue = inputMoveValueInputManager;

        playerHVelocity = playerController.MoveList(inputHMoveValue, moveForce, isGrounded,isBoundaryWalled, isDownCharacterSpecial_AttackKey,playerObject);
    }

    public void CharacterRun(float inputMoveValueInputManager)
    {
        inputHMoveValue = inputMoveValueInputManager;

        playerHVelocity = playerController.Hrun(inputHMoveValue, moveForce,isGrounded, isBoundaryWalled, playerObject);
    }

    public void CharacterVMove(float inputMoveValueInputManager)
    {
        inputVMoveValue = inputMoveValueInputManager;
        
        if(!isGrounded)
        {
            playerVVelocity = playerController.VerticalMove(inputVMoveValue, verticalAcceleration, isGrounded, isWalled, isRoofed, playerObject);
        }
    }
    
    //점프
    public void CharacterJump(bool isDownCharacterJumpKeyInputManager)
    {
       isDownCharacterJumpKey = isDownCharacterJumpKeyInputManager;

       if(isGrounded && !isDownCharacterNomal_AttackKey && !isDownCharacterSpecial_AttackKey) //조건 나중에 PlayerController에서 처리
       {
            playerController.Jump(jumpForce, gravityVelocity, playerObject);
       }
    }
    #endregion
#region YDJ Delegate Method
    /* 윤동준 구현 틀 -> 이 틀은 수정하지 말고 PlayerController 부분의 해당 이벤트 메소드를 구현 및 수정하면 됨*/

    //대쉬 윤동준
    public void CharacterDash(bool isDownCharacterDashKeyInputMananger)
    {
        isDownCharacterDashKey = isDownCharacterDashKeyInputMananger; //대쉬 키를 눌렀다는 것에 대한 true || false를 전달

        if(playerController.isHung)
        {
            return;
        }

        /*(코드설명) 윤동준 이곳이 대쉬키 true || false를 받고 PlayerController 클래스의 Dash 함수로 넘어감 */
        playerController.Dash();
    }

    //총 발사 공격 윤동준
    public void CharacterShoot(bool isDownCharacterShootingKeyManager)
    {
        /*(코드설명) 이곳에서 총 발사한 키의 false || true를 받고 PlayerController 클래스의 Shoot으로 넘긴다 윤동준 */

        isDownShootingKey = isDownCharacterShootingKeyManager; //총 발사 키를 눌렀다는 것에 대한 true || false를 전달

        playerController.Shoot();
    }
    /* 이곳까지 윤동준 구현 틀*/
#endregion



#region Old Attack Script
    /* Old Attack Script */

    //공격!
    public void CharacterNomal_Attack(bool isDownCharacterNomalAttackKeyManager)
    {
        isDownCharacterNomal_AttackKey = isDownCharacterNomalAttackKeyManager;

        playerController.NomalAttack(isGrounded, isDownCharacterNomal_AttackKey, nomal_Attack_Speed ,attack1Hit, attack2Hit, attack3Hit);

        /* 입력 받은 키 초기화*/
        isDownCharacterNomal_AttackKey = false;
    }

    //특수 공격
    public void CharacterSpecial_Attack(bool isDownCharacterSpecialAttackKeyManager)
    {
        isDownCharacterSpecial_AttackKey = isDownCharacterSpecialAttackKeyManager;

        playerController.SpecialAttack(isGrounded, isDownCharacterSpecial_AttackKey, delaySpecialAttackTime, specialAttackHit, attack1Hit, attack2Hit, attack3Hit);

        /* 입력 받은 키 초기화*/
        isDownCharacterSpecial_AttackKey = false;
    }

    /*public void CharacterSpell(bool isDownCharacterSpellKeyManager)
    {
        playerController.Spell(playerObject,prefab);
    }*/
#endregion
}

#region Abstract Controller
public abstract class AbsController
{
    abstract public float DefaultMove(float moveValue, float moveForce, bool isBoundaryWalled, GameObject targetObject);
    abstract public float AttackMove(float moveValue, float moveForce, bool isBoundaryWalled, GameObject targetObject);
    abstract public float Hrun(float moveValue, float moveForce,bool isGrounded, bool isBoundaryWalled, GameObject targetObject);
    abstract public float VerticalMove(float moveValue, float moveForce, bool isGrounded, bool isWalled, bool isRoofed, GameObject targetObject);
    abstract public float Gravity(bool isGrounded, bool isWalled, float gravityVelocity, GameObject targetObject);
    abstract public void Jump(float jumpForce, float gravityVelocity, GameObject playerObject);
    abstract public void Dash(/* 매개변수 넣을거 이곳에 미리 넣어둘 것 윤동준*/); //윤동준 추상메소드 대쉬 -> PlayerController에서 구현
    abstract public void NomalAttack(bool isGrounded, bool isNomalAttackKeyDown, float NomalAttackSpeed, GameObject nomalAttackCollider1, GameObject nomalAttackCollider2, GameObject nomalAttackCollider3);
    abstract public void SpecialAttack(bool isGrounded, bool isAttackKeyDown, float delaySpeicalAttackTime, GameObject specialAttakCollider, GameObject nomalAttackCollider1, GameObject nomalAttackCollider2, GameObject nomalAttackCollider3);
    /*abstract public void Spell(GameObject targetObject, GameObject spellPrefabs);*/
    abstract public void Hit(GameObject targetObject, int hitLayer, float dir, Collider2D hitCollide);

    abstract public void Shoot(/* 매개변수 넣을거 이곳에 미리 넣어둘 것 윤동준*/); //윤동준 추상메소드 Shoot -> PlayerController에서 구현
}
#endregion

[System.Serializable]

public class PlayerController : AbsController
{
    #region private
    private float playerMass;
    private float playerGravity;
    private float maxVelocity;

    private float gravity;
    private float gravityVelocity;

    private float playerSpeed;

    private float HmoveVelocity;
    private float VmoveVelocity;

    private float jumpTime;
    private float JumpVelocity;

    private float blinkDistance;
    private bool isDelayBlink;

    private bool isCheckAttackTime;

    private byte attackIndex;
    private BitArray attackKind;

    private IEnumerator attackExpireCoroutine;

    private float nomalAttackTime;
    private bool isNomalAttackNow;
    private bool isNomalAttacking;
    private bool isSpecialAttacking;

    private bool isDelaySpecialAttack;

    private float sinThirty;

    private float nomalVelocity;
    private float nomalTime;
    private float nomalForce;
    private float nomalVertex;

    private float specialVelocity;
    private float specialTime;
    private float specialForce;
    private float specialVertex;

    private int plusDir;
    private int minusDir;

    private bool isSpecialAttacked;
    private float gravityCorrection;
    private float correctionValue;

    private HeroAttribute hero;

    private Vector3 spellPosition;
    private Vector3 spellScale;
    #endregion
    #region delegate
    public delegate float Moving(float moveValue, float moveForce, bool isBoundaryWalled, GameObject targetObject);// Move 델리게이트
    public delegate void NomalAttacking(GameObject colliderAttack); // Attack 델리게이트
    public delegate IEnumerator HitDelegate(float velocity, float force, float vertex, float dir, GameObject targetObject); // hitDelegate 델리게이트
    #endregion
    #region event
    public event Moving defaultMoving; //기본 공격에 관한 움직임들
    public event Moving attackMoving; // 모든 기술 및 공격들에 관한 움직임들

    public event NomalAttacking nomalAttack1; //기본공격 1타
    public event NomalAttacking nomalAttack2; //기본공격 2타
    public event NomalAttacking nomalAttack3; //기본공격 3타

    public event HitDelegate nomalHit1;
    public event HitDelegate nomalHit2;
    public event HitDelegate nomalHit3;
    public event HitDelegate specialHit;
    #endregion
    #region public
    public bool isCheckLastAttack; //애니메이션에서 마지막 3타 공격이 마치는 것을 체크하기 위해

    public bool isInvincibilityStatement; //수정
    public bool isAttack1;
    public bool isAttack2;
    public bool isAttack3;

    public bool isHung; //수정 삭제

    public float verticalMoveValue;
    public bool isJumpPack;
    public bool isJumpPackDown;
    public bool isRunning;
    #endregion

    #region YDJ Field
    /* 윤동준 필드 */

    // *이곳에 쓸 필드를 정의*

    /* 여기 영역 까지*/
    #endregion


    public PlayerController(HeroAttribute hero) //인스턴스 넘겨줄거 생성자에 저장
    {
        playerMass = 1.0f;
        playerGravity = 1.0f;
        maxVelocity = 0.15f;

        gravityVelocity = 9.8f;

        sinThirty = Mathf.Sin(30.0f * Mathf.Deg2Rad);

        nomalForce = 2f;
        nomalVertex = nomalForce * sinThirty * (0.102f + Mathf.Epsilon);

        specialForce = 7f;
        specialVertex = specialForce * sinThirty * (0.102f + Mathf.Epsilon);

        plusDir = +1;
        minusDir = -1;

        defaultMoving += DefaultMove;
        attackMoving += AttackMove;

        nomalAttack1 += NomalAttacking1;
        nomalAttack2 += NomalAttacking2;
        nomalAttack3 += NomalAttacking3;

        nomalHit1 += NomalAttack1Knock;
        nomalHit2 += NomalAttack2Knock;
        nomalHit3 += NomalAttack3Knock;
        specialHit += SpecialAttackKnock;

        attackKind = new BitArray(4, false);

        attackExpireCoroutine = AttackExpiration();

        this.hero = hero;
        correctionValue = 0.4f;

        #region YDJ Constructer
        /* 이곳 밑에서부터 주석 영역까지 생성자 초기화할 부분 윤동준 */

        //   *이곳에서 초기화*

        /*  */
        #endregion
    }

    #region MoveList
    /* public */
    public float MoveList(float moveValue, float moveForce, bool isGrounded, bool isBoundaryWalled, bool isSpecialAttack, GameObject targetObject)
    {
        if (isGrounded && isNomalAttacking || isGrounded && isSpecialAttack) // 공격 및 기술 시전시 조건 추가
        {
            playerSpeed = attackMoving(moveValue, moveForce, isBoundaryWalled, targetObject);
        }
        else
        {
            playerSpeed = defaultMoving(moveValue, moveForce, isBoundaryWalled, targetObject);
        }

        return playerSpeed;
    }
    #endregion

    #region MoveCondition
    /*private*/
    private LiveMoveStatement MoveCondition(float moveValue)
    {
        if (moveValue > 0.0f)
        {
            return LiveMoveStatement.right;
        }
        else if (moveValue < 0.0f)
        {
            return LiveMoveStatement.left;
        }
        else
        {
            return LiveMoveStatement.wait;
        }
    }
    #endregion

    #region VerticalMoveCondition
    private LiveMoveStatement VerticalMoveCondition(float moveValue)
    {
        if (moveValue > 0.0f)
        {
            return LiveMoveStatement.up;
        }
        else if (moveValue < 0.0f)
        {
            return LiveMoveStatement.down;
        }
        else
        {
            return LiveMoveStatement.wait;
        }
    }
    #endregion

    #region Old AttackCondition
    private AttackStatement AttackCondition()
    {
        if (!attackKind[3] & !attackKind[2] & !attackKind[1] & attackKind[0])
        {
            return AttackStatement.attack1;
        }
        else if (!attackKind[3] & !attackKind[2] & attackKind[1] & attackKind[0] & isNomalAttackNow)
        {
            return AttackStatement.attack2;
        }
        else if (!attackKind[3] & attackKind[2] & attackKind[1] & attackKind[0] & isNomalAttackNow)
        {
            return AttackStatement.attack3;
        }
        else if (attackKind[3] & attackKind[2] & attackKind[1] & attackKind[0] & isNomalAttackNow)
        {
            return AttackStatement.wait;
        }
        else // attack에서 텀을 가질동안을 이쪽으로 넘어오게
        {
            return AttackStatement.nothing;
        }
    }
    #endregion

    #region Old Attack
    private void CheckNomalAttack() // attack statement check
    {
        for (int i = 0; i <= attackIndex; i++)
        {
            attackKind[i] = true;
        }
    }

    private void InitCheckNomalAttack()
    {
        isNomalAttackNow = false;
        isCheckLastAttack = false;

        isAttack1 = false;
        isAttack2 = false;
        isAttack3 = false;

        for (int i = 0; i <= attackIndex; i++)
        {
            attackKind[i] = false;
        }

        attackIndex = 0;
    }

    private void NomalAttacking1(GameObject colliderAttack1)
    {
        colliderAttack1.SetActive(true);
        CoroutineHandler.instance.StartCoroutine(DelayAttack(colliderAttack1));
        attackIndex++;
    }

    private void NomalAttacking2(GameObject colliderAttack2)
    {
        colliderAttack2.SetActive(true);
        CoroutineHandler.instance.StartCoroutine(DelayAttack(colliderAttack2));
        attackIndex++;
    }
    private void NomalAttacking3(GameObject colliderAttack3)
    {
        colliderAttack3.SetActive(true);
        CoroutineHandler.instance.StartCoroutine(DelayAttack(colliderAttack3));
        attackIndex++;
    }
    #endregion


    /*override*/
    #region override Method
    public override float Gravity(bool isGrounded, bool isWalled, float gravityVelocity, GameObject targetObject) //캐싱필요, 수정 필요
    {
        if (isJumpPackDown)
        {
            gravity += 0.25f * gravityVelocity * 0.5f * (Time.deltaTime * 2f);
            gravity = Mathf.Clamp(gravity, 0.0f, 1f);
        }
        else
        {
            gravity += 0.25f * gravityVelocity * 0.5f * (Time.deltaTime * 0.8f);
            gravityCorrection += (0.25f * gravityVelocity * 0.5f * Time.deltaTime) * correctionValue;
            gravity = Mathf.Clamp(gravity, 0.0f, 0.5f);
        }

        if (isGrounded || isJumpPack)
        {
            gravity = 0.0f;
            gravityCorrection = 0.0f;
            correctionValue = 0.4f;
            isSpecialAttacked = false;
        }

        if (isSpecialAttacked)
        {
            correctionValue += 0.003f;

            targetObject.transform.Translate(new Vector2(0, -gravityCorrection));
            return gravityCorrection;
        }

        targetObject.transform.Translate(new Vector2(0, -gravity));
        return gravity;
    }

    public override float DefaultMove(float moveValue, float moveForce, bool isBoundaryWalled, GameObject targetObject)
    {
        switch (MoveCondition(moveValue)) //up down 추가 예정
        {
            case LiveMoveStatement.right:
                HmoveVelocity += +moveForce * (0.25f * (playerMass + playerGravity));
                targetObject.transform.localScale = new Vector3(+1.0f, 1.0f, 1.0f);
                break;
            case LiveMoveStatement.left:
                HmoveVelocity += -moveForce * (0.25f * (playerMass + playerGravity));
                targetObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                break;
            case LiveMoveStatement.wait:
                HmoveVelocity = 0.0f;
                break;
        }

        HmoveVelocity = Mathf.Clamp(HmoveVelocity, -0.15f, 0.15f);

        if (isBoundaryWalled)
        {
            return HmoveVelocity;
        }

        targetObject.transform.Translate(new Vector2(HmoveVelocity, 0));
        return HmoveVelocity;
    }

    public override float Hrun(float moveValue, float moveForce,bool isGrounded, bool isBoundaryWalled, GameObject targetObject)
    {
        //점프팩 사용중에는 달리기를 못하게 한다.
        //달리게 된다면 DefaultMove로 넘어가게 한다.

        if (isBoundaryWalled)
        {
            return HmoveVelocity;
        }

        if (!isGrounded && !isRunning) //어떻게 할까, 달리다가 점프하면 계속 가속되서 날라가고, 달리다가 떨어지면 그 속도를 유지하며 떨어진다.
        {
            HmoveVelocity = DefaultMove(moveValue, moveForce, isBoundaryWalled, targetObject);
            return HmoveVelocity;
        }

        moveForce = 0.65f;
        isRunning = true;

        if(moveValue > 0.0f)
        {
            HmoveVelocity = +moveForce * (0.25f * (playerMass + playerGravity));
            targetObject.transform.localScale = new Vector3(+1.0f, 1.0f, 1.0f);
        }
        else if(moveValue < 0.0f)
        {
            HmoveVelocity = -moveForce * (0.25f * (playerMass + playerGravity));
            targetObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            isRunning = false;
            HmoveVelocity = 0.0f;
        }

        targetObject.transform.Translate(new Vector2(HmoveVelocity, 0));
        return HmoveVelocity;
    }

    public override float AttackMove(float moveValue, float moveForce, bool isBoundaryWalled, GameObject targetObject)
    {
        if (isCheckLastAttack)
        {
            return 0;
        }

        switch (MoveCondition(moveValue))
        {
            case LiveMoveStatement.right:
                HmoveVelocity = +moveForce * (0.25f * (playerMass + playerGravity));
                targetObject.transform.localScale = new Vector3(+1.0f, 1.0f, 1.0f);
                break;
            case LiveMoveStatement.left:
                HmoveVelocity = -moveForce * (0.25f * (playerMass + playerGravity));
                targetObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                break;
            case LiveMoveStatement.wait:
                if (targetObject.transform.localScale.x > 0.0f)
                {
                    HmoveVelocity = +moveForce * (0.1f * (playerMass + playerGravity));
                    targetObject.transform.localScale = new Vector3(+1.0f, 1.0f, 1.0f);
                }
                else if (targetObject.transform.localScale.x < 0.0f)
                {
                    HmoveVelocity = -moveForce * (0.1f * (playerMass + playerGravity));
                    targetObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                break;
        }

        if (isBoundaryWalled)
        {
            return HmoveVelocity;
        }

        targetObject.transform.Translate(new Vector2(HmoveVelocity, 0));
        return HmoveVelocity;
    }

    public override float VerticalMove(float moveValue, float moveForce, bool isGrounded, bool isWalled, bool isRoofed, GameObject targetObject)
    {
        verticalMoveValue = moveValue;

        if (CEnergyStatement.energy <= 0.5f) //임시
        {
            isJumpPack = false;
            isJumpPackDown = false;
            return 0;
        }

        if (verticalMoveValue == 1)
        {
            isJumpPack = true;
        }
        else if(verticalMoveValue == -1)
        {
            isJumpPackDown = true;
        }
        else if(verticalMoveValue == 0)
        {
            isJumpPack = false;
            isJumpPackDown = false;
        }

        switch (VerticalMoveCondition(moveValue)) //up down 추가 예정
        {
            case LiveMoveStatement.up:
                VmoveVelocity += +moveForce * (0.25f * (playerMass + playerGravity));
                break;
            case LiveMoveStatement.down:
                if (isGrounded) //땅에 닿았다.
                {
                    isJumpPackDown = false;
                    VmoveVelocity = 0.0f;
                    gravity = 0.0f;
                    return 0;
                }
                VmoveVelocity = 0.0f;
                break;
            case LiveMoveStatement.wait:
                VmoveVelocity = 0.0f;
                break;
        }

        VmoveVelocity = Mathf.Clamp(VmoveVelocity, 0, 0.3f);

        if(isRoofed)
        {
            return VmoveVelocity;
        }

        targetObject.transform.Translate(new Vector2(0, VmoveVelocity));
        return VmoveVelocity;
    }

    public override void Jump(float jumpForce, float gravityVelocity, GameObject playerObject)
    {
        if(!isNomalAttacking && !isSpecialAttacking)
        {
            CoroutineHandler.instance.StartCoroutine(ActionJump(jumpForce, gravityVelocity, playerObject));//캐싱필요?
        }

    }
    #endregion

#region YDJ Dash Method
    /*윤동준 Dash */
    public override void Dash()
    {
        Debug.Log("Dash");
        /* 윤동준 대쉬구현 */

        

    }
    #endregion


    #region YDJ Shoot Method
    /*윤동준 Shoot*/
    public override void Shoot()
    {
        Debug.Log("Shoot");
        /* 윤동준 Shoot 구현 */

    }
    #endregion

    #region YDJ Custom Method
    /*윤동준 이곳 영역부터 추가로 작성할 커스텀 메소드 작성*/

    // *이곳에*

    /*여기 영역 까지*/
    #endregion


#region Attack Combo Old Method
    /* Nomal Attack Old Method */
    public override void NomalAttack(bool isGrounded, bool isNomalAttackKeyDown, float nomalAttackSpeed, GameObject nomalAttackCollider1, GameObject nomalAttackCollider2, GameObject nomalAttackCollider3)
    {
        if (!isGrounded)
        {
            return;
        }

        nomalAttackTime = nomalAttackSpeed;
        isNomalAttacking = isNomalAttackKeyDown;

        CheckNomalAttack();

        CoroutineHandler.instance.StopCoroutine(attackExpireCoroutine); // 공격키 누를때마다 딜레이 시간 초기화
        attackExpireCoroutine = AttackExpiration();
        CoroutineHandler.instance.StartCoroutine(attackExpireCoroutine);

        try
        {
            switch (AttackCondition()) //애니메이션 처리에 있어서 공격 할때 enum으로 캐릭터를 따로 분리하여 각 캐릭터에 맞는 애니메이션을 작동시킨다.
            {
                case AttackStatement.attack1:
                    isNomalAttackNow = true;
                    isAttack1 = true;
                    TestEaseliaAni.instance.EaselNomal_Attack(isNomalAttackKeyDown); //Singleton 말고 딴거 쓸 것 수정
                    nomalAttack1(nomalAttackCollider1);                   
                    break;
                case AttackStatement.attack2:
                    isAttack1 = false;
                    isAttack2 = true;
                    TestEaseliaAni.instance.EaselNomal_Attack(isNomalAttackKeyDown); //Singleton 말고 딴거 쓸 것 수정
                    CoroutineHandler.instance.StartCoroutine(CutomInvoke(nomalAttack2, nomalAttackCollider2));
                    break;
                case AttackStatement.attack3:
                    isAttack2 = false;
                    isAttack3 = true;
                    TestEaseliaAni.instance.EaselNomal_Attack(isNomalAttackKeyDown); //Singleton 말고 딴거 쓸 것 수정
                    CoroutineHandler.instance.StartCoroutine(CutomInvoke(nomalAttack3, nomalAttackCollider3));
                    break;
                case AttackStatement.wait:
                    isAttack3 = false;

                    if (isCheckLastAttack)
                    {                      
                        break;
                    }
                    else
                    {
                        CoroutineHandler.instance.StartCoroutine(StopAttackDelay());
                        TestEaseliaAni.instance.EaselNomal_Attack(isNomalAttackKeyDown); //Singleton 말고 딴거 쓸 것 수정
                    }
                    break;
                case AttackStatement.nothing: //오류 처리
                    Debug.Log("Nothing Instruction");
                    break;
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }

        CoroutineHandler.instance.StartCoroutine(AttackMoveWait());
    }

    public override void SpecialAttack(bool isGrounded, bool isAttackKeyDown, float delayTime, GameObject specialAttakCollider, GameObject nomalAttackCollider1, GameObject nomalAttackCollider2, GameObject nomalAttackCollider3)
    {

        if(isDelaySpecialAttack)
        {
            return;
        }

        isSpecialAttacking = isAttackKeyDown;

        nomalAttackCollider1.SetActive(false); //for문으로 받아올까?
        nomalAttackCollider2.SetActive(false);
        nomalAttackCollider3.SetActive(false);

        specialAttakCollider.SetActive(true);

        CoroutineHandler.instance.StartCoroutine(DelaySpecialAttack(delayTime));

        CoroutineHandler.instance.StartCoroutine(DelayAttack(specialAttakCollider));

        CoroutineHandler.instance.StartCoroutine(AttackMoveWait());
    }

    /*public override void Spell(GameObject targetObject, GameObject spellPrefabs)
    {
        if (targetObject.transform.localScale.x > 0)
        {
            spellPosition = new Vector3(+targetObject.transform.position.x + 1.0f, targetObject.transform.position.y, targetObject.transform.position.z);
            spellScale = targetObject.transform.localScale;
        }
        else if (targetObject.transform.localScale.x < 0)
        {
            spellPosition = new Vector3(targetObject.transform.position.x - 1.0f, targetObject.transform.position.y, targetObject.transform.position.z);
            spellScale = targetObject.transform.localScale;
        }

        //EffectScript에서 받은 조건에 따라 Effect 오브젝트들을 Active한다.(테스트로 FlameAttackEffect만 존재
        if (EffectScript.instance.isPropertyFlame) //InputManager로 받은 키에 대해서
        {
            ManagementableEffect.instance.AttackEffect(EffectScript.instance.isPropertyFlame, spellPrefabs, spellPosition, spellScale);
        }
        else if (EffectScript.instance.isPropertyIce)
        {
            ManagementableEffect.instance.AttackEffect(EffectScript.instance.isPropertyIce, spellPrefabs, spellPosition, spellScale);
        }
    }*/

    private void HitInit()
    {
        nomalVelocity = 0.0f;
        specialVelocity = 0.0f;

        nomalTime = 0.0f;
        specialTime = 0.0f;

       /* if(!isSpecialAttacked) // 중력보정으로인한 무적 수정
        {
            Debug.Log("asdfaf");
            CoroutineHandler.instance.StartCoroutine(Invincibilited()); // 중력보정 수정 완료시 발동
        }*/
        //CoroutineHandler.instance.StartCoroutine(Invincibilited()); // 중력보정 수정 완료시 발동
    }

    public IEnumerator Invincibilited() //중력 보정 후 땅에 떨어졌을때 발동
    {
        isInvincibilityStatement = true;

        yield return new WaitForSeconds(1.5f);

        isInvincibilityStatement = false;
    }

    public override void Hit(GameObject targetObject, int hitLayer, float dir, Collider2D hitCollide)
    {
        if(isInvincibilityStatement)
        {
            return;
        }

        int hittingDir = 0;

        if(dir > 0.0f)
        {
            hittingDir = plusDir;
        }
        else if(dir < 0.0f)
        {
            hittingDir = minusDir;
        }

        //hero.LoseHealth에서 상대의 공격력을 받아와서 깍아야 한다. LoseHealth의 매개변수에는 공격력 변수가 들어가야한다.

        switch(hitLayer)
        {
            case 10:
                //LiveAttribute의 LoseHealth가 들어가야한다.
                hero.LoseHealth(ColliderInstanceID.instance.CheckCollider(hitCollide));// 데미지를 주는 해당 객체를 매개변수로 받아와서 넘기자
                KnockDownCollect(nomalHit1, nomalVelocity, nomalForce, nomalVertex, hittingDir, targetObject);
                break;
            case 11:
                hero.LoseHealth(ColliderInstanceID.instance.CheckCollider(hitCollide));
                //LiveAttribute의 LoseHealth가 들어가야한다.
                KnockDownCollect(nomalHit2, nomalVelocity, nomalForce, nomalVertex, hittingDir, targetObject);
                break;
            case 12:
                hero.LoseHealth(ColliderInstanceID.instance.CheckCollider(hitCollide));
                //LiveAttribute의 LoseHealth가 들어가야한다.
                KnockDownCollect(nomalHit3, nomalVelocity, nomalForce, nomalVertex, hittingDir, targetObject);
                break;
            case 13:
                hero.LoseHealth(ColliderInstanceID.instance.CheckCollider(hitCollide));
                //LiveAttribute의 LoseHealth가 들어가야한다.
                KnockDownCollect(specialHit, specialVelocity, specialForce, specialVertex, hittingDir, targetObject);
                break;
        }
    }

    private void KnockDownCollect(HitDelegate hitDelegate, float velocity, float force, float vertex, float dir, GameObject targetObject)
    {
        CoroutineHandler.instance.StartCoroutine(hitDelegate(velocity, force, vertex, dir, targetObject));
    }

    public IEnumerator NomalAttack1Knock(float velocity, float force, float vertex, float dir, GameObject targetObject)
    {
        for(; nomalTime < vertex ;  nomalTime += Time.deltaTime)
        {
            yield return null;
            gravity = 0.0f;
            gravityCorrection = 0.0f;
            velocity = ((-9.8f) * 0.5f * Mathf.Pow(nomalTime, 2)) + (force * sinThirty * nomalTime) ;
            targetObject.transform.Translate(new Vector2(velocity * dir, 0));
        }

        HitInit();
    }
    public IEnumerator NomalAttack2Knock(float velocity, float force, float vertex, float dir, GameObject targetObject) //1과 똑같아서 지울 수 있음 수정
    {
        for ( ; nomalTime < vertex; nomalTime += Time.deltaTime)
        {
            yield return null;
            gravity = 0.0f;
            gravityCorrection = 0.0f;
            velocity = ((-9.8f) * 0.5f * Mathf.Pow(nomalTime, 2)) + (force * sinThirty * nomalTime);
            targetObject.transform.Translate(new Vector2(velocity * dir, 0));
        }

        HitInit();
    }
    public IEnumerator NomalAttack3Knock(float velocity, float force, float vertex, float dir, GameObject targetObject)
    {
        force = 5; //special Case
        vertex = force * sinThirty * (0.102f + Mathf.Epsilon); //special Case

        for (; nomalTime < vertex; nomalTime += Time.deltaTime)
        {
            yield return null;
            gravity = 0.0f;
            gravityCorrection = 0.0f;
            velocity = ((-9.8f) * 0.5f * Mathf.Pow(nomalTime, 2)) + (force * sinThirty * nomalTime);
            targetObject.transform.Translate(new Vector2(0.02f * dir, velocity));
        }

        HitInit();
    }
    public IEnumerator SpecialAttackKnock(float velocity, float force, float vertex, float dir, GameObject targetObject)
    {
        for ( ; specialTime < vertex ; specialTime += Time.deltaTime )
        {
            yield return null;
            gravity = 0.0f;
            isSpecialAttacked = true;
            velocity = ((-9.8f) * 0.5f * Mathf.Pow(specialTime, 2)) + (force * sinThirty * specialTime);
            targetObject.transform.Translate(new Vector2(0.03f * dir, velocity));
        }

        HitInit();
    }
    #endregion

#region Coroutine
    /* CoroutineHandler */
    public IEnumerator ActionJump(float jumpForce, float gravityVelocity, GameObject playerObject)
    {
        for(; JumpVelocity >- Mathf.Epsilon; jumpTime+= Time.deltaTime)
        {
            yield return null;
            gravity = 0.0f;
            JumpVelocity = (jumpTime * jumpTime * (-gravityVelocity) * Mathf.Cos(Mathf.PI * (0.3f + Mathf.Epsilon)) + (jumpTime * jumpForce));
            playerObject.transform.Translate(new Vector2(0, JumpVelocity));
        }

        if (JumpVelocity < -Mathf.Epsilon)
        {
            JumpVelocity = 0.0f;
            jumpTime = 0.0f;
        }
    }

    public IEnumerator DelaySpecialAttack(float delayTime)
    {
        isDelaySpecialAttack = true;
        yield return new WaitForSeconds(delayTime);
        isDelaySpecialAttack = false;
    }

    public IEnumerator AttackExpiration()
    {
        yield return new WaitForSeconds(0.5f);
        InitCheckNomalAttack();
    }

    public IEnumerator CutomInvoke(NomalAttacking attacking, GameObject collider)
    {
        if (isCheckAttackTime)
        {
            yield break;
        }

        isCheckAttackTime = true;

        yield return new WaitForSeconds(nomalAttackTime);
        isCheckAttackTime = false;
        attacking(collider);
    }

    public IEnumerator StopAttackDelay() //3타를 다 하고 그다음 일반공격 딜레이
    {
        isCheckLastAttack = true;
        TestEaseliaAni.instance.EaselNomal_Attack(isNomalAttacking); //Singleton 말고 딴거 쓸 것 수정
        yield return new WaitForSeconds(0.4f);
        InitCheckNomalAttack();
    }

    public IEnumerator DelayAttack(GameObject collider) //collider 바로 꺼지지 말고 좀 이따가 꺼지게 해서 공격이 프레임 내에 들어가게 설정
    {
        yield return new WaitForSeconds(0.1f);
        collider.SetActive(false);
    }

    public IEnumerator AttackMoveWait() //공격동안 AttackMove가 비활성화를 하기 위한 코루틴
    {
        yield return new WaitForSeconds(0.5f);
        isSpecialAttacking = false;
        isNomalAttacking = false;
    }
}
#endregion

#region enum
enum LiveMoveStatement
{
    right,
    left,
    wait,
    up,
    down
}

[Flags]
enum AttackStatement
{
    attack1,
    attack2,
    attack3,
    wait,
    nothing
}
#endregion


#region coroutine Handler
/* 코루틴 이용을 위한 Handler Class */
public class CoroutineHandler : MonoBehaviour //나중에 다른 오브젝트 및 스크립트로 빼놓기, 캐싱 고려
{
    private static CoroutineHandler _instance;
    public static CoroutineHandler instance
    {
        get
        {
            if(CoroutineHandler.Equals(_instance,null))//_instance == null)
            {
                _instance = new GameObject("CoroutineHandler").AddComponent<CoroutineHandler>();
            }

            return _instance;
        }
    }
}
#endregion





/*IEnumerator Dash() //혹시 모를 나중을 대비해서 Dash는 남겨둠
{ 
    if(!isGrounded) //점프 대쉬
    {
        while(!isGrounded)
        {
            yield return null;
            isDownCharacterJumpKey = false;// 점프 대쉬중이므로 이미 점프가 끝났기 때문에 점프키는 false
            if(this.transform.localScale.x > 0.0f)
            {
                playerHVelocity = +dashSpeed;
            }
            else if(this.transform.localScale.x < 0.0f)
            {
                 playerHVelocity = -dashSpeed;
            }

            if(Input.GetButtonDown("Dash"))
            {
                yield break;
            }
            ani.SetBool("isDashkeyDown",isDownCharacterDashKey);
            ani.SetBool("isJumpkeyDown", isDownCharacterJumpKey);
            transform.Translate(new Vector2(playerHVelocity, 0.0f));
        }

        if(isGrounded) //땅에 닿으면 그대로 멈춤
        {
            dashTime = 0.0f;
            playerHVelocity = 0.0f;
            isDownCharacterDashKey = false;
            ani.SetBool("isGrounded", isGrounded);
            ani.SetBool("isDashkeyDown",isDownCharacterDashKey);
            yield break;
        }
    }

    else if(isGrounded) //땅에 닿고 있을때의 대쉬(일반대쉬)
    {
        while(dashTime < 0.5f) //대쉬 시작(계속 달려나감)
        {
             yield return null;

            dashTime += Time.deltaTime;

            if(this.transform.localScale.x > 0.0f)
            {
                 playerHVelocity = +dashSpeed;
            }

            else if(this.transform.localScale.x < 0.0f)
            {
                 playerHVelocity = -dashSpeed;
            }

             if(Input.GetButtonDown("Dash")) //또 대쉬키를 눌러도 더 이동하지 못하게 방지
            {
                 yield break;
            }

            if(Input.GetButtonUp("Dash") && inputHMoveValue == 0.0f)// 대쉬 키 떼면 멈춤
            {
                playerHVelocity = 0.0f;
                dashTime = 0.0f;
                isDownCharacterDashKey = false;
                ani.SetBool("isDashkeyDown",isDownCharacterDashKey);
                yield break;
            }

            transform.Translate(new Vector2(playerHVelocity, 0.0f));

            ani.SetBool("isGrounded", isGrounded);
            ani.SetBool("isDashkeyDown",isDownCharacterDashKey);
        }
    }

        if(isDownCharacterJumpKey && !isGrounded) //점프키를 눌렀을때(대쉬 점프)
        {
            while(!isGrounded)
            {
                yield return null;
                isDownCharacterJumpKey = true; //대쉬 점프이므로 점프키가 true
                if(this.transform.localScale.x > 0.0f)
                {
                    playerHVelocity = +dashSpeed;
                }
                else if(this.transform.localScale.x < 0.0f)
                {
                    playerHVelocity = -dashSpeed;
                }

                if(Input.GetButtonDown("Dash"))
                {
                    yield break;
                }
                ani.SetBool("isJumpkeyDown", isDownCharacterJumpKey);
                transform.Translate(new Vector2(playerHVelocity, 0.0f));
            }
            //땅에 닿으면 그대로 멈춤
            if(isGrounded)
            {
                dashTime = 0.0f;
                playerHVelocity = 0.0f;
                isDownCharacterDashKey = false;
                isDownCharacterJumpKey = false;
                ani.SetBool("isGrounded", isGrounded);
                ani.SetBool("isJumpkeyDown", isDownCharacterJumpKey);
                ani.SetBool("isDashkeyDown",isDownCharacterDashKey);
                yield break;
            }
        }

            dashTime = 0.0f;
            playerHVelocity = dashSpeed;

            if(this.transform.localScale.x > 0.0f) //점프키 안누르고 일반 대쉬 정방향
            {
                while(playerHVelocity > -Mathf.Epsilon)
                {
                    yield return null;
                    dashTime += Time.deltaTime;

                    playerHVelocity = dashSpeed - dashTime*1.2f;

                    if(Input.GetButtonDown("Dash")) //또 대쉬키를 눌러도 더 이동하지 못하게 방지
                    {
                         yield break;
                    }

                    transform.Translate(new Vector2(playerHVelocity, 0.0f));

                    ani.SetBool("isGrounded", isGrounded);
                    ani.SetBool("isDashkeyDown",isDownCharacterDashKey);
                }
            }

            dashTime = 0.0f;
            playerHVelocity = -dashSpeed;

            if(this.transform.localScale.x < 0.0f) //점프키 안누르고 일반 대쉬 정방향
            {
                while(playerHVelocity < +Mathf.Epsilon)
                {
                    yield return null;
                    dashTime += Time.deltaTime;

                    playerHVelocity = -dashSpeed + dashTime*1.2f;

                    if(Input.GetButtonDown("Dash")) //또 대쉬키를 눌러도 더 이동하지 못하게 방지
                    {
                         yield break;
                    }

                    transform.Translate(new Vector2(playerHVelocity, 0.0f));

                    ani.SetBool("isGrounded", isGrounded);
                    ani.SetBool("isDashkeyDown",isDownCharacterDashKey);
                }
            }

        if(playerHVelocity < -Mathf.Epsilon)
        {
            isDownCharacterDashKey = false;
            playerHVelocity = 0.0f;
            dashTime = 0.0f;
        }

        else if(playerHVelocity > +Mathf.Epsilon)
        {
            isDownCharacterDashKey = false;
            playerHVelocity = 0.0f;
            dashTime = 0.0f;
        }

        ani.SetBool("isDashkeyDown",isDownCharacterDashKey);
}*/

