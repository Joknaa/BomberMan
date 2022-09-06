using System;
using UnityEngine;

namespace BomberMan {
    public class ScoreController {
        public static ScoreController Instance => _instance ??= new ScoreController();
        private static ScoreController _instance;
        public event Action<int> OnScoreChanged; 

        public ScoreController() {
            GameStateController.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState currentGameState) {
            if (currentGameState == GameState.GameOver || currentGameState == GameState.GameWon) {
                
            }
        }

        private int Score {
            get => PlayerPrefs.GetInt("Score", 0);
            set => PlayerPrefs.SetInt("Score", value);
        }

        public void AddScore(int addedScore) {
            Score += addedScore;
            OnScoreChanged?.Invoke(Score);
        }

        public void ResetScore() => Score = 0;
        public int GetScore() => Score;
        
        
    }
}