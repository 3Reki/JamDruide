using DG.Tweening;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CameraScripts
{
    public class FollowCameraTrigger : MonoBehaviour
    {
        [SerializeField] private float movementDuration;
        [SerializeField] private float viewSize;
        [SerializeField] private bool lockX;
        [SerializeField] private float lockXPos;
        [SerializeField] private bool lockY;
        [SerializeField] private float lockYPos;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CameraController.instance.enabled = true;
                CameraController.instance.lockX = lockX;
                CameraController.instance.lockXPos = lockXPos;
                CameraController.instance.lockY = lockY;
                CameraController.instance.lockYPos = lockYPos;
                CameraController.mainCamera.DOOrthoSize(viewSize, movementDuration);
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 position = transform.position;
            Vector2 colliderSize = GetComponent<BoxCollider2D>().size;
            
            Gizmos.color = new Color(1, .4f, .4f);
            Gizmos.DrawWireCube(position + new Vector3(0, 0, -1), colliderSize);
            Gizmos.color = new Color(1, .15f, .15f, .7f);
            Gizmos.DrawCube(position, colliderSize);
            
            var style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState {textColor = Color.white}
            };

            Handles.Label(position + new Vector3(0, 0, -2), "FollowCameraTrigger", style);

        }
#endif
    }
}
