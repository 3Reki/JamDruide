using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public static TimerManager Instance;
    [SerializeField] private TextMeshProUGUI timerText;
    public float totalTime;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        totalTime += Time.deltaTime;
        timerText.text = FormatTime(totalTime);
    }

    public void ResetTimer()
    {
        totalTime = 0;
    }

    public static string FormatTime(float time)
    {
        return $"{((int) time) / 3600:00}:{((int) time) % 3600 / 60:00}:{((int) time) % 3600 % 60:00}";
    }
}
