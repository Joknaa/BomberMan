
using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;

namespace BomberMan {
    public class GridManager : MonoBehaviour {
        private int height = 10;
        private int width = 15;

        void Start() {
            GenerateGrid();
            GenerateUnits();
        }

        private void GenerateGrid() {
            List<Tile> tiles = Resources.LoadAll<Tile>("Tiles").ToList();
            
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {

                    var randomTile = GetRandomTileByWeight(tiles); ; 
                    GameObject tile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity, transform);
                    tile.GetComponent<Tile>().Init(x, y);
                }
            }

            Camera.main.transform.position = new Vector3(width * 0.5f - 0.5f, height * 0.5f - 0.5f, -10);
        }
        
        private void GenerateUnits() {
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1) {
                        GameObject unit = Instantiate(Resources.LoadAll<Tile>("Units").ToList().Random().gameObject);
                        unit.transform.position = new Vector3(j, i, 0);
                        unit.transform.parent = transform;
                        unit.GetComponent<Tile>().Init(i, j);

                    }
                }
            }
        }

        private GameObject GetRandomTileByWeight(List<Tile> tiles) {
            float totalWeight = 0;
            foreach (var tile in tiles) totalWeight += tile.GetWeight();
            
            float random = Random.Range(0f, totalWeight);
            
            foreach (var tile in tiles) {
                random -= tile.GetWeight();
                if (random <= 0) return tile.gameObject;
            }
            
            return tiles[0].gameObject;
        }
    }
}