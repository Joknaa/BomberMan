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
        public static bool RunAwayFromPlayer = false;
        public LayerMask MovementStopper;
        public Transform DestinationPoint;
        public float tickTime = 0.5f;
        public static int EnemyCount = 5;
        public int moveSpeed;
        public float raycastDistance = 1f;
        public int killScore = 100;

        private GameObject _player;
        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private float _detectionCircleRadius = 0.2f;
        private float _tickTimer;
        private List<Vector2> _availableDirections = new List<Vector2>();
        private List<Vector2> _RunAwayFromDirections = new List<Vector2>();
        private Vector2 _lastDirection;
        private bool _isDead;

        private void Start() {
            DestinationPoint.parent = null;
            _tickTimer = tickTime * (1 + Random.Range(-0.2f, 0.2f));
            _player = GameObject.FindGameObjectWithTag("Player");
        }


        private void Update() {
            if (GameStateController.Instance.GetState() != GameState.Playing) return;

            _tickTimer -= Time.deltaTime;
            if (Vector2.Distance(transform.position, DestinationPoint.position) <= 0.05f) {
                if (_tickTimer > 0) return;
                _tickTimer = tickTime;

                GetAvailableDirections();
                if (RunAwayFromPlayer) {
                    ExcludePlayerDirections();
                }
                if (_availableDirections.Count > 0) {
                    
                    DestinationPoint.position += (Vector3)_availableDirections.Random();
                    _tickTimer = tickTime;
                    
                }
            }

            void GetAvailableDirections() {
                _availableDirections.Clear();
                _availableDirections.Add(Vector2.zero);
                if (!Physics2D.OverlapCircle(transform.position + Vector3.up, _detectionCircleRadius, MovementStopper)) _availableDirections.Add(Vector2.up);
                if (!Physics2D.OverlapCircle(transform.position + Vector3.down, _detectionCircleRadius, MovementStopper)) _availableDirections.Add(Vector2.down);
                if (!Physics2D.OverlapCircle(transform.position + Vector3.left, _detectionCircleRadius, MovementStopper)) _availableDirections.Add(Vector2.left);
                if (!Physics2D.OverlapCircle(transform.position + Vector3.right, _detectionCircleRadius, MovementStopper)) _availableDirections.Add(Vector2.right);
            }
            
            void ExcludePlayerDirections() {
                var playerPosition = _player.transform.position;
                var enemyPosition = transform.position;
                var direction = (playerPosition - enemyPosition);
            
                _RunAwayFromDirections.Clear();
                _RunAwayFromDirections.Add(direction.x > 0 ? Vector2.right : direction.x < 0 ? Vector2.left : Extensions.GetRandomDirection(right: true, left: true, zero: true));
                _RunAwayFromDirections.Add(direction.y > 0 ? Vector2.up : direction.y < 0 ? Vector2.down : Extensions.GetRandomDirection(up: true, down: true, zero: true));
            
                _availableDirections.Subtract(_RunAwayFromDirections);
            }
        }
        
        private void FixedUpdate() {
            if (GameStateController.Instance.GetState() != GameState.Playing) return;
            transform.position = Vector3.MoveTowards(transform.position, DestinationPoint.position, moveSpeed * Time.deltaTime);
        }


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
            _isDead = true;
            EnemyCount--;
            if (EnemyCount == 0) GameStateController.Instance.SetState(GameState.GameWon);
            if (EnemyCount == 2) RunAwayFromPlayer = true;

            print("OnDeath .. " + EnemyCount + " remains");
            ScoreController.Instance.AddScore(killScore);
            Destroy(gameObject);
        }

        public bool IsDead() => _isDead;
    }
}