using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ScoreSystem {
    public class PlayerScore : MonoBehaviour {
        public TextMeshProUGUI playerRank;
        public TextMeshProUGUI playerName;
        public TextMeshProUGUI playerScore;
        public TMP_InputField playerNameInput;
        private PlayerScoreData _PlayerScoreData;

        public void SetRowScore(int rank, string playerName, float score, bool newHighScore = false) {
            this.playerRank.text = "" + rank;
            this.playerName.text = playerName;
            playerScore.text = "" + score;

            if (!newHighScore) return;
            playerNameInput.gameObject.SetActive(true);
            this.playerName.gameObject.SetActive(false);
        }

        public void SetRowScore(int rank, PlayerScoreData playerScoreData, bool isHighScore = false) {
            _PlayerScoreData = playerScoreData;
            playerRank.text = "" + rank;
            playerName.text = playerScoreData.playerName;
            playerScore.text = playerScoreData.score.ToString();

            if (!isHighScore) return;

            if (LeaderBoardController.PlayerName == LeaderBoardController.DefaultPlayerName) {
                playerName.gameObject.SetActive(false);
                playerNameInput.gameObject.SetActive(true);
                playerNameInput.placeholder.GetComponent<TextMeshProUGUI>().text = "You";
                playerNameInput.Select();
            }
            else {
                if (LeaderBoardController.SupportNameChange) {
                    playerNameInput.gameObject.SetActive(true);
                    playerNameInput.placeholder.GetComponent<TextMeshProUGUI>().text = LeaderBoardController.PlayerName;
                }
            }
        }

        public void UpdatePlayerScoreName() {
            var isEmptyOrNull = string.IsNullOrEmpty(playerNameInput.text);

            if (!isEmptyOrNull) {
                _PlayerScoreData.playerName = playerNameInput.text;
                LeaderBoardController.PlayerName = playerNameInput.text;
            }
            else
                _PlayerScoreData.playerName = LeaderBoardController.PlayerName;
        }

        public string GetPlayerNameInput() => playerNameInput.text;
    }
}