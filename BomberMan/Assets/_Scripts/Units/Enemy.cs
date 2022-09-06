using System;
using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BomberMan {
    public class Enemy : Unit {
        public static event Action<int> OnEnemyKilled;
        public static bool RunAwayFromPlayer = false;
        public static int EnemyCount = 5;
        private static int _enemiesLeft;

        public LayerMask MovementStopper;
        public Transform DestinationPoint;
        public float tickTime = 0.5f;
        public int moveSpeed;
        public float raycastDistance = 1f;
        public int killScore = 100;

        
        private readonly List<Vector2> _availableDirections = new List<Vector2>();
        private readonly List<Vector2> _runAwayFromDirections = new List<Vector2>();
        private readonly float _detectionCircleRadius = 0.2f;
        private GameObject _player;
        private float _tickTimer;
        private bool _isDead;

        private void Start() {
            RunAwayFromPlayer = false;
            DestinationPoint.parent = null;
            _enemiesLeft = EnemyCount;
            _tickTimer = tickTime * (1 + Random.Range(-0.2f, 0.2f));
            _player = GameObject.FindGameObjectWithTag("Player");
            OnEnemyKilled?.Invoke(_enemiesLeft);
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
            
                _runAwayFromDirections.Clear();
                _runAwayFromDirections.Add(direction.x > 0 ? Vector2.right : direction.x < 0 ? Vector2.left : Extensions.GetRandomDirection(right: true, left: true, zero: true));
                _runAwayFromDirections.Add(direction.y > 0 ? Vector2.up : direction.y < 0 ? Vector2.down : Extensions.GetRandomDirection(up: true, down: true, zero: true));
            
                _availableDirections.Subtract(_runAwayFromDirections);
            }
        }
        
        private void FixedUpdate() {
            if (GameStateController.Instance.GetState() != GameState.Playing) return;
            transform.position = Vector3.MoveTowards(transform.position, DestinationPoint.position, moveSpeed * Time.deltaTime);
        }

        
        public void OnDeath() {
            _isDead = true;
            _enemiesLeft--;

            ScoreController.Instance.AddScore(killScore);
            OnEnemyKilled?.Invoke(_enemiesLeft);
            
            if (_enemiesLeft == 0) GameStateController.Instance.SetState(GameState.GameWon);
            if (_enemiesLeft == 2) RunAwayFromPlayer = true;

            print("OnDeath .. " + _enemiesLeft + " remains");
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                GameStateController.Instance.SetState(GameState.GameOver);
            }
        }

        public bool IsDead() => _isDead;
    }
}