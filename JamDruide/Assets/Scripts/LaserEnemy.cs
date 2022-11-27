using Player;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    public GameObject door;
    
    [SerializeField] private ParticleSystem laserStart;
    [SerializeField] private ParticleSystem laserEnd;
    [SerializeField] private LineRenderer activeLineRenderer, inactiveLineRenderer;
    [SerializeField] private float laserDuration;
    [SerializeField] private float laserCastTime;
    [SerializeField] private Animator graphAnimator;
    [SerializeField] private LayerMask layer;
    [SerializeField] private AudioClip shoot;
    
    private Vector2 laserDirection;
    private float startTime;
    private bool laserActive;
    private PlayerActions player;

    private new AudioSource audio;
    private void Start()
    {
        DeactivateLaser();
        Vector3 position = transform.position;
        activeLineRenderer.SetPosition(0, position);
        inactiveLineRenderer.SetPosition(0, position);
        audio = GetComponent<AudioSource>();
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
        Vector3 position = transform.position;
        laserDirection = player.transform.position - position;
        RaycastHit2D hit = Physics2D.Raycast(position, laserDirection, 100, layer);

        inactiveLineRenderer.SetPosition(1, hit.point);
        
        if (!laserActive) return;
        
        activeLineRenderer.SetPosition(1, hit.point);
        laserEnd.transform.position = hit.point;
        laserEnd.transform.LookAt(transform.position);
        laserStart.transform.LookAt(hit.point);
            
        if (hit.collider == null || !hit.transform.CompareTag("Player")) return;
            
        player.StartCoroutine(player.Death());
        Invoke(nameof(DeactivateLaser), 1);
        laserActive = false;
        player = null;

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
            audio.PlayOneShot(shoot);

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

    public void Death()
    {
        door.GetComponent<Animator>().Play("DoorOpen");

        Destroy(gameObject);
    }
}
