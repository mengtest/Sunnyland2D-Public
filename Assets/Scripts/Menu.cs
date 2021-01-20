using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer audioMixer;
    public Slider slider;
    public PlayerController player;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//加载下一个场景
    }

    public void ExitGame()
    {
        Application.Quit();//退出游戏
    }

    public void UIEnable()
    {
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);//激活“UI”面板
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);//打开暂停菜单
        Time.timeScale = 0f;//画面停止
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);//关闭暂停菜单
        Time.timeScale = 1f;//画面恢复
    }

    public void SetVolume()
    {
        audioMixer.SetFloat("MainVolume", slider.value);
    }

    public void EnterHouse()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//加载下一个场景
    }

    public void NewJumping()
    {
        PlayerController.player.NewJump();
    }

    public void GoToMainMenu()//回主菜单
    {
        SceneManager.LoadScene(0);
    }

    public void ButtonDash()
    {
        if (Time.time >= (PlayerController.player.lastDash + 
            PlayerController.player.dashCoolDown))//超过上次的时间和CD时间
        {
            //可以执行dash
            PlayerController.player.ReadyToDash();
        }
    }
}
