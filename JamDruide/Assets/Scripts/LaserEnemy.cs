using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem laserStart;
    [SerializeField] private ParticleSystem laserEnd;
    private Vector2 laserDirection;
    [SerializeField] private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer.SetPosition(0, transform.position);
        laserStart.Play();
        laserEnd.Play();
    }

    public void Update()
    {
        LaserCreation();
    }

    private void LaserCreation()
    {
        laserDirection = PlayerActions.Instance.transform.position- transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, laserDirection);
        lineRenderer.SetPosition(1, hit.point);
        laserEnd.transform.position = hit.point;
        float angle = Vector2.Angle(hit.point, Vector2.right);
        laserEnd.transform.LookAt(transform.position);
        
        Debug.Log(hit.transform.name);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, laserDirection);
    }
}
