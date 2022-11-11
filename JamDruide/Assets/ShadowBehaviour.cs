using System;
using Player;
using UnityEngine;

public class ShadowBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private float startDelay;
    private float startTime;

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
    }

    private void Update()
    {
        if (Time.time >= startTime + startDelay)
        {
            transform.position = playerActions.playerPositions.Dequeue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayerActions.Instance.Death());
        }
    }
}
