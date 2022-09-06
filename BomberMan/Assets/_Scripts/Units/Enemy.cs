using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using OknaaEXTENSIONS;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace BomberMan {
    public class Enemy : Unit {
        public LayerMask MovementStopper;
        public Transform DestinationPoint;
        public float tickTime = 0.5f;
        public static int EnemyCount = 5;
        public int moveSpeed;
        public float raycastDistance = 1f;
        public int killScore = 100;

        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private float _detectionCircleRadius = 0.2f;
        private float _tickTimer;
        private List<Vector2> _availableDirections = new List<Vector2>();


        private void Start() {
            DestinationPoint.parent = null;
            _tickTimer = tickTime * (1 + Random.Range(-0.2f, 0.2f));

            /*_rigidbody = GetComponent<Rigidbody2D>();
            
            var position = transform.position;
             
            var hitUp = Physics2D.Raycast(position + Vector3.up * 0.5f, Vector2.up, raycastDistance);
            var hitDown = Physics2D.Raycast(position + Vector3.down * 0.5f, Vector2.down, raycastDistance);
            var hitLeft = Physics2D.Raycast(position + Vector3.left * 0.5f, Vector2.left, raycastDistance);
            var hitRight = Physics2D.Raycast(position + Vector3.right * 0.5f, Vector2.right, raycastDistance);

            List<Vector3> possibleDirections = new List<Vector3>();

            if (hitUp.collider == null) possibleDirections.Add(Vector2.up);
            if (hitDown.collider == null) possibleDirections.Add(Vector2.down);
            if (hitLeft.collider == null) possibleDirections.Add(Vector2.left);
            if (hitRight.collider == null) possibleDirections.Add(Vector2.right);

            _moveDirection = possibleDirections.Random(Vector3.zero);*/
        }
        
        
        private void Update() {
            if(GameStateController.Instance.GetState() != GameState.Playing) return;
            
            _tickTimer -= Time.deltaTime;            
            if (Vector2.Distance(transform.position, DestinationPoint.position) <= 0.05f) {
                if (_tickTimer > 0) return;
                _tickTimer = tickTime;

                GetAvailableDirections();
                if (_availableDirections.Count > 0) {
                    DestinationPoint.position += (Vector3) _availableDirections.Random();
                    _tickTimer = tickTime;
                }
            }
            
            

            void GetAvailableDirections() {
                _availableDirections.Clear();
                _availableDirections.Add(Vector2.zero);
                if (!Physics2D.OverlapCircle(transform.position + Vector3.up,    _detectionCircleRadius, MovementStopper)) _availableDirections.Add(Vector2.up);
                if (!Physics2D.OverlapCircle(transform.position + Vector3.down,  _detectionCircleRadius, MovementStopper)) _availableDirections.Add(Vector2.down);
                if (!Physics2D.OverlapCircle(transform.position + Vector3.left,  _detectionCircleRadius, MovementStopper)) _availableDirections.Add(Vector2.left);
                if (!Physics2D.OverlapCircle(transform.position + Vector3.right, _detectionCircleRadius, MovementStopper)) _availableDirections.Add(Vector2.right);
            }
        }
        
        private void FixedUpdate() {
            if(GameStateController.Instance.GetState() != GameState.Playing) return;

            Move();
        }

        private void Move() {
            transform.position = Vector3.MoveTowards(transform.position, DestinationPoint.position, moveSpeed * Time.deltaTime);
        }
        
        /*private void FixedUpdate() {
            
            
            
            /*var position = _rigidbody.position;
            _rigidbody.MovePosition(position + _moveDirection * (moveSpeed * Time.fixedDeltaTime));
            FindFreeDirection();
            
            void FindFreeDirection() {
                var hit = Physics2D.Raycast(position + (Vector3)_moveDirection * 0.5f, _moveDirection, raycastDistance);
                if (hit.collider != null && CollidedWithObstacle()) {
                    _moveDirection = -_moveDirection;
                }
            
                bool CollidedWithObstacle() {
                    GameObject other = hit.collider.gameObject;
                    return other.CompareTag("Border") ||
                           other.CompareTag("Pillar") ||
                           other.CompareTag("Destructible") ||
                           other.CompareTag("Bomb") ||
                           other.CompareTag("Player");
                }
            }#1#
        }*/

        #region Collision Detection

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                GameStateController.Instance.SetState(GameState.GameOver);
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Explosion")) {
                Destroy(gameObject);
            }
        }

        #endregion


        public void OnDeath() {
            EnemyCount--;
            if (EnemyCount == 0) GameStateController.Instance.SetState(GameState.GameWon);
            
            print("OnDeath .. " + EnemyCount + " remains");
            ScoreController.Instance.AddScore(killScore);
            Destroy(gameObject);
        }
    }
}