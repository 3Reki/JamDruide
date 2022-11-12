using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public static TimerManager Instance;
    [SerializeField] private TextMeshProUGUI timerText;
    private float totalTime;
    private float currentLevelTime;
    [SerializeField] private List<float> times;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        totalTime += Time.deltaTime;
        currentLevelTime += Time.deltaTime;
        timerText.text = FormatTime(currentLevelTime);
    }

    public void SaveTime()
    {
        times.Add(currentLevelTime);
        currentLevelTime = 0;
        
    }
    
    public static string FormatTime(float time)
    {
        return $"{((int) time) / 3600:00}:{((int) time) % 3600 / 60:00}:{((int) time) % 3600 % 60:00}";
    }
}
