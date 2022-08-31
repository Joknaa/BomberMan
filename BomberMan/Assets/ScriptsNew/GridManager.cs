
using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;

namespace BomberMan {
    public class GridManager : MonoBehaviour {
        public int height = 10;
        public int width = 15;
        private Camera _mainCamera;
        private Tile[,] _gridTiles;

        void Start() {
            _mainCamera = Camera.main;
            _gridTiles = new Tile[width, height];
            GenerateGrid();
            GenerateUnits();
        }

        private void GenerateGrid() {
            List<Tile> tiles = Resources.LoadAll<Tile>("Tiles").ToList();
            
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {

                    var randomTile = GetRandomTileByWeight(tiles); ; 
                    GameObject tileInstance = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity, transform);
                    // tileInstance.GetComponent<Tile>().Init(x, y);
                    Tile tile = tileInstance.GetComponent<Tile>();
                    tile.Init(x, y);
                }
            }
            _mainCamera.transform.position = new Vector3(width * 0.5f - 0.5f, height * 0.5f - 0.5f, -10);
        }
        
        private void GenerateUnits() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    GenerateBorders(y, x);
                    GeneratePillars(y, x);
                }
            }
         

            void GenerateBorders(int y, int x) {
                bool isBorder = y == 0 || y == height - 1 || x == 0 || x == width - 1;
                if (!isBorder) return;
                SetupTile(PathVariables.Border, x, y);
            }
            
            void GeneratePillars(int y, int x) {
                bool isBorder = y == 0 || y == height - 1 || x == 0 || x == width - 1;
                if (isBorder) return;
                if (y % 2 == 1 || x % 2 == 1) return;
                SetupTile(PathVariables.Pillar, x, y);
            }

            void SetupTile(string tilePath, int x, int y) {
                GameObject unit = Instantiate(Resources.LoadAll<Tile>(tilePath).ToList().Random().gameObject, new Vector3(x, y, 0), Quaternion.identity, transform);
                Tile tile = unit.GetComponent<Tile>();
                tile.Init(x, y);
                tile.SetFree(false);
                _gridTiles[x, y] = tile;
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