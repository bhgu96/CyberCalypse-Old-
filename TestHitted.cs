using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHitted : MonoBehaviour
{
    public bool isGrounded;
    public float groundCheckerRadius;
    public Transform groundChecker;
    public LayerMask whatIsGround;
    public float gravity;
    public float gravityVelocity;

    /*public float moveForce;
    public float playerMass;
    public float playerGravity;*/

    //위 부분은 지울 것

    public float testNomalAttackHittedFirstVelocity;
    public float testNomalAttackHittedVelocity;
    public float testNomalAttackUpTime;
    public float testNomalAttackTime;
    public bool isNowNomalAttack;

    public float testSpecialAttackHittedFirstVelocity;
    public float testSpecialAttackHittedVelocity;
    public float testSpecialAttackUpTime;
    public float testSpecialAttackTime;
    public bool isNowSpecialAttack;

    public float dir;

    public GameObject nomalAttack1;
    public GameObject nomalAttack2;
    public GameObject nomalAttack3;
    public GameObject specialAttack;

    public float gravityCorrection;
    public bool isInvincibilityStatement;

    public AudioClip nomalAttackSound;
    public AudioClip specialAttackSound;

    public bool isLayDown;

    public void Start()
    {
        groundCheckerRadius = 0.3f;

        testNomalAttackHittedFirstVelocity = 2f;
        testNomalAttackUpTime = testNomalAttackHittedFirstVelocity * Mathf.Sin(Mathf.PI * 30.0f * Mathf.Deg2Rad) * (0.102f + Mathf.Epsilon); //최고점 오르는 시간

        testSpecialAttackHittedFirstVelocity = 5f;
        testSpecialAttackUpTime = testSpecialAttackHittedFirstVelocity * Mathf.Sin(Mathf.PI * 30.0f * Mathf.Deg2Rad) * (0.102f + Mathf.Epsilon); //최고점 오르는 시간

        /*moveForce = 0.01f;
        playerMass = 1.0f;
        playerGravity = 1.0f;*/
    }

    private void Update() //이 부분 지울것
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, whatIsGround);
        //중력작용
        if (!isGrounded)
        {
            StartCoroutine(GetGravity());
        }
        else
        {
            gravity = 0.0f;
            gravityCorrection = 0.0f;
        }
    }

    //이하 부분만 남기기
    private void OnTriggerEnter2D(Collider2D col)
    {
        try
        {
            if (isInvincibilityStatement)
            {
                return;
            }

            else
            {
                if (col.gameObject == nomalAttack1 || col.gameObject == nomalAttack2 || col.gameObject == nomalAttack3)
                {
                    SoundManager.instance.RandomizeSfx(nomalAttackSound);
                    isNowNomalAttack = true;
                    dir = col.offset.x;
                    StartCoroutine(NomalAttackHitted(dir, col));
                    return;
                }

                else if (col.gameObject == specialAttack)
                {
                    SoundManager.instance.RandomizeSfx(specialAttackSound);
                    isNowSpecialAttack = true;
                    dir = col.offset.x;
                    StartCoroutine(SpecialAttackHited(dir));
                    return;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator NomalAttackHitted(float dir, Collider2D col)
    {
        if (isGrounded)
        {
            if (dir > 0.0f) // 수정
            {
                if (col.gameObject == nomalAttack3)
                {
                    isLayDown = true;

                    while (testNomalAttackTime < testNomalAttackUpTime) // 수정
                    {
                        yield return null;

                        if (isNowSpecialAttack) //nomal -> Special로 갈때 이동중이던 모든 NomalAttack에 관한것을 정지
                        {
                            isNowNomalAttack = false;
                            testNomalAttackTime = 0.0f;
                            testNomalAttackHittedVelocity = 0.0f;
                            yield break;
                        }

                        gravity = 0.0f;
                        testNomalAttackTime += Time.deltaTime;
                        testNomalAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testNomalAttackTime, 2)) + (testNomalAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testNomalAttackTime);
                        transform.Translate(new Vector2(+testNomalAttackHittedVelocity, 0.25f));
                    }
                }
                else // 수정
                {
                    while (testNomalAttackTime < testNomalAttackUpTime) // 수정
                    {
                        yield return null;

                        if (isNowSpecialAttack) //nomal -> Special로 갈때 이동중이던 모든 NomalAttack에 관한것을 정지
                        {
                            isNowNomalAttack = false;
                            testNomalAttackTime = 0.0f;
                            testNomalAttackHittedVelocity = 0.0f;
                            yield break;
                        }

                        testNomalAttackTime += Time.deltaTime;
                        testNomalAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testNomalAttackTime, 2)) + (testNomalAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testNomalAttackTime);
                        transform.Translate(new Vector2(+testNomalAttackHittedVelocity, 0));
                    }
                }

            }
            else if (dir < 0.0f) // 수정
            {
                if (col.gameObject == nomalAttack3)
                {
                    isLayDown = true;

                    while (testNomalAttackTime < testNomalAttackUpTime) //수정
                    {
                        yield return null;

                        if (isNowSpecialAttack)
                        {
                            isNowNomalAttack = false;
                            testNomalAttackTime = 0.0f;
                            testNomalAttackHittedVelocity = 0.0f;
                            yield break;
                        }

                        gravity = 0.0f;
                        testNomalAttackTime += Time.deltaTime;
                        testNomalAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testNomalAttackTime, 2)) + (testNomalAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testNomalAttackTime);
                        transform.Translate(new Vector2(-testNomalAttackHittedVelocity, 0.25f));
                    }
                }
                else
                {
                    while (testNomalAttackTime < testNomalAttackUpTime) //수정
                    {
                        yield return null;

                        if (isNowSpecialAttack)
                        {
                            isNowNomalAttack = false;
                            testNomalAttackTime = 0.0f;
                            testNomalAttackHittedVelocity = 0.0f;
                            yield break;
                        }

                        testNomalAttackTime += Time.deltaTime;
                        testNomalAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testNomalAttackTime, 2)) + (testNomalAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testNomalAttackTime);
                        transform.Translate(new Vector2(-testNomalAttackHittedVelocity, 0));
                    }
                }
            }
        }

        else if (!isGrounded) // 수정
        {
            if (dir > 0.0f)
            {
                if (col.gameObject == nomalAttack3)
                {
                    isLayDown = true;

                    while (testNomalAttackTime < testNomalAttackUpTime)//수정
                    {
                        yield return null;

                        if (isNowSpecialAttack)
                        {
                            isNowNomalAttack = false;

                            testNomalAttackTime = 0.0f;
                            testNomalAttackHittedVelocity = 0.0f;
                            yield break;
                        }

                        gravity = 0.0f;
                        gravityCorrection = 0.0f;
                        testNomalAttackTime += Time.deltaTime;
                        testNomalAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testNomalAttackTime, 2)) + (testNomalAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testNomalAttackTime);
                        transform.Translate(new Vector2(+testNomalAttackHittedVelocity, 0.25f));
                    }
                }
                else
                {
                    while (testNomalAttackTime < testNomalAttackUpTime)//수정
                    {
                        yield return null;

                        if (isNowSpecialAttack)
                        {
                            isNowNomalAttack = false;
                            testNomalAttackTime = 0.0f;
                            testNomalAttackHittedVelocity = 0.0f;
                            yield break;
                        }

                        gravity = 0.0f;
                        gravityCorrection = Mathf.Epsilon * 1.6f; //평캔 중력 보정
                        testNomalAttackTime += Time.deltaTime;
                        testNomalAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testNomalAttackTime, 2)) + (testNomalAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testNomalAttackTime);
                        transform.Translate(new Vector2(+testNomalAttackHittedVelocity, 0.02f));
                    }
                }
            }
            else if (dir < 0.0f) // 수정
            {
                if (col.gameObject == nomalAttack3)
                {
                    isLayDown = true;

                    while (testNomalAttackTime < testNomalAttackUpTime)//수정
                    {
                        yield return null;

                        if (isNowSpecialAttack)
                        {
                            isNowNomalAttack = false;
                            testNomalAttackTime = 0.0f;
                            testNomalAttackHittedVelocity = 0.0f;
                            yield break;
                        }

                        gravity = 0.0f;
                        gravityCorrection = 0.0f;
                        testNomalAttackTime += Time.deltaTime;
                        testNomalAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testNomalAttackTime, 2)) + (testNomalAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testNomalAttackTime);
                        transform.Translate(new Vector2(-testNomalAttackHittedVelocity, 0.25f));
                    }
                }
                else
                {
                    while (testNomalAttackTime < testNomalAttackUpTime)//수정
                    {
                        yield return null;

                        if (isNowSpecialAttack)
                        {
                            isNowNomalAttack = false;
                            testNomalAttackTime = 0.0f;
                            testNomalAttackHittedVelocity = 0.0f;
                            yield break;
                        }

                        gravity = 0.0f;
                        gravityCorrection = Mathf.Epsilon * 1.6f; //평캔 중력 보정
                        testNomalAttackTime += Time.deltaTime;
                        testNomalAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testNomalAttackTime, 2)) + (testNomalAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testNomalAttackTime);
                        transform.Translate(new Vector2(-testNomalAttackHittedVelocity, 0.02f));
                    }
                }
            }
        }

        if (testNomalAttackTime > testNomalAttackUpTime)
        {
            StartCoroutine(NomalLayDown());
            isNowNomalAttack = false;
            testNomalAttackTime = 0.0f;
            testNomalAttackHittedVelocity = 0.0f;
            yield break;
        }
    }

    IEnumerator SpecialAttackHited(float dir)
    {
        if (!isGrounded)
        {
            if (dir > 0.0f)
            {
                while (testSpecialAttackTime < testSpecialAttackUpTime)//수정
                {
                    yield return null;

                    if (isNowNomalAttack)
                    {
                        isNowSpecialAttack = false;
                        testSpecialAttackTime = 0.0f;
                        testSpecialAttackHittedVelocity = 0.0f;
                        yield break;
                    }

                    testSpecialAttackTime += Time.deltaTime;
                    testSpecialAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testSpecialAttackTime, 2)) + (testSpecialAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testSpecialAttackTime);
                    transform.Translate(new Vector2(+0.03f, testSpecialAttackHittedVelocity));
                }
            }
            else if (dir < 0.0f)
            {
                while (testSpecialAttackTime < testSpecialAttackUpTime)//수정
                {
                    yield return null;

                    if (isNowNomalAttack)
                    {
                        isNowSpecialAttack = false;
                        testSpecialAttackTime = 0.0f;
                        testSpecialAttackHittedVelocity = 0.0f;
                        yield break;
                    }

                    testSpecialAttackTime += Time.deltaTime;
                    testSpecialAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testSpecialAttackTime, 2)) + (testSpecialAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testSpecialAttackTime);
                    transform.Translate(new Vector2(-0.03f, testSpecialAttackHittedVelocity));
                }
            }
        }

        if (isGrounded) // 수정
        {
            if (dir > 0.0f)
            {
                while (testSpecialAttackTime < testSpecialAttackUpTime)//수정
                {
                    yield return null;

                    if (isNowNomalAttack)
                    {
                        isNowSpecialAttack = false;
                        testSpecialAttackTime = 0.0f;
                        testSpecialAttackHittedVelocity = 0.0f;
                        yield break;
                    }

                    testSpecialAttackTime += Time.deltaTime;
                    testSpecialAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testSpecialAttackTime, 2)) + (testSpecialAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testSpecialAttackTime);
                    transform.Translate(new Vector2(+0.03f, testSpecialAttackHittedVelocity));
                }
            }
            else if (dir < 0.0f)
            {
                while (testSpecialAttackTime < testSpecialAttackUpTime)//수정
                {
                    yield return null;

                    if (isNowNomalAttack)
                    {
                        isNowSpecialAttack = false;
                        testSpecialAttackTime = 0.0f;
                        testSpecialAttackHittedVelocity = 0.0f;
                        yield break;
                    }

                    testSpecialAttackTime += Time.deltaTime;
                    testSpecialAttackHittedVelocity = ((-gravityVelocity) * 0.5f * Mathf.Pow(testSpecialAttackTime, 2)) + (testSpecialAttackHittedFirstVelocity * Mathf.Sin(30.0f * Mathf.Deg2Rad) * testSpecialAttackTime);
                    transform.Translate(new Vector2(-0.03f, testSpecialAttackHittedVelocity));
                }
            }
        }

        if (testSpecialAttackTime > testSpecialAttackUpTime)
        {
            isLayDown = true;
            StartCoroutine(GravityCorrction());
            isNowSpecialAttack = false;
            testSpecialAttackTime = 0.0f;
            testSpecialAttackHittedVelocity = 0.0f;
            yield break;
        }
    }

    IEnumerator GetGravity()
    {
        gravity += 0.25f * gravityVelocity * 0.5f * Time.deltaTime;

        if (gravity > 0.5f)
        {
            gravity = 0.5f;
        }

        else if (isNowNomalAttack || isNowSpecialAttack) //추가 부분
        {
            gravity = 0.0f;
        }

        this.transform.Translate(new Vector2(0, -gravity));
        yield return null;
    }

    IEnumerator GravityCorrction()
    {
        if (testSpecialAttackTime > testSpecialAttackUpTime) // 수정
        {
            while (!isGrounded)
            {
                yield return null;
                gravityCorrection += (0.25f * gravityVelocity * 0.5f * Time.deltaTime) * 0.04f;
                this.transform.Translate(new Vector2(0, -gravityCorrection));
            }
        }

        if (isGrounded) // 수정 필요
        {
            isInvincibilityStatement = true;
            isLayDown = true;
            StartCoroutine(LayDown()); //수정 필요
            yield break;
        }
    }

    IEnumerator NomalLayDown()
    {
        if(!isGrounded) // 수정
        {
            while(!isGrounded)
            {
                yield return null;
            }

            if(isGrounded) // 수정
            {
                isInvincibilityStatement = true;
                StartCoroutine(NomalLayDownTime());
            }
        }
    }

    IEnumerator NomalLayDownTime()
    {
        yield return new WaitForSeconds(0.8f);
        isInvincibilityStatement = false;
        isLayDown = false;
        yield break;
    }

    IEnumerator LayDown() // 수정필요
    {
        yield return new WaitForSeconds(0.8f);
        isInvincibilityStatement = false;
        isLayDown = false;
        yield break;
    }
}
    


