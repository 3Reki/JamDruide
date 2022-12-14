using System;
using Player;
using UnityEngine;

namespace CameraScripts
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;
        public static Camera mainCamera;
    
        public Transform target;
        public float smoothTime = 0.3F;
        private Vector3 velocity = Vector3.zero;
        public Vector3 offset = new Vector3(0, 2, -10);
        
        public bool lockX;
        public float lockXPos;
        public bool lockY;
        public float lockYPos;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

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

        private void Update()
        {
            // Define a target position above and behind the target transform
            Vector3 targetPosition = target.TransformPoint(offset);

            if (PlayerController.Controls.Movement.LookDown.IsPressed())
            {
                targetPosition += new Vector3(0, -5);
            }

            if (lockX)
            {
                targetPosition.x = lockXPos;
            }

            if (lockY)
            {
                targetPosition.y = lockYPos;
            }
 
            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
