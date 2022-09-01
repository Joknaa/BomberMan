using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BomberMan {
    public class GridManager : MonoBehaviour {
        [Header("Grid Dimensions")] 
        public int height = 10;
        public int width = 15;

        [Header("Destructible Blocks")] 
        public int destructibleCount = 30;
        [Range(0.0f, 1.0f)] public float destructibleSpawnChance = 0.4f;
        public int playerDestructibleSafeZoneDiameter = 2;

        [Header("Enemies")] 
        public int enemyCount = 5;
        [Range(0.0f, 1.0f)] public float enemySpawnChance = 0.4f;
        public int playerEnemySafeZoneDiameter = 2;

        private Camera _mainCamera;
        private Tile[,] _gridTiles;
        private int _destructiblesInstantiated = 0;
        private int _enemiesInstantiated = 0;
        private List<Tile> _tiles;
        private List<Unit> _enemies;

        private void Start() {
            _mainCamera = Camera.main;
            _gridTiles = new Tile[width, height];

            GenerateGrid();
        }

        private void GenerateGrid() {
            _tiles = Resources.LoadAll<Tile>("Tiles").ToList();
            _enemies = Resources.LoadAll<Unit>(PathVariables.EnemiesFolder).ToList();

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    GenerateTiles(y, x);
                    GenerateBorders(y, x);
                    GeneratePillars(y, x);
                    GenerateDestructibleBlocks(y, x);
                    GenerateEnemies(y, x);
                }
            }
            GeneratePlayer();

            _mainCamera.transform.position = new Vector3(width * 0.5f - 0.5f, height * 0.5f - 0.5f, -10);
        }
        

        private void GenerateTiles(int y, int x) {
            var randomTile = GetRandomTileByWeight(_tiles);
            GameObject tileInstance = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity, transform);
            Tile tile = tileInstance.GetComponent<Tile>();
            tile.Init(this, randomTile.name, x, y);
        }
        private void GenerateBorders(int y, int x) {
            bool isBorder = y == 0 || y == height - 1 || x == 0 || x == width - 1;
            if (!isBorder) return;
            SetupUnite(Resources.Load<Tile>(PathVariables.Border).gameObject, x, y);
        }
        private void GeneratePillars(int y, int x) {
            bool isBorder = y == 0 || y == height - 1 || x == 0 || x == width - 1;
            if (isBorder) return;
            if (y % 2 == 1 || x % 2 == 1) return;
            SetupUnite(Resources.Load<Tile>(PathVariables.Pillar).gameObject, x, y);
        }
        private void GenerateDestructibleBlocks(int y, int x) {
            if (_destructiblesInstantiated >= destructibleCount) return; // if all destructible blocks are instantiated, do not generate any more
            if (IsPlayerSaveZone(playerDestructibleSafeZoneDiameter, y, x))
                return; // To leave some space for the player to dodge the first bomb, no obstacles are allowed in his spawn point
            if (_gridTiles[x, y] != null && !_gridTiles[x, y].IsFree()) return; // if tile is not free, do not generate destructible block
            if (Random.Range(0f, 1f) > destructibleSpawnChance) return; // To generate destructible blocks 50% of the time

            SetupUnite(Resources.Load<Tile>(PathVariables.DestructibleBlock).gameObject, x, y);
            _destructiblesInstantiated++;
        }
        private void GenerateEnemies(int y, int x) {
            if (_enemiesInstantiated >= enemyCount) return;
            if (IsPlayerSaveZone(playerEnemySafeZoneDiameter, y, x)) return;
            if (_gridTiles[x, y] != null && !_gridTiles[x, y].IsFree()) return;
            if (Random.Range(0f, 1f) > enemySpawnChance) return;

            SetupUnite(_enemies[_enemiesInstantiated].gameObject, x, y);
            _enemiesInstantiated++;
        }
        private void GeneratePlayer() {
            SetupUnite(Resources.Load<Tile>(PathVariables.Player).gameObject, 1, height - 2);
        }

        private void SetupUnite(GameObject tileGameObject, int x, int y) {
            GameObject unit = Instantiate(tileGameObject, new Vector3(x, y, 0), Quaternion.identity, transform);
            Tile tile = unit.GetComponent<Tile>();
            tile.Init(this, tileGameObject.name, x, y);
            tile.SetFree(false);
            _gridTiles[x, y] = tile;
        }

        private bool IsPlayerSaveZone(int saveZoneDiameter, int y, int x) {
            int playerSpawnX = 1;
            int playerSpawnY = height - 2;

            int playerSafeZoneMaxX = playerSpawnX + saveZoneDiameter;
            int playerSafeZoneMaxY = playerSpawnY - saveZoneDiameter;

            return x >= playerSpawnX && x <= playerSafeZoneMaxX && y <= playerSpawnY && y >= playerSafeZoneMaxY;
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

        // temporary
        public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void MoveUnit(Unit unitToMove, Vector2 moveDirection, int moveSpeed) {
        }
    }
}