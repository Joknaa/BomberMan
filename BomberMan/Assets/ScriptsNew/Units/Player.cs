
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BomberMan {
    public class Player : Unit {
        public int moveSpeed;
        public int bombCount;
        public int bombRange;
        public int bombTimer;

    
        private Vector2 _moveDirection;
        private Vector2 _horizontalDirection;
        private Vector2 _verticalDirection;
        private Rigidbody2D _rigidbody;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");
            _horizontalDirection = horizontalInput > 0 ? Vector2.right : horizontalInput < 0 ? Vector2.left : Vector2.zero;
            _verticalDirection = verticalInput > 0 ? Vector2.up : verticalInput < 0 ? Vector2.down : Vector2.zero;
            // _moveDirection = _horizontalDirection == Vector2.zero ? _verticalDirection : _horizontalDirection;
            _moveDirection = _horizontalDirection + _verticalDirection;
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                PlaceBomb();
            }
        }

        private void FixedUpdate() {
            Move();
        }

        private void Move() {
            // _gridManager.MoveUnit(this, _moveDirection, moveSpeed);
            // transform.Translate(_moveDirection * (moveSpeed * Time.deltaTime));
            _rigidbody.velocity = _moveDirection * (moveSpeed * Time.fixedDeltaTime * 100) ;
        }

        private void PlaceBomb() {
            print("Bomb Placed");
            _gridManager.PlaceBomb(transform.position, bombRange, bombTimer);
        }
    }
}