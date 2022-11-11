using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        activeLineRenderer.enabled = false;
        activeLineRenderer.SetPosition(0, transform.position);
        inactiveLineRenderer.SetPosition(0, transform.position);
        laserStart.Play();
        laserEnd.Play();
        startTime = Time.time;
    }

    public void Update()
    {
        LaserCreation();
        if (!laserActive && Time.time >= startTime + laserCastTime)
        {
            laserActive = true;
            activeLineRenderer.enabled = true;
            startTime = Time.time;
        }

        if (laserActive && Time.time >= startTime + laserDuration)
        {
            laserActive = false;
            startTime = Time.time;
            activeLineRenderer.enabled = false;
        }
    }

    private void LaserCreation()
    {
        laserDirection = PlayerActions.Instance.transform.position- transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, laserDirection);
        inactiveLineRenderer.SetPosition(1, hit.point);
        if (laserActive)
        {
            activeLineRenderer.SetPosition(1, hit.point);
            laserEnd.transform.position = hit.point;
            float angle = Vector2.Angle(hit.point, Vector2.right);
            laserEnd.transform.LookAt(transform.position);
            if (hit.transform.CompareTag("Player"))
            {
                StartCoroutine(PlayerActions.Instance.Death());
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, laserDirection);
    }
}
