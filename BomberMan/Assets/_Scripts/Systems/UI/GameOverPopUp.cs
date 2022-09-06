using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace BomberMan {
    public class GameOverPopUp : MonoBehaviour {
        public Button tryAgainButton;


        private void Start() {
            tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
        }
        
        private void OnTryAgainButtonClicked() {
            ScoreController.Instance.ResetScore();
            GridManager.RestartScene();
        }
    }
}