using System;
using System.Collections;
using System.Collections.Generic;
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
            var bombCoord = _gridManager.GetCoordFromPosition(bombPosition);
            var originX = bombCoord.x;
            var originY = bombCoord.y;

            // Explode in all directions
            List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            for (int i = 0; i < 4; i++) {
                var direction = directions[i];
                var currentX = originX;
                var currentY = originY;
                for (int j = 0; j < bombRange; j++) {
                    currentX += direction.x;
                    currentY += direction.y;
                    if (currentX < 0 || currentX >= _gridManager.width || currentY < 0 || currentY >= _gridManager.height) break;
                    var tile = _gridManager.GetTileFromCoord(currentX, currentY);

                    if (tile == null) {
                        Destroy(Instantiate(Resources.Load<GameObject>(PathVariables.Explosion01Effect), new Vector2(currentX, currentY), Quaternion.identity), 0.2f);
                        continue;
                    }


                    if (tile.CompareTag("Destructible")) {
                        Destroy(Instantiate(Resources.Load<GameObject>(PathVariables.Explosion01Effect), tile.transform.position, Quaternion.identity), 0.2f);
                        Destroy(tile.gameObject);
                    }
                    else if (tile.CompareTag("Player")) {
                        Destroy(Instantiate(Resources.Load<GameObject>(PathVariables.Explosion01Effect), tile.transform.position, Quaternion.identity), 0.2f);
                        _gridManager.RestartScene();
                        break;
                    }
                    else if (tile.CompareTag("Enemy")) {
                        Destroy(Instantiate(Resources.Load<GameObject>(PathVariables.Explosion01Effect), tile.transform.position, Quaternion.identity), 0.2f);
                        Destroy(tile.gameObject);
                        break;
                    }
                }
            }

            Destroy(gameObject);
        }

        private void DestroyTile(Tile tile) {
            if (tile.CompareTag("Destructible")) {
                // Destroy(Instantiate(Resources.Load<GameObject>(PathVariables.ExplosionEffect), tile.transform.position, Quaternion.identity), 0.2f);
                Destroy(tile.gameObject);
            }
        }
        
        

        public bool IsDestructible { get; set; }
    }
}