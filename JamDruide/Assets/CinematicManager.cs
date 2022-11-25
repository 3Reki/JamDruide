using Player;
using TMPro;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{

    [SerializeField] private Animator blow;
    [SerializeField] private GameObject victoriousPlayer;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject particles;
    [SerializeField] private TextMeshProUGUI timerResult;
    
    public void StartCinematic()
    {
        PlayerActions.Instance.GetComponent<PlayerController>().enabled = false;
        PlayerActions.Instance.GetComponent<SpriteRenderer>().enabled = false;
        PlayerActions.Instance.enabled = false;
        blow.Play("Blow");
        victoriousPlayer.SetActive(true);
        HUD.SetActive(false);
        text.SetActive(true);
        particles.SetActive(true);
        timerResult.gameObject.SetActive(true);
        TimerManager.Instance.HideText();
        timerResult.text = "Your total time is : " + TimerManager.FormatTime(TimerManager.Instance.totalTime);
    }
}
