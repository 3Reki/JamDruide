using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class ShadowBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerActions player;
    [SerializeField] private float startDelay;
    [SerializeField] private AudioClip epicDrillRemix;
    
    private float startTime;
    private readonly Queue<Vector3> playerPositions = new();
    private bool playerMoved;

    private void OnEnable()
    {
        PauseMenu.OnPause.AddListener(() => enabled = false);
        PauseMenu.OnResume.AddListener(() => enabled = true);
    }

    private void OnDisable()
    {
        PauseMenu.OnPause.RemoveListener(() => enabled = false);
        PauseMenu.OnResume.RemoveListener(() => enabled = true);
    }

    private void Start()
    {
        startTime = Time.time;
        MusicInstance.instance.SwapMusicClip(epicDrillRemix, 0.22f);
        StartCoroutine(WaitForMove());
    }

    private void FixedUpdate()
    {
        if (!playerMoved)
            return;
        
        SavePlayerPosition();
        
        if (Time.time >= startTime + startDelay)
        {
            transform.position = playerPositions.Dequeue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(player.Death());
        }
    }
    
    private void SavePlayerPosition()
    {
        playerPositions.Enqueue(player.transform.position);
    }

    private IEnumerator WaitForMove()
    {
        Vector3 initialPlayerPos = player.transform.position;
        while (Vector3.Distance(player.transform.position, initialPlayerPos) < 0.5f)
        {
            startTime = Time.time;
            yield return null;
        }

        playerMoved = true;
    }
}
