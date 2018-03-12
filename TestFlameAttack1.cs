using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFlameAttack1 : EffectObjectPool
{
    public GameObject target;
    public float distance_Speed;

    private void Awake()
    {
        target = GameObject.Find("Mage_TestPlayer");
    }
    private void Start()
    {
        
    }
    void Update ()
    {
        // 플레이어의 방향에 따라 움직이던 총알이 바뀌지 않게
        if (this.transform.localScale.x > 0)
        {
            distance_Speed = +0.5f;
        }
        else if (this.transform.localScale.x < 0)
        {
            distance_Speed = -0.5f;
        }
        StartCoroutine(FlameBall(distance_Speed));
	}

    public override void OnObjectReuse()
    {
        // 다시 초기화
        // 방향 초기화
        if(target.transform.localScale.x > 0)
        {
            transform.position = new Vector2(target.transform.position.x + 1.0f, target.transform.position.y);
        }
        else if(target.transform.localScale.x < 0)
        {
            transform.position = new Vector2(target.transform.position.x - 1.0f, target.transform.position.y);
        }
        transform.localScale = target.transform.localScale;
    }

    IEnumerator FlameBall(float distance_Speed)
    {
        
        if (distance_Speed > 0)
        {
             transform.Translate(new Vector2(distance_Speed, 0));
        }
        else if (distance_Speed < 0)
        {
             transform.Translate(new Vector2(distance_Speed, 0));
        }
        
        yield return null;
    }
}

