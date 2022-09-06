using System.Collections.Generic;
using UnityEngine;

namespace ScoreSystem {
    
    [System.Serializable]
    public class LeaderBoardData {
        public List<PlayerScoreData> playerScoresList;
        
        private static string _leaderBoardName = "LeaderBoard";
        private static List<PlayerScoreData> _defaultPlayerScoreList = new List<PlayerScoreData>() {
        };

        
        public LeaderBoardData(List<PlayerScoreData> playerScoresList) => this.playerScoresList = playerScoresList;
        public void Save() => PlayerPrefs.SetString(_leaderBoardName, JsonUtility.ToJson(this));
        

        public static LeaderBoardData Load() {
            var defaultLeaderBoard = new LeaderBoardData(_defaultPlayerScoreList);
            var leaderBoardJson = PlayerPrefs.GetString(_leaderBoardName, JsonUtility.ToJson(defaultLeaderBoard));
            var leaderBoardData = JsonUtility.FromJson<LeaderBoardData>(leaderBoardJson);
            return leaderBoardData;
        }
    }
}