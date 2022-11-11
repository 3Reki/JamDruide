using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPause;

    [SerializeField] GameObject gameHUD;
    [SerializeField] GameObject pauseHUD;

    private void Update()
    {
        MyInputs();

    }

    void MyInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPause = !gameIsPause;
            if (gameIsPause)
                Pause();
            else
                Resume();
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        OnPause.Invoke();
        pauseHUD.SetActive(true);
        gameHUD.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        OnResume.Invoke();
        pauseHUD.SetActive(false);
        gameHUD.SetActive(true);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public static UnityEvent OnPause = new UnityEvent();
    public static UnityEvent OnResume = new UnityEvent();
}
