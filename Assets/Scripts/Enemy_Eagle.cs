using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public Transform top, bottom;

    public float speed;
    private float topY, bottomY;//得到上下点
    private bool isUp = true;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        //得到上下两个值并销毁
        topY = top.position.y;
        bottomY = bottom.position.y;
        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);//x轴不动，y轴向上速度
            if (transform.position.y > topY)//超过top点不再上升
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);//x轴不动，y轴下降速度

            if (transform.position.y < bottomY)//超过bottom点开始上升
            {
                isUp = true;
            }
        }
    }
}
