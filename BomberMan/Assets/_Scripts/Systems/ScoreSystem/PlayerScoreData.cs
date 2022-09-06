namespace ScoreSystem {
    
    [System.Serializable]
    public class PlayerScoreData {
        public int score;
        public string playerName;

        public PlayerScoreData(int score, string playerName) {
            this.score = score;
            this.playerName = playerName;
        }
    }
}