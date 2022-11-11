using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        Time.timeScale = 0f;
        OnPause.Invoke();
        pauseHUD.SetActive(true);
        gameHUD.SetActive(false);
    }

    void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        OnResume.Invoke();
        pauseHUD.SetActive(false);
        gameHUD.SetActive(true);
    }

    public static UnityEvent OnPause = new UnityEvent();
    public static UnityEvent OnResume = new UnityEvent();
}
