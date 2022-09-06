using System;
using UnityEngine;

namespace BomberMan {
    public class ScoreController {
        public static ScoreController Instance => _instance ??= new ScoreController();
        private static ScoreController _instance;
        public event Action<int> OnScoreChanged; 
        private int _score = 0;
        
        private ScoreController() {
            _score = 0;
        }
        
        public void AddScore(int addedScore) {
            _score += addedScore;
            OnScoreChanged?.Invoke(_score);
        }

        public int GetScore() => _score;
        
        public void ResetScore() {
            _score = 0;
            OnScoreChanged?.Invoke(_score);
        }
    }
}