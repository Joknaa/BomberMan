using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BomberMan {
    public class Player : Unit {
        public LayerMask MovementStopper;
        public Transform DestinationPoint;
        public int moveSpeed;
        // public float moveDuration;
        public int bombCount;
        public int bombRange;
        public int bombTimer;

        private float _detectionCircleRadius = 0.2f;
        private bool _isDead;
        private bool _isMoving;

        private void Start() {
            DestinationPoint.parent = null;
        }

        private void Update() {
            if (GameStateController.Instance.GetState() != GameState.Playing) return;
            /*var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");
            print("horizontalInput: " + horizontalInput + " verticalInput: " + verticalInput);
            if (verticalInput > 0 && !_isMoving && !IsSomethingAhead(Vector2.up)) StartCoroutine(Move(Vector2.up));
            if (verticalInput < 0 && !_isMoving && !IsSomethingAhead(Vector2.down)) StartCoroutine(Move(Vector2.down));
            if (horizontalInput < 0 && !_isMoving && !IsSomethingAhead(Vector2.left)) StartCoroutine(Move(Vector2.left));
            if (horizontalInput > 0 && !_isMoving && !IsSomethingAhead(Vector2.right)) StartCoroutine(Move(Vector2.right));*/
            

            var distanceToDestination = Vector2.Distance(transform.position, DestinationPoint.position);
            if (distanceToDestination <= 0.05f) {
                var horizontalInput = Input.GetAxisRaw("Horizontal");
                var horizontalDirection = new Vector3(horizontalInput, 0, 0);
                var verticalInput = Input.GetAxisRaw("Vertical");
                var verticalDirection = new Vector3(0, verticalInput, 0);
                
                if (Mathf.Abs(horizontalInput) == 1) {
                    if (!IsSomethingAhead(horizontalDirection)) {
                        DestinationPoint.position += horizontalDirection;
                    }
                } else if (Mathf.Abs(verticalInput) == 1) {
                    if (!IsSomethingAhead(verticalDirection)) {
                        DestinationPoint.position += verticalDirection;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && bombCount > 0) {
                PlaceBomb();
                bombCount--;
            }

            bool IsSomethingAhead(Vector3 direction) {
                return Physics2D.OverlapCircle(DestinationPoint.position + direction, _detectionCircleRadius, MovementStopper);
            }
        }

        private void FixedUpdate() {
            if (GameStateController.Instance.GetState() != GameState.Playing) return;
            transform.position = Vector3.MoveTowards(transform.position, DestinationPoint.position, moveSpeed * Time.deltaTime);
        }

        /*
        private IEnumerator Move(Vector2 moveDirection) {
            _isMoving = true;
            var elapsedTime = 0f;
            var originalPosition = transform.position;
            DestinationPoint.position += (Vector3) moveDirection;
            var targetPosition = DestinationPoint.position + (Vector3)moveDirection;
 
            while (elapsedTime < moveDuration) {
                transform.position = Vector2.Lerp(originalPosition, DestinationPoint.position, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null; 
            }
            transform.position = DestinationPoint.position;
            _isMoving = false;
        }
        */

        private void PlaceBomb() {
            if (GameStateController.Instance.GetState() != GameState.Playing) return;
            _gridManager.PlaceBomb(transform.position, bombRange, bombTimer);
        }

        public void OnDeath() {
            _isDead = true;
            if (GameStateController.Instance.GetState() != GameState.Playing) return;
            GameStateController.Instance.SetState(GameState.GameOver);
            Destroy(gameObject);
        }

        public bool IsDead() => _isDead;
    }
}