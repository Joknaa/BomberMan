using System;
using TMPro;
using UnityEngine;

namespace BomberMan {
    public class InGameHUD : MonoBehaviour {
        public TMP_Text score;

        private void Start() {
            ScoreController.Instance.OnScoreChanged += OnScoreChanged;
            score.text = "0000";

        }

        private void OnScoreChanged(int newScore) {
            score.text = "" + newScore;
        }
        
        private void OnDestroy() {
            ScoreController.Instance.OnScoreChanged -= OnScoreChanged;
        }
    }
}