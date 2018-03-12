using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testColliderDirection : MonoBehaviour
{
    BoxCollider2D box2d;
    Transform thisLocalScale;

    private void Start()
    {
        box2d = this.GetComponent<BoxCollider2D>();
        thisLocalScale = this.transform.GetComponent<Transform>();
    }
    private void Update()
    {
        if(transform.parent.localScale.x > 0.0f)
        {
            box2d.offset = new Vector2(+2.2f, 2f);
            thisLocalScale.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        else if(transform.parent.localScale.x < 0.0f)
        {
            box2d.offset = new Vector2(-2.2f, 2f);
            thisLocalScale.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }
}
