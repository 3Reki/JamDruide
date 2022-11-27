using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private int level;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Text volume;
    [SerializeField] private Slider slider;
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private string saveName;
    private float displayNumber;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat(saveName, 1));
        MusicInstance.instance.SwapMusicClip(defaultMusic, 1);
        slider.value = PlayerPrefs.GetFloat(saveName, 1);
        TimerManager.Instance.HideText();
    }

    public void Play()
    {
        TimerManager.Instance.ResetTimer();
        TimerManager.Instance.ShowText();
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
