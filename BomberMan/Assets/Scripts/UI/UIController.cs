using System;
using UnityEngine;

namespace BomberMan {
    public class UIController : MonoBehaviour {
        public GameObject InGameHUD;
        public GameObject GameOverPopUp;
        public GameObject GameWonPopUp;
        
        private void Start() {
            GameStateController.Instance.SetState(GameState.Playing);
            GameStateController.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newGameState) {
            print("OnGameStateChanged: " + newGameState);
            switch (newGameState) {
                case GameState.Playing:
                    DisplayUI(hud: true);
                    break;
                case GameState.Paused:
                case GameState.GameOver:
                    DisplayUI(gameOver: true, hud: true);
                    break;
                case GameState.GameWon:
                    DisplayUI(gameWon: true, hud: true);
                    break;
            }
        }

        private void DisplayUI(bool startingMenu = false, bool hud = false, bool gameOver = false, bool gameWon = false) {
            print("Displaying UI for: " + (startingMenu ? "Starting Menu" : gameOver ? "Game Over" : gameWon ? "Game Won" : "Unknown"));
            InGameHUD.SetActive(hud);
            GameOverPopUp.SetActive(gameOver);
            GameWonPopUp.SetActive(gameWon);
        }
            
        
        private void OnDestroy() {
            GameStateController.Instance.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}