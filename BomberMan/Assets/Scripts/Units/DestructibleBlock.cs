
using UnityEngine.SocialPlatforms.Impl;

namespace BomberMan {
    public class DestructibleBlock : Unit {
        public int destructionScore = 10;

        public void Init(GridManager gridManager, string name, int x, int y) {
            base.Init(gridManager, name, x, y);
            IsDestoyeable = true;
        }        
        
        
        public void Destroy() {
            ScoreController.Instance.AddScore(destructionScore);
            Destroy(gameObject);
        }
        
    }
}