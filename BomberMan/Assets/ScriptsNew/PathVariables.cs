namespace BomberMan {
    public static class PathVariables {
        
        private const string PlayerPath = "Units/Player";
        private const string DestructibleBlockPath = "Units/DestructibleBlock";
        private const string BorderPath = "Units/Border";
        private const string PillarPath = "Units/Pillar";
        private const string EnemiesFolderPath = "Units/Enemies";
        

        public static string Player => PlayerPath;
        public static string DestructibleBlock => DestructibleBlockPath;
        public static string Border => BorderPath;
        public static string Pillar => PillarPath;
        public static string EnemiesFolder => EnemiesFolderPath;
    }
}