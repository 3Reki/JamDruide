using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// Right now it only contains movement and jumping, but it should be pretty easy to expand... I may even do it myself
    /// if there's enough interest. You can play and compete for best times here: https://tarodev.itch.io/
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/GqeHHnhHpz
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public bool CanDoubleJump;
        
        // Public for external hooks
        public Vector3 Velocity { get; private set; }
        public FrameInput Input { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector3 RawMovement { get; private set; }
        public bool Grounded => colDown;

        private Vector3 lastPosition;
        private float currentHorizontalSpeed, currentVerticalSpeed;

        // This is horrible, but for some reason colliders are not fully established when update starts...
        private bool active;
        private void Awake() => Invoke(nameof(Activate), 0.5f);
        private void Activate() => active = true;

        private SpriteRenderer sprite;
        private Animator animator;

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
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!active) return;
            // Calculate velocity
            Vector3 position = transform.position;
            Velocity = (position - lastPosition) / Time.deltaTime;
            lastPosition = position;

            GatherInput();
            if (Input.x > 0)
            {
                sprite.flipX = false;
            }
            else if (Input.x < 0)
            {
                sprite.flipX = true;
            }

            if (Velocity.y < 0)
            {
                animator.SetBool("IsFalling", true);
            }

            RunCollisionChecks();

            CalculateWalk(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical
            CalculateDoubleJump();

            MoveCharacter(); // Actually perform the axis movement
        }


        #region Gather Input

        private void GatherInput()
        {
            Input = new FrameInput
            {
                jumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                jumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                x = UnityEngine.Input.GetAxisRaw("Horizontal")
            };
            if (Input.jumpDown)
            {
                lastJumpPressed = Time.time;
            }
        }

        #endregion

        #region Collisions

        public Bounds CharacterBounds
        {
            get => _characterBounds;
            set => _characterBounds = value;
        }
        
        [Header("COLLISION")] [SerializeField] private Bounds _characterBounds;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;
        [SerializeField] [Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

        private RayRange raysUp, raysRight, raysDown, raysLeft;
        private bool colUp, colRight, colDown, colLeft;

        private float timeLeftGrounded;

        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks()
        {
            // Generate ray ranges. 
            CalculateRayRanged();

            // Ground
            LandingThisFrame = false;
            var groundedCheck = RunDetection(raysDown);
            animator.SetBool("Grounded", groundedCheck);
            if (groundedCheck)
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }
            if (colDown && !groundedCheck) timeLeftGrounded = Time.time; // Only trigger when first leaving
            else if (!colDown && groundedCheck)
            {
                coyoteUsable = true; // Only trigger when first touching
                LandingThisFrame = true;
            }

            colDown = groundedCheck;

            // The rest
            colUp = RunDetection(raysUp);
            colLeft = RunDetection(raysLeft);
            colRight = RunDetection(raysRight);

            bool RunDetection(RayRange range)
            {
                return EvaluateRayPositions(range)
                    .Any(point => Physics2D.Raycast(point, range.dir, _detectionRayLength, _groundLayer));
            }
        }

        private void CalculateRayRanged()
        {
            // This is crying out for some kind of refactor. 
            var b = new Bounds(transform.position + _characterBounds.center, _characterBounds.size);

            raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
            raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
            raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
            raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
        }


        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        {
            for (var i = 0; i < _detectorCount; i++)
            {
                var t = (float) i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.start, range.end, t);
            }
        }

        private void OnDrawGizmos()
        {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying)
            {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> {raysUp, raysRight, raysDown, raysLeft})
                {
                    foreach (var point in EvaluateRayPositions(range))
                    {
                        Gizmos.DrawRay(point, range.dir * _detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            Gizmos.color = Color.red;
            var move = new Vector3(currentHorizontalSpeed, currentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center + move, _characterBounds.size);
        }

        #endregion


        #region Walk

        [Header("WALKING")] [SerializeField] private float _acceleration = 90;
        public float moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _apexBonus = 2;

        private void CalculateWalk()
        {
            if (Input.x != 0)
            {
                // Set horizontal move speed
                currentHorizontalSpeed += Input.x * _acceleration * Time.deltaTime;

                // clamped by max frame movement
                currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -moveClamp, moveClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(Input.x) * _apexBonus * apexPoint;
                currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else
            {
                // No input. Let's slow the character down
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
            }

            if (currentHorizontalSpeed > 0 && colRight || currentHorizontalSpeed < 0 && colLeft)
            {
                // Don't walk through walls
                currentHorizontalSpeed = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")] [SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float fallSpeed;

        private void CalculateGravity()
        {
            if (colDown)
            {
                // Move out of the ground
                if (currentVerticalSpeed < 0) currentVerticalSpeed = 0;
            }
            else
            {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = endedJumpEarly && currentVerticalSpeed > 0
                    ? this.fallSpeed * _jumpEndEarlyGravityModifier
                    : this.fallSpeed;

                // Fall
                currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (currentVerticalSpeed < _fallClamp) currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion

        #region Jump

        [Header("JUMPING")] [SerializeField] private float _jumpHeight = 30;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        private bool coyoteUsable;
        private bool endedJumpEarly = true;
        private float apexPoint; // Becomes 1 at the apex of a jump
        private float lastJumpPressed;
        private bool doubleJumpUsed;
        private bool CanUseCoyote => coyoteUsable && !colDown && timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => colDown && lastJumpPressed + _jumpBuffer > Time.time;
        private bool CanUseDouble => CanDoubleJump && !doubleJumpUsed && !JumpingThisFrame;

        private void CalculateJumpApex()
        {
            if (!colDown)
            {
                // Gets stronger the closer to the top of the jump
                apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, apexPoint);
            }
            else
            {
                apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (Input.jumpDown && CanUseCoyote || HasBufferedJump)
            {
                currentVerticalSpeed = _jumpHeight;
                endedJumpEarly = false;
                coyoteUsable = false;
                timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
                doubleJumpUsed = false;
                animator.SetBool("IsJumping", true);
            }
            else
            {
                JumpingThisFrame = false;
            }

            // End the jump early if button released
            if (!colDown && Input.jumpUp && !endedJumpEarly && Velocity.y > 0)
            {
                // _currentVerticalSpeed = 0;
                endedJumpEarly = true;
            }

            if (colUp)
            {
                if (currentVerticalSpeed > 0) currentVerticalSpeed = 0;
            }
        }

        private void CalculateDoubleJump()
        {
            if (CanUseDouble && Input.jumpDown && !CanUseCoyote)
            {
                currentVerticalSpeed = _jumpHeight;
                endedJumpEarly = false;
                coyoteUsable = false;
                timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
                doubleJumpUsed = true;
            }
        }

        #endregion

        #region Move

        [Header("MOVE")]
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter()
        {
            Vector3 transformPos = transform.position;
            Vector3 pos = transformPos + _characterBounds.center;
            RawMovement = new Vector3(currentHorizontalSpeed, currentVerticalSpeed); // Used externally
            Vector3 move = RawMovement * Time.deltaTime;
            if (move != Vector3.zero)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
            Vector3 furthestPoint = pos + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
            if (!hit)
            {
                transform.position += move;
                return;
            }

            // otherwise increment away from current pos; see what closest position we can move to
            Vector3 positionToMoveTo = transformPos;
            for (int i = 1; i < _freeColliderIterations; i++)
            {
                // increment to check all but furthestPoint - we did that already
                var t = (float) i / _freeColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer))
                {
                    transform.position = positionToMoveTo;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i == 1)
                    {
                        if (currentVerticalSpeed < 0) currentVerticalSpeed = 0;
                        var dir = transformPos - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }

                positionToMoveTo = posToTry;
            }
        }

        #endregion
    }
}

