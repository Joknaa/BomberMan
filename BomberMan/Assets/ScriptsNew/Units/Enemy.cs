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
        public int moveSpeed;
        public float raycastDistance = 1f;

        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        
        bool upAllowed = true;
        bool rightAllowed = true;
        bool downAllowed = true;
        bool leftAllowed = true;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody2D>();
            
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

            _moveDirection = possibleDirections.Random(Vector3.zero);
            
        }

        private void Update() { }

        private bool CollidedWithObstacle(RaycastHit2D hit) {
            GameObject other = hit.collider.gameObject;
            return other.CompareTag("Border") ||
                   other.CompareTag("Pillar") ||
                   other.CompareTag("Destructible") ||
                   other.CompareTag("Bomb") ||
                   other.CompareTag("Player");
        }

        private void FixedUpdate() {
            var position = _rigidbody.position;
            _rigidbody.MovePosition(position + _moveDirection * (moveSpeed * Time.fixedDeltaTime));
            FindFreeDirection();
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            Debug.Log("Collided with " + collision.gameObject.name);

            if (collision.gameObject.CompareTag("Border") ||
                collision.gameObject.CompareTag("Pillar") ||
                collision.gameObject.CompareTag("Destructible")) {
                FindFreeDirection();
            }
            if (collision.gameObject.CompareTag("Player")) {
                _gridManager.RestartScene();
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Explosion")) {
                Destroy(gameObject);
            }
        }

        private void FindFreeDirection() {
            
            var position = transform.position;
             
            var hit = Physics2D.Raycast(position + (Vector3)_moveDirection * 0.5f, _moveDirection, raycastDistance);
            if (hit.collider != null && CollidedWithObstacle(hit)) {
                _moveDirection = -_moveDirection;
            }
        }
    }
}