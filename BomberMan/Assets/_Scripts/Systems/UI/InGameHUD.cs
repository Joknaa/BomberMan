using System;
using TMPro;
using UnityEngine;

namespace BomberMan {
    public class InGameHUD : MonoBehaviour {
        public TMP_Text score;
        public TMP_Text enemiesLeft;
        public TMP_Text elapsedTime;
        

        private void Start() {
            ScoreController.Instance.OnScoreChanged += OnScoreChanged;
            Enemy.OnEnemyKilled += OnEnemyKilled;
            
            score.text = "0000";
            enemiesLeft.text = "" + Enemy.EnemyCount;
            elapsedTime.text = "00";
        }

        private void FixedUpdate() {
            if (GameStateController.Instance.GetState() != GameState.Playing) return;
            elapsedTime.text = "Time: " + (Time.timeSinceLevelLoad).ToString("00");
        }

        private void OnScoreChanged(int newScore) => score.text = "" + newScore;
        private void OnEnemyKilled(int enemiesLeft) => this.enemiesLeft.text = "Left: 0" + enemiesLeft;
        
        
        private void OnDestroy() {
            ScoreController.Instance.OnScoreChanged -= OnScoreChanged;
            Enemy.OnEnemyKilled -= OnEnemyKilled;
        }
    }
}