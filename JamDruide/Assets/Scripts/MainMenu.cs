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

    public void Play()
    {
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
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);

        displayNumber = sliderValue * 100;
        volume.text = "s o u n d s : " + displayNumber.ToString("F0");
        PlayerPrefs.SetFloat(saveName, sliderValue);
    }

}
