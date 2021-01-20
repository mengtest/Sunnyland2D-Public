using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;//仅限在父子关系中使用
    protected AudioSource deathAudio;

    protected virtual void Start()//虚拟临时变量
    {
        anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    public void JumpOn()//被踩上
    {
        anim.SetTrigger("death");
        deathAudio.Play();
    }

    public void Death()//被踩上后被消灭
    {
        GetComponent<Collider2D>().enabled = false;//敌人被消灭时禁用碰撞器
        Destroy(gameObject);
    }
}
