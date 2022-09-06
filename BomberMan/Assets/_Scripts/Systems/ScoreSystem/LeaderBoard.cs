using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ScoreSystem {
    public class LeaderBoard : MonoBehaviour {
        public GameObject container;
        public PlayerScore othersEntry;
        public PlayerScore playerEntry;

        private PlayerScore _playerScore;
        private LeaderBoardData _leaderBoardData;
        private PlayerScoreData _newPlayerScoreData;
        private bool _isHighScore;

        public void Display(LeaderBoardData leaderBoardData, PlayerScoreData newPlayerScoreData) {
            _leaderBoardData = leaderBoardData;
            _newPlayerScoreData = newPlayerScoreData;

            SetupLeaderBoard();
            
            foreach (PlayerScoreData playerScoreData in _leaderBoardData.playerScoresList) {
                var rank = leaderBoardData.playerScoresList.IndexOf(playerScoreData) + 1;
                
                if (playerScoreData == newPlayerScoreData) {
                    PlayerScore playerScore = _isHighScore ? Instantiate(playerEntry, container.transform) : Instantiate(othersEntry, container.transform);
                    playerScore.SetRowScore(rank, newPlayerScoreData, isHighScore: _isHighScore);
                    _playerScore = playerScore;
                }
                else {
                    PlayerScore playerScore = Instantiate(othersEntry, container.transform);
                    playerScore.SetRowScore(rank, playerScoreData.playerName, playerScoreData.score);
                }
            }
            
            if (_leaderBoardData.playerScoresList.Count == 0) {
                PlayerScore playerScore = Instantiate(playerEntry, container.transform);
                playerScore.SetRowScore(1, newPlayerScoreData, isHighScore: true);
                _playerScore = playerScore;
            }

            leaderBoardData.Save();
        }

        private void SetupLeaderBoard() {
            _isHighScore = GetIsHighScore();
            SortScores();


            bool GetIsHighScore() {
                foreach (PlayerScoreData playerScore in _leaderBoardData.playerScoresList) {
                    if (_newPlayerScoreData.score <= playerScore.score) continue;
                    _isHighScore = true;
                    break;
                }

                if (_leaderBoardData.playerScoresList.Count == 0) _isHighScore = true;

                return _isHighScore;
            }

            void SortScores() {
                if (_isHighScore) _leaderBoardData.playerScoresList.Add(_newPlayerScoreData);

                _leaderBoardData.playerScoresList.Sort((x, y) => (x.score.CompareTo(y.score)));
                _leaderBoardData.playerScoresList.Reverse();

                if (_isHighScore && _leaderBoardData.playerScoresList.Count >= 4) _leaderBoardData.playerScoresList.RemoveAt(_leaderBoardData.playerScoresList.Count - 1);
            }
        }
        
        public void NextButtonClick() {
            if (_playerScore != null) _playerScore.UpdatePlayerScoreName();
            _leaderBoardData.Save();
        }

    }
}