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
            bool MetAnObstacle = false;
            
            PlayExplosionEffect(originX, originY);
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
                        PlayExplosionEffect(currentX, currentY);
                        continue;
                    }

                    if (tile.CompareTag("Destructible")) {
                        PlayExplosionEffect(currentX, currentY); 
                        ((DestructibleBlock) tile).Destroy();    
                        break;
                    }

                    if (tile.CompareTag("Border") || tile.CompareTag("Pillar")) {
                        break;
                    }
                }
            }
            _gridManager.Player.bombCount++;
            Destroy(gameObject);
        }

        private void PlayExplosionEffect(int x, int y) {
            Destroy(Instantiate(Resources.Load<GameObject>(PathVariables.Explosion01Effect), new Vector2(x, y), Quaternion.identity), 0.2f);
        }
    }
}