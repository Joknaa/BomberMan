using System;
using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;

namespace BomberMan {
    public abstract class Tile : MonoBehaviour {
        [SerializeField] protected float weight;
        [SerializeField] protected List<Sprite> sprites;

        protected int _x;
        protected int _y;
        protected GridManager _gridManager;
        
        private SpriteRenderer _spriteRenderer;
        private GameObject _highlight;
        private bool _isFree;

        protected virtual void Awake() {
            _highlight = transform.GetChild(0).gameObject;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void Init(GridManager gridManager, string name, int x, int y) {
            gameObject.name = $"Tile {x}, {y} : {name}";
            _x = x;
            _y = y;
            _gridManager = gridManager;
            _spriteRenderer.sprite = sprites.Random();
        }

        
        public void OnMouseEnter() => _highlight.SetActive(true);
        public void OnMouseExit() => _highlight.SetActive(false);
        
        public float GetWeight() => weight;
        public bool IsFree() => _isFree;
        public void SetFree(bool isFree) => _isFree = isFree;
        public int GetX() => _x;
        public int GetY() => _y;
    }
}