using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer thisSprite;//当前图像
    private SpriteRenderer playerSprite;//玩家图像
    private Color color;

    [Header("Time Set Parameter")]
    public float activeTime;//显示时间
    public float activeStart;//开始显示的时间点

    [Header("Alpha Set")]
    private float alpha;//不透明度
    public float alphaSet;//初始值
    public float alphaMultiplier;//连续不断乘积变小

    private void OnEnable()//一开始获得参数
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;//查找player
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        thisSprite.sprite = playerSprite.sprite;//显示刚才player的图像

        transform.position = player.position;//获得player的坐标等
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;//记录开始显示时间点
    }

    void Update()
    {
        alpha *= alphaMultiplier;//不透明值越来越少
        color = new Color(0.5f, 0.5f, 1, alpha);//偏蓝色 Color(1,1,1,1)代表100%显示各通道颜色，请查看Api手册
        thisSprite.color = color;//覆盖当前sprite颜色

        if (Time.time >= activeStart + activeTime)//超过开始时间+显示时间
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);//返回ShadowPool当前物体
        }
    }
}