namespace BomberMan {
    public static class PathVariables {
        
        private const string PlayerPath = "Units/Player";
        private const string DestructibleBlockPath = "Units/DestructibleBlock";
        private const string BorderPath = "Units/Border";
        private const string PillarPath = "Units/Pillar";
        private const string EnemiesFolderPath = "Units/Enemies";
        private const string BombPath = "Units/Bomb";
        
        
        private const string Explosion01EffectPath = "Effects/Explosion01";


        public static string Player => PlayerPath;
        public static string DestructibleBlock => DestructibleBlockPath;
        public static string Border => BorderPath;
        public static string Pillar => PillarPath;
        public static string EnemiesFolder => EnemiesFolderPath;
        public static string Bomb => BombPath;
        
        
        
        public static string Explosion01Effect => Explosion01EffectPath;
    }
}