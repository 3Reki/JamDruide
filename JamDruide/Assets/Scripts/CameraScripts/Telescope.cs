using DG.Tweening;
using UnityEngine;

namespace CameraScripts
{
    public class Telescope : MonoBehaviour
    {
        [SerializeField] private GameObject input;
        
        [SerializeField] private Vector2 anchorPosition;
        [SerializeField] private float movementDuration;
        [SerializeField] private float viewSize;

        private bool oldState;
        private Vector3 oldPos;
        private float oldSize;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = input.GetComponent<SpriteRenderer>();
            
        }

        private void OnEnable()
        {
            UIManager.OnKeySpriteUpdate += UpdateInputSprite;
        }

        private void OnDisable()
        {
            UIManager.OnKeySpriteUpdate -= UpdateInputSprite;
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                input.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                input.SetActive(false);
            }
        }

        public void StartUsing()
        {
            oldState = CameraController.instance.enabled;
            oldPos = CameraController.mainCamera.transform.position;
            oldSize = CameraController.mainCamera.orthographicSize;

            CameraController.instance.enabled = false;
            CameraController.mainCamera.transform.DOMove(transform.TransformPoint(new Vector3(anchorPosition.x, anchorPosition.y, -10)), movementDuration);
            CameraController.mainCamera.DOOrthoSize(viewSize, movementDuration);
        }

        public void StopUsing()
        {
            if (oldState)
            {
                CameraController.instance.enabled = true;
            }
            else
            {
                CameraController.mainCamera.transform.DOMove(oldPos, movementDuration);
            }
            
            CameraController.mainCamera.DOOrthoSize(oldSize, movementDuration);
        }
        
        private void UpdateInputSprite(SpriteArrayList spriteLists)
        {
            spriteRenderer.sprite = spriteLists.GetSpriteForInputActionName("Interact");
        }

#if UNITY_EDITOR
        private const float aspectRatio = 16f / 9f;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.TransformPoint(anchorPosition));
            Gizmos.DrawSphere(transform.TransformPoint(anchorPosition), .5f);
            Gizmos.DrawWireCube(transform.TransformPoint(anchorPosition), new Vector3(viewSize * 2 * aspectRatio, viewSize * 2));
        }
#endif
    }
}
