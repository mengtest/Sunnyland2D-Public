using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController player;

    public Rigidbody2D rb;
    public Animator anim;
    public Collider2D coll;
    public Collider2D disColl;

    public Transform cellingCheck, groundCheck;
    //public AudioSource jumpAudio,hurtAudio,cherryAudio;
    public Joystick joystick;
    public LayerMask ground;//设置地面图层

    public float speed, jumpForce;
    private float horizontalMoveJoy, horizontalMove;//全局变量
    private bool isHurt;//是否受伤，默认是false    

    [SerializeField]
    private int cherry, gem;
    //public TextMeshProUGUI CherryNum;
    public Text CherryNum;

    bool jumpPressed;
    int jumpCount;

    [Header("CD UI Component")]
    public Image cdImage;

    [Header("Dash Parameter")]
    public float dashTime;//dash时长
    private float dashTimeLeft;//冲锋剩余时间
    public float lastDash = -10f;//上一次dash时间点，一开始就可以执行
    public float dashCoolDown;//CD冷却时间
    public float dashSpeed;//冲锋速度

    public bool isGround, isJump, isDashing;

    private void Awake()
    {
        player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Returns the component of Type type if the GameObject has one attached, null if it doesn't
        rb = GetComponent<Rigidbody2D>();//获得组件 刚体
        coll = GetComponent<Collider2D>();//获得组件 碰撞体
        anim = GetComponent<Animator>();//获得组件 动画
    }

    //调用 FixedUpdate 的频度常常超过Update
    //如果帧率很低，可以每帧调用该函数多次
    //如果帧率很高，可能在帧之间完全不调用该函数
    //在 FixedUpdate 之后将立即进行所有物理计算和更新
    private void FixedUpdate()  //该函数会在一个固定帧率的帧时被调用
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);//每一帧检查若干次是否在地面

        Dash();
        if (isDashing)
        {
            return;//冲锋时不执行其他
        }

        if (!isHurt)
        {
            GroundMovement();
        }
        Jump();
        SwitchAnim();
    }

    // Update is called once per frame
    void Update()
    {
        //当一个按键被按下
        //在用户按下由 buttonName 标识的虚拟按钮的帧期间返回 true
        //接触地面时才跳跃
        if ( (joystick.Vertical > 0.5f && jumpCount > 0)
            || (Input.GetButtonDown("Jump") && jumpCount > 0))
        {
            jumpPressed = true;//按下跳跃按钮在FixedUpdate里禁用而在Update启用
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time >= (lastDash + dashCoolDown))//超过上次的时间和CD时间
            {
                //可以执行dash
                ReadyToDash();
            }
        }
        cdImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;//剩余时间冷却

        Crouch();
        CherryNum.text = cherry.ToString();//樱桃数改变
    }

    void GroundMovement()//地面移动
    {
        //获得角色移动的按键的input方式
        //获得横向移动输入
        //-1向左 0没动 1向右

        //返回由 axisName 标识的虚拟轴的值，未应用平滑过滤
        horizontalMoveJoy = joystick.Horizontal ;//摇杆
        horizontalMove = Input.GetAxisRaw("Horizontal");//键盘 只返回-1，0，1 获取整数

        if (horizontalMoveJoy != 0f)
        {
            rb.velocity = new Vector2(horizontalMoveJoy * speed, rb.velocity.y);
            //anim.SetFloat("running", Mathf.Abs(horizontalMoveJoy)); //取绝对值时设置奔跑动画
        }
        if (horizontalMoveJoy > 0f)
        {
            transform.localScale = new Vector3(1, 1, 1);//改变朝向
        }
        if (horizontalMoveJoy < 0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);//改变朝向
        }

        if (horizontalMove != 0f)
        {
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
            //anim.SetFloat("running", Mathf.Abs(horizontalMove)); //取绝对值时设置奔跑动画
            transform.localScale = new Vector3(horizontalMove, 1, 1);//改变朝向
        }
    }

    void SwitchAnim()//动画切换
    {
        anim.SetFloat("running",Mathf.Abs(rb.velocity.x));//取绝对值时设置奔跑动画
        //anim.SetBool("idle", false);

        //向上冲的力没有了，没有接触地面
        //if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        //{
        //    anim.SetBool("falling", true);
        //}
        if (isGround)
        {
            anim.SetBool("falling", false);
            //anim.SetBool("idle", true);
        }
      //  if (anim.GetBool("jumping"))
        //{ }
        if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)//跳跃时没有了跳跃力
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);//受伤后让跑步速度变为0
            if (Mathf.Abs(rb.velocity.x) < 0.1f)//受伤状态停止
            {
                anim.SetBool("hurt", false);
                isHurt = false;
            }
        } //检查该碰撞体是否正在接触指定 layerMask 上的任何碰撞体
          //else if (coll.IsTouchingLayers(ground))

    }

    void Jump() //跳跃
    {
        if (isGround)
        {
            jumpCount = 2;//可跳跃数量
            isJump = false;
        }
        if (jumpPressed && isGround)//在地面上跳
        {
            isJump = true;
            rb.velocity = Vector2.up * jumpForce;//y轴纵向有跳跃力
            jumpCount--;
            //jumpAudio.Play();
            SoundManager.instance.JumpAudio();
            jumpPressed = false;
            //anim.SetBool("jumping", true);
        }
        else if (jumpPressed && isJump && jumpCount > 0)//在空中跳
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpCount--;
            //jumpAudio.Play();
            SoundManager.instance.JumpAudio();
            jumpPressed = false;
           // anim.SetBool("jumping", true);
        }
    }

    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)//如果一个2D碰撞器进入触发器
    {
        //收集物品
        if (collision.tag == "Cherry")
        {
            //cherryAudio.Play();
            SoundManager.instance.CherryAudio();
            //Destroy(collision.gameObject);
            //cherry += 1;
            collision.GetComponent<Animator>().Play("isGot");//单独播放获得动画
            //CherryNum.text = cherry.ToString();
        }

        //收集物品
        if (collision.tag == "Gem")
        {
            //cherryAudio.Play();
            SoundManager.instance.CherryAudio();
            Destroy(collision.gameObject);
        }

        //死亡线
        if (collision.tag == "Deadline")
        {
            //GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 1f);//重新游戏
        }
    }

    public void CherryCount()//计算樱桃数
    {
        cherry += 1;
    }


    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();//调用敌人的属性

            if (anim.GetBool("falling"))
            {
                //Destroy(collision.gameObject);
                enemy.JumpOn();//跳上并销毁敌人
                rb.velocity = Vector2.up * jumpForce;//跳起销毁
                anim.SetBool("jumping", true);//消灭敌人时再次跳起
            }
            //受伤
            else if (transform.position.x < collision.gameObject.transform.position.x)//在敌人左边
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)//在敌人右边
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
        }
    }

    //下蹲趴下
    void Crouch()
    {
        if ((joystick.Vertical < -0.5f) || (Input.GetButton("Crouch")) )
        {
            anim.SetBool("crouching", true);
            disColl.enabled = false;
        }
        else if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))//检查碰撞器是否掉到定义区域圆形范围
        {   //不在此范围时才能站起，方形碰撞器恢复
            anim.SetBool("crouching", false);
            disColl.enabled = true;
        }
    }

    //重制当前场景
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //二段跳按钮
    public void NewJump()
    {
        if (jumpCount > 1)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpCount--;
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
        if (jumpCount == 0 && isGround)
        {
            rb.velocity = Vector2.up * jumpForce;
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
    }

    //准备冲锋
    public void ReadyToDash()
    {
        isDashing = true;//开始冲锋
        dashTimeLeft = dashTime;//可持续冲锋时间，开始计时
        lastDash = Time.time;//最后时间点是按下按键的时间
        cdImage.fillAmount = 1;
    }

    //冲锋
    public void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)//剩余时间大于0，继续冲锋
            {
                if (rb.velocity.y > 0 && !isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce);//在空中Dash向上
                }
                rb.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, rb.velocity.y);//地面Dash

                dashTimeLeft -= Time.deltaTime;//剩余时间
                ShadowPool.instance.GetFormPool();//拿个影子出来
            }

            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                if (!isGround)
                {
                    //目的：为了在空中结束Dash的时候可以接一个小跳跃。根据自己需要随意删减调整
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce);
                }
            }
        }
    }

}
