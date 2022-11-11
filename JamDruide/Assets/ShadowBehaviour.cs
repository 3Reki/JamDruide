using System;
using Player;
using UnityEngine;

public class ShadowBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private float startDelay;
    private float startTime;

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
}
