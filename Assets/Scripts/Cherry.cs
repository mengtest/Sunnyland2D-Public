using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    void Disappear()
    {
        FindObjectOfType<PlayerController>().CherryCount();//找到角色控制器中樱桃数计算
        Destroy(gameObject);
    }
}
