using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;
    //private Animator anim;

    public LayerMask ground;//判断地面图层
    public Transform leftPoint, rightPoint;//左右点的位置

    public float speed, jumpForce;
    private float leftx, rightx;
    private bool faceleft = true;//判断是否面向左

    protected override void Start()//重写父级start
    {
        base.Start();//继承父级start
        //获取相关组件
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        //anim = GetComponent<Animator>();

        transform.DetachChildren();//与子项目分离

        //获得左右侧的x值再销毁
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void Update()
    {
        //Movement();
        SwitchAnim();
    }

    void Movement()//敌人移动
    {
        if (faceleft)//面向左侧
        {
            if (coll.IsTouchingLayers(ground))//接触地面时才跳跃
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);//向左移动
            }
            if (transform.position.x < leftx)//超过左侧点掉头
            {
                transform.localScale = new Vector3(-1, 1, 1);//朝向右侧
                faceleft = false;
            }
        }
        else //面向右侧
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
            if (transform.position.x > rightx)//超过右侧点掉头
            {
                transform.localScale = new Vector3(1, 1, 1);//朝向左侧
                faceleft = true;
            }
        }
    }

    void SwitchAnim()//切换动画
    {
        if (anim.GetBool("jumping"))//下落
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }

        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))//返回
        {
            anim.SetBool("falling", false);
        }
    }

}
