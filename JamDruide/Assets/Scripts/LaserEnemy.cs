using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Player;
using UnityEditor;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem laserStart;
    [SerializeField] private ParticleSystem laserEnd;
    private Vector2 laserDirection;
    [SerializeField] private LineRenderer activeLineRenderer, inactiveLineRenderer;
    [SerializeField] private float laserDuration;
    [SerializeField] private float laserCastTime;
    private float startTime;
    private bool laserActive;
    [SerializeField] private PlayerActions player;
    [SerializeField] private Animator graphAnimator;

    private void Start()
    {
        DeactivateLaser();
        activeLineRenderer.SetPosition(0, transform.position);
        inactiveLineRenderer.SetPosition(0, transform.position);
    }

    private void DeactivateLaser()
    {
        activeLineRenderer.enabled = false;
        inactiveLineRenderer.enabled = false;
        laserEnd.Stop();
        laserStart.Stop();
    }

    public void Update()
    {
        if (player != null)
        {
            inactiveLineRenderer.enabled = true;
            LaserState();
            LaserCreation();
        }
        
    }

    private void LaserCreation()
    {
        laserDirection = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, laserDirection);
        inactiveLineRenderer.SetPosition(1, hit.point);
        if (laserActive)
        {
            activeLineRenderer.SetPosition(1, hit.point);
            laserEnd.transform.position = hit.point;
            laserEnd.transform.LookAt(transform.position);
            laserStart.transform.LookAt(hit.point);
            if (hit.transform.CompareTag("Player"))
            {
                StartCoroutine(player.Death());
                Invoke("DeactivateLaser", 1);
                laserActive = false;
                player = null;
            }
        }
        
    }

    private void LaserState()
    {
        if (!laserActive && Time.time >= startTime + laserCastTime)
        {
            laserActive = true;
            activeLineRenderer.enabled = true;
            laserStart.Play();
            laserEnd.Play();
            startTime = Time.time;
        }

        if (laserActive && Time.time >= startTime + laserDuration)
        {
            graphAnimator.Play("EnemyAttack");
            laserActive = false;
            startTime = Time.time;
            activeLineRenderer.enabled = false;
            DeactivateLaser();
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, laserDirection);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (player == null && col.CompareTag("Player"))
        {
            graphAnimator.Play("EnemyAttack");
            startTime = Time.time;
            player = col.GetComponent<PlayerActions>();
        }
    }
}
