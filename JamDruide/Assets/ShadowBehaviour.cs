using System.Collections.Generic;
using Player;
using UnityEngine;

public class ShadowBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private float startDelay;
    
    private float startTime;
    private PlayerActions player;
    public Queue<Vector3> playerPositions = new();

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
        player = PlayerActions.Instance;
    }

    private void Update()
    {
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
}
