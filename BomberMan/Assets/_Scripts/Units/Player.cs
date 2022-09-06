
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BomberMan {
    public class Player : Unit {
        public LayerMask MovementStopper;
        public Transform DestinationPoint;
        public int moveSpeed;
        public int bombCount;
        public int bombRange;
        public int bombTimer;

    
        private Vector2 _moveDirection;
        private Vector2 _horizontalDirection;
        private Vector2 _verticalDirection;
        private Rigidbody2D _rigidbody;
        private float _detectionCircleRadius = 0.2f;

        private void Start() {
            //_rigidbody = GetComponent<Rigidbody2D>();
            DestinationPoint.parent = null;
            //DestinationPoint.position = transform.position;
        }

        private void Update() {
            if(GameStateController.Instance.GetState() != GameState.Playing) return;
            
            var distanceToDestination = Vector2.Distance(transform.position, DestinationPoint.position);
            if (distanceToDestination <= 0.05f) {
                var horizontalInput = Input.GetAxisRaw("Horizontal");
                var horizontalDirection = new Vector3(horizontalInput, 0, 0);
                var verticalInput = Input.GetAxisRaw("Vertical");
                var verticalDirection = new Vector3(0, verticalInput, 0);
                
                if (Mathf.Abs(horizontalInput) == 1 ) {
                    if (!IsSomethingAhead(horizontalDirection)) {
                        DestinationPoint.position += horizontalDirection;
                    }
                } else if (Mathf.Abs(verticalInput) == 1) {
                    if (!IsSomethingAhead(verticalDirection)) {
                        DestinationPoint.position += verticalDirection;
                    }
                }
            }
            
            /*
             Old Movement
             var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");
            _horizontalDirection = horizontalInput > 0 ? Vector2.right : horizontalInput < 0 ? Vector2.left : Vector2.zero;
            _verticalDirection = verticalInput > 0 ? Vector2.up : verticalInput < 0 ? Vector2.down : Vector2.zero;
            _moveDirection = _horizontalDirection + _verticalDirection;*/
            
            if (Input.GetKeyDown(KeyCode.Space) && bombCount > 0) {
                PlaceBomb();
                bombCount--;
            }
            
            bool IsSomethingAhead(Vector3 direction) {
                return Physics2D.OverlapCircle(DestinationPoint.position + direction, _detectionCircleRadius, MovementStopper);
            }
        }
        
        private void FixedUpdate() {
            if(GameStateController.Instance.GetState() != GameState.Playing) return;

            Move();
        }

        private void Move() {
            // _rigidbody.velocity = _moveDirection * (moveSpeed * Time.fixedDeltaTime * 100) ;
            transform.position = Vector3.MoveTowards(transform.position, DestinationPoint.position, moveSpeed * Time.deltaTime);

        }

        private void PlaceBomb() {
            if(GameStateController.Instance.GetState() != GameState.Playing) return;

            _gridManager.PlaceBomb(transform.position, bombRange, bombTimer);
        }

        public void OnDeath() {
            if(GameStateController.Instance.GetState() != GameState.Playing) return;

            GameStateController.Instance.SetState(GameState.GameOver);
            Destroy(gameObject);
        }
    }
}