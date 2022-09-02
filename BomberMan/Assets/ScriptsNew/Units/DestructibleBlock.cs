
namespace BomberMan {
    public class DestructibleBlock : Unit {


        public void Init(GridManager gridManager, string name, int x, int y) {
            base.Init(gridManager, name, x, y);
            IsDestoyeable = true;
        }        
        
        
        public void Destroy() {
            Destroy(gameObject);
        }
        
    }
}