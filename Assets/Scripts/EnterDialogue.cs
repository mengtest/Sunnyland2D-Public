using UnityEngine;

public class EnterDialogue : MonoBehaviour
{
    public GameObject enterDialogue;

    private void OnTriggerEnter2D(Collider2D collision)//如果一个2D碰撞器进入触发器
    {
        if (collision.tag == "Player")
        {
            enterDialogue.SetActive(true);//激活对话面板
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//如果一个2D碰撞器停止接触触发器
    {
        if (collision.tag == "Player")
        {
            enterDialogue.SetActive(false); //取消激活对话面板
        }
    }
}
