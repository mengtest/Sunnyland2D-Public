using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))//按下键盘E键
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);//加载下一个场景
        }
    }
}
