using System.Collections;
using CameraScripts;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CinematicManager : MonoBehaviour
{

    [SerializeField] private GameObject credits;
    [SerializeField] private Image whiteScreen;
    [SerializeField] private Sprite raisingSword;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Animator blow;
    [SerializeField] private GameObject victoriousPlayer;
    [SerializeField] private AudioSource drawingSword;
    [SerializeField] private Transform cosmosEnemy;
    [SerializeField] private Transform enemyCinematicPosition;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject particles;
    [SerializeField] private TextMeshProUGUI timerResult;

    public void StartCinematic()
    {
        cosmosEnemy.GetComponent<ShadowBehaviour>().enabled = false;
        cosmosEnemy.GetComponent<CircleCollider2D>().enabled = false;
        PlayerActions.Instance.enabled = false;
        blow.Play("Blow");
        CameraController.instance.target = victoriousPlayer.transform;
        HUD.SetActive(false);
        text.SetActive(true);
        particles.SetActive(true);
        timerResult.gameObject.SetActive(true);
        TimerManager.Instance.HideText();
        timerResult.text = "Your total time is : " + TimerManager.FormatTime(TimerManager.Instance.totalTime);
        
        StartCoroutine(EndingCoroutine());
    }

    private IEnumerator EndingCoroutine()
    {
        WaitForSecondsRealtime waitOne = new WaitForSecondsRealtime(1);
        yield return waitOne;
        PlayerActions.Instance.GetComponent<PlayerController>().enabled = false;
        PlayerActions.Instance.GetComponent<SpriteRenderer>().enabled = false;
        victoriousPlayer.SetActive(true);
        
        var position = enemyCinematicPosition.position;
        cosmosEnemy.position = position;
        
        yield return waitOne;
        CameraController.instance.offset = new Vector3(-2, 0, -10);
        CameraController.mainCamera.DOOrthoSize(7, 1f);

        yield return new WaitForSecondsRealtime(.5f);
        var dummySpriteRenderer = victoriousPlayer.GetComponent<SpriteRenderer>();
        dummySpriteRenderer.flipX = true;
        
        yield return new WaitForSecondsRealtime(0.6f);
        drawingSword.Play();
        
        yield return new WaitForSecondsRealtime(0.2f);
        Sprite firstSprite = dummySpriteRenderer.sprite;
        dummySpriteRenderer.sprite = raisingSword;
        

        yield return new WaitForSecondsRealtime(2);
        Instantiate(explosion, position + Vector3.back, Quaternion.identity);
        
        yield return new WaitForSecondsRealtime(.15f);
        cosmosEnemy.gameObject.SetActive(false);
        dummySpriteRenderer.sprite = firstSprite;

        yield return new WaitForSecondsRealtime(.7f);
        CameraController.instance.enabled = false;
        CameraController.mainCamera.transform.DOLocalMoveY(40, 8).SetEase(Ease.InSine);
        
        yield return new WaitForSecondsRealtime(2f);
        whiteScreen.DOFade(1, 6).SetEase(Ease.InSine);

        InputSystem.onAnyButtonPress.CallOnce(_ =>
        {
            StartCoroutine(CreditsCoroutine());
        });
    }

    private IEnumerator CreditsCoroutine()
    {
        timerResult.gameObject.SetActive(false);
        text.SetActive(false);
        credits.SetActive(true);

        yield return new WaitForSeconds(.5f);
        InputSystem.onAnyButtonPress.CallOnce(_ => SceneManager.LoadScene(0));
    }
}
