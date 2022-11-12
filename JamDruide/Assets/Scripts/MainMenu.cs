using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] int level;

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Text volume;
    [SerializeField] Slider slider;
    [SerializeField] private string saveName;
    float displayNumber;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat(saveName, 1));
        slider.value = PlayerPrefs.GetFloat(saveName, 1);
    }

    public void Play()
    {
        TimerManager.Instance.totalTime = 0;
        SceneManager.LoadScene(level);
    }

    public void GoTo(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);

        displayNumber = sliderValue * 100;
        volume.text = "s o u n d s : " + displayNumber.ToString("F0");
        PlayerPrefs.SetFloat(saveName, sliderValue);
    }

}
