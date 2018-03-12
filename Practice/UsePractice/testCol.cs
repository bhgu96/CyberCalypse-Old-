using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCol : MonoBehaviour
{
    private bool isGrounded;
    private float groundCheckerRadius;

    private PlayerController playerController;

    private int hitLayer;

    private float HitDir;

    /* public 인스펙터에서 조정 가능한 것*/

    public Transform groundChecker;
    public LayerMask whatIsGround;

    public float gravityVelocity;

    public GameObject playerObject; //자기 자신을 오브젝트로 넘겨주기 위함

    public AudioClip nomalAttackSound;
    public AudioClip specialAttackSound;

    public MainCameraController cameraObject; //카메라 쉐이크를 위한 변수. 삭제할것이다. 수정

    private HeroAttribute hero;

    void Awake()
    {
        hero = GetComponent<HeroAttribute>();
        groundCheckerRadius = 0.3f;
        playerController = new PlayerController(hero);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, whatIsGround);
        playerController.Gravity(isGrounded, isGrounded, gravityVelocity, playerObject);
    }

    private void OnTriggerEnter2D(Collider2D col)//HitDelegate Box
    {
        hitLayer = col.gameObject.layer;
        HitDir = col.offset.x;

        if (int.Equals(hitLayer,10) || int.Equals(hitLayer,11) || int.Equals(hitLayer, 12))
        {
            SoundManager.instance.RandomizeSfx(nomalAttackSound);
            cameraObject.CameraShake();
        }
        else if(int.Equals(hitLayer,13))
        {
            SoundManager.instance.RandomizeSfx(specialAttackSound);
            cameraObject.CameraShake();
        }

        playerController.Hit(playerObject, hitLayer, HitDir, col);
    }
}

      