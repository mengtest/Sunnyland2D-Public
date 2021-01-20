using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject shadowPrefab;
    public int shadowCount;//计数prefab
    private Queue<GameObject> availableObjects = new Queue<GameObject>();//队列

    void Awake()
    {
        instance = this;

        //初始化对象池
        FillPool();
    }

    public void FillPool()//填满对象池
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);//临时变量，生成prefab
            newShadow.transform.SetParent(transform);//成为子集

            //取消启用,返回对象池
            ReturnPool(newShadow);//新生成好放入队列等待使用
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);//取消启用
        availableObjects.Enqueue(gameObject);//放入队列末端
    }

    public GameObject GetFormPool()//生成预制体影子
    {
        if (availableObjects.Count == 0)
        {
            FillPool();//再次填充
        }

        var outShadow = availableObjects.Dequeue();//临时变量，从队列开头获得GameObject
        outShadow.SetActive(true);//直接调用ShadowSprite的OnEnable等
        return outShadow;
    }
}