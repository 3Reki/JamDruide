using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPause;

    [SerializeField] private GameObject gameHUD;
    [SerializeField] private GameObject pauseHUD;
    [SerializeField] private GameObject optionHUD;

    //audio
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Text volume;
    [SerializeField] private Slider slider;
    [SerializeField] private string saveName;
    private float displayNumber;

    private void Start()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat(saveName, 1));
        slider.value = PlayerPrefs.GetFloat(saveName, 1);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        MyInputs();

    }

    private void MyInputs()
    {
        if (PlayerController.Controls.Other.Menu.WasPerformedThisFrame())
        {
            gameIsPause = !gameIsPause;
            if (gameIsPause)
                Pause();
            else
                Resume();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        OnPause.Invoke();
        pauseHUD.SetActive(true);
        gameHUD.SetActive(false);
        gameIsPause = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        OnResume.Invoke();
        pauseHUD.SetActive(false);
        gameHUD.SetActive(true);
        optionHUD.SetActive(false);
        gameIsPause = false;
    }

    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
        displayNumber = sliderValue * 100;
        volume.text = "s o u n d s : " + displayNumber.ToString("F0");
        PlayerPrefs.SetFloat(saveName, sliderValue);
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
