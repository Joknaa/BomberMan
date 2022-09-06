using System;
using ScoreSystem;
using UnityEngine;

namespace BomberMan {
    public class UIController : MonoBehaviour {
        public GameObject inGameHUD;
        public GameObject pausePopUp;
        public GameObject gameOverPopUp;
        public GameObject gameWonPopUp;
        
        private LeaderBoardController _leaderBoardController;
        
        private void Start() {
            _leaderBoardController = GameObject.FindGameObjectWithTag("HighScoreController").GetComponent<LeaderBoardController>();
            GameStateController.Instance.OnGameStateChanged += OnGameStateChanged;
            GameStateController.Instance.SetState(GameState.Playing);
        }

        private void OnGameStateChanged(GameState newGameState) {
            switch (newGameState) {
                case GameState.Playing:
                    DisplayUI(hud: true);
                    break;
                case GameState.Paused:
                    DisplayUI(hud: true, paused: true);
                    break;
                case GameState.GameOver:
                    DisplayUI(gameOver: true, hud: true);
                    _leaderBoardController.DisplayLeaderBoard(ScoreController.Instance.GetScore());
                    break;
                case GameState.GameWon:
                    DisplayUI(gameWon: true, hud: true);
                     _leaderBoardController.DisplayLeaderBoard(ScoreController.Instance.GetScore());
                    break;
            }
        }

        private void DisplayUI(bool startingMenu = false, bool hud = false, bool paused = false, bool gameOver = false, bool gameWon = false) {
            inGameHUD.SetActive(hud);
            pausePopUp.SetActive(paused);
            gameOverPopUp.SetActive(gameOver);
            gameWonPopUp.SetActive(gameWon);
        }
            
        
        private void OnDestroy() {
            GameStateController.Instance.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}