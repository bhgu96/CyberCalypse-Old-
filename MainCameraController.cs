using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
	public GameObject target;

    /* CamerShake */
    public float shakeCircleX;
    public float shakeCircleY;
    public float shakeRadius;

    public float insideX;

    public float shakeDistance;
    /* CamerShake */

    public LayerMask whatIsWall;
    public Transform leftWallChecker;
    public Transform rightWallChecker;
    public Transform roofChecker;
    public float wallDistance;

    /* private */
    public float horizontalAddition;
    public float vertexAddition;
    private float smoother;

    private bool isLeftWalled;
    private bool isRightWalled;
    private bool isRoofed;
    private float wallCheckerRadius;

    private Vector3 targetPosition;
    private Vector3 cameraPosition;

    private void Awake()
    {
        wallCheckerRadius = 0.4f;
        wallDistance = Mathf.Abs(leftWallChecker.position.x + 3.3f);

        shakeRadius = Mathf.Sqrt(Mathf.Pow(shakeCircleX, 2) + Mathf.Pow(shakeCircleY, 2)); //Shake Distance Circle
        isLeftWalled = Physics2D.OverlapCircle(leftWallChecker.position, wallCheckerRadius, whatIsWall); //leftWallCheck
        isRightWalled = Physics2D.OverlapCircle(rightWallChecker.position, wallCheckerRadius, whatIsWall);//rightWallCheck
        isRoofed = Physics2D.OverlapCircle(roofChecker.position, wallCheckerRadius, whatIsWall);

        vertexAddition = 1.8f;
        smoother = 30.0f;

        //거리 벌리는건 wallChecker들의 거리에 따라서 달라진다 (+13, -13)
        //천장 체크도 구현 바람(y축 체크)
        if (isLeftWalled)
        {
            transform.position = new Vector3(target.transform.position.x + wallDistance, target.transform.position.y + vertexAddition, this.transform.position.z);
        }
        else if (isRightWalled)
        {
            transform.position = new Vector3(target.transform.position.x - wallDistance, target.transform.position.y + vertexAddition, this.transform.position.z);
        }
        else
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y + vertexAddition, this.transform.position.z); //타겟으로 바로 가지않고 벽에 충돌하고 있으면 벽에 닿는 곳까지만 보이게 한다.
        }
    }

    private void Start()
	{
        if (insideX > shakeRadius)
        {
            throw new System.ArgumentException("Get off shakeRadius");
        }
    }

    private void Update()
    {
        isLeftWalled = Physics2D.OverlapCircle(leftWallChecker.position, wallCheckerRadius, whatIsWall);
        isRightWalled = Physics2D.OverlapCircle(rightWallChecker.position, wallCheckerRadius, whatIsWall);
        isRoofed = Physics2D.OverlapCircle(roofChecker.position, wallCheckerRadius, whatIsWall);
    }

    private void LateUpdate()
	{
        //만약 발이 빠졌을때는 카메라가 움직이지 못하게 한다. 발이 나올때까지(y축)
        targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        cameraPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //오른쪽,왼쪽으로 잠깐 움직이는 동안은 카메라의 x축이 고정되어야한다.
        //한 3,4걸음 걸을동안은 카메라의 x축은 움직이면 안된다.
        //y축은 움직이어야한다.

        if (isRoofed && (isLeftWalled || isRightWalled))
        {
            return;
        }
        else if(isRoofed && (!isLeftWalled || !isRightWalled)) //중첩 if문 수정
        {
            if (targetPosition.x - cameraPosition.x > horizontalAddition + Mathf.Epsilon)
            {
                this.transform.position = new Vector3(targetPosition.x - horizontalAddition, cameraPosition.y, this.transform.position.z);
            }
            else if (targetPosition.x - cameraPosition.x < -(horizontalAddition + Mathf.Epsilon))
            {
                this.transform.position = new Vector3(targetPosition.x + horizontalAddition, cameraPosition.y, this.transform.position.z);
            }
            return;
        }

        if (isLeftWalled || isRightWalled)
        {
            // 벽에 부딪칠시 벽에서 일정 거리 떨어진채로 카메라가 위치해야한다.
            transform.position = new Vector3(cameraPosition.x, targetPosition.y + vertexAddition, this.transform.position.z);
            return;
        }


        if (targetPosition.x - cameraPosition.x > horizontalAddition + Mathf.Epsilon) // +horizontalAddition 넘어갔으므로 카메라 움직인다.
        {
            targetPosition = new Vector3(targetPosition.x - horizontalAddition, targetPosition.y + vertexAddition, this.transform.position.z);
        }
        else if(targetPosition.x - cameraPosition.x < -(horizontalAddition + Mathf.Epsilon)) //-addtion 보다 작으므로 카메라 움직인다.
        {
            targetPosition = new Vector3(targetPosition.x + horizontalAddition, targetPosition.y + vertexAddition, this.transform.position.z);
        }
        else//그 외에는 y축만 움직이게 한다.
        {
            transform.position = new Vector3(cameraPosition.x, targetPosition.y + vertexAddition, this.transform.position.z);
            return;
        }

        transform.position = Vector3.Lerp(cameraPosition, targetPosition, smoother * Time.deltaTime);
    }

    public void CameraShake() //Test용, ArcanePhsics에서 Collide 될때마다 호출
    {
        StartCoroutine(CameraInsideXShake());
    }

    public IEnumerator CameraInsideXShake()
    {
        //Camera Shake Loop
        for(float i = 0; i < 0.3f; i+= Time.deltaTime) // 임시 루프
        {
            shakeDistance = Random.Range(-insideX, +insideX); //프레임마다 랜덤 처리, 루프 안에서 처리
            transform.position = new Vector3(this.transform.position.x + shakeDistance, this.transform.position.y, this.transform.position.z);
            yield return null;
        }
    }
}