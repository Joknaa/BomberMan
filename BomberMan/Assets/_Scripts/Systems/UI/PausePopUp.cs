using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BomberMan {
    public class PausePopUp : MonoBehaviour {
        public Button unpauseButton;
        public Button restartButton;


        private void Start() {
            unpauseButton.onClick.AddListener(OnUnpauseButtonPressed);
            restartButton.onClick.AddListener(OnRestartButtonPressed);
        }

        private void OnRestartButtonPressed() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnUnpauseButtonPressed() {
            GameStateController.Instance.SetState(GameState.Playing);
        }
    }
}