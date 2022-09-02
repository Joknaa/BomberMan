using System;
using System.Collections;
using UnityEngine;

namespace BomberMan {
    public class Bomb : Unit {
        private float timeToExplode = 1;
        private int bombRange;
        private Vector2 bombPosition;


        public void Init(GridManager gridManager, string name, int x, int y, float timeToExplode, int bombRange) {
            base.Init(gridManager, name, x, y);
            this.timeToExplode = timeToExplode;
            this.bombRange = bombRange;
            bombPosition = new Vector2(x, y);
        }
        
        private void Update() {
            timeToExplode -= Time.deltaTime;
            if (timeToExplode <= 0) {
                Explode();
            }
        }


        private void Explode() {
            var tileCoord = _gridManager.PositionToTileCoord(bombPosition);
            var tile = _gridManager.GetTile(tileCoord);
            print("Boom");

                Destroy(gameObject);
            
        }
    }
}