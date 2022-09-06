using System;
using UnityEngine;
using UnityEngine.UI;

namespace BomberMan {
    public class GameWonPopUp : MonoBehaviour {
        public Button nextLevelButton;


        private void Start() {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }
        
        private void OnNextLevelButtonClicked() {
            ScoreController.Instance.ResetScore();
            GridManager.RestartScene();
        }
    }
}