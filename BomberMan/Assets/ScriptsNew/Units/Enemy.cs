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
                   // other.CompareTag("Enemy") ||
                   other.CompareTag("Player");
        }

        private void FixedUpdate() {
            var position = _rigidbody.position;
            /*_moveDirection = _randomDirection switch {
                1 => Vector2.up,
                2 => Vector2.right,
                3 => Vector2.down,
                4 => Vector2.left,
                _ => _moveDirection
            };*/

            // transform.Translate(_moveDirection * (moveSpeed * Time.deltaTime));
            
            _rigidbody.MovePosition(position + _moveDirection * (moveSpeed * Time.fixedDeltaTime));
            FindFreeDirection();
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            Debug.Log("Collided with " + collision.gameObject.name);

            if (collision.gameObject.CompareTag("Border") ||
                collision.gameObject.CompareTag("Pillar") ||
                collision.gameObject.CompareTag("Enemy") ||
                collision.gameObject.CompareTag("Destructible")) {
                FindFreeDirection();


                /*if (_allowRandomDirection) {
                    _randomDirection = Random.Range(1, 5);
                    _allowRandomDirection = false;
                    Invoke(nameof(AllowRandomDirection), randomDirectionCoolDown);
                }*/
            }
        }

        private void FindFreeDirection() {
            
            var position = transform.position;
             
            
            
            
            var hit = Physics2D.Raycast(position + (Vector3)_moveDirection * 0.5f, _moveDirection, raycastDistance);
            if (hit.collider != null && CollidedWithObstacle(hit)) {
                _moveDirection = -_moveDirection;
            }
            
         

            /*
            if (hitUp.collider == null && upAllowed) {
                _moveDirection = Vector2.up;
                upAllowed = false;
            }
            else if (hitDown.collider == null && downAllowed) {
                _moveDirection = Vector2.down;
                downAllowed = false;
            }
            else if (hitLeft.collider == null && leftAllowed) {
                _moveDirection = Vector2.left;
                leftAllowed = false;
            }
            else if (hitRight.collider == null && rightAllowed) {
                _moveDirection = Vector2.right;
                rightAllowed = false;
            }
            
            if (hitUp.collider != null && !upAllowed) upAllowed = true;
            if (hitDown.collider != null && !downAllowed) downAllowed = true;
            if (hitLeft.collider != null && !leftAllowed) leftAllowed = true;
            if (hitRight.collider != null && !rightAllowed) rightAllowed = true;*/
        }
    }
}