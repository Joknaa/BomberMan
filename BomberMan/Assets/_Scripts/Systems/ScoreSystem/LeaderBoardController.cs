using UnityEngine;

namespace ScoreSystem {
    public class LeaderBoardController : MonoBehaviour {
        public LeaderBoard leaderBoardPrefab;
        public RectTransform leaderBoardParent;
        public static readonly bool SupportNameChange = true;
        public static string PlayerName {
            get => PlayerPrefs.GetString("PlayerName", DefaultPlayerName);
            set => PlayerPrefs.SetString("PlayerName", value);
        }

        [HideInInspector] public const string DefaultPlayerName = "You";
        private LeaderBoardData _leaderBoardData;

        
        private void Start() {
            _leaderBoardData = LeaderBoardData.Load();
        }

        public void DisplayLeaderBoard(int playerScore) {
            LeaderBoard leaderBoardGameObject = Instantiate(leaderBoardPrefab, leaderBoardParent);
            leaderBoardGameObject.Display(_leaderBoardData, new PlayerScoreData(playerScore, PlayerName));
        }
    }
}