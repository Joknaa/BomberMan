using System;
using UnityEngine;

namespace BomberMan {
    public enum GameState {
        Playing,
        Paused,
        GameOver,
        GameWon
    }
    public class GameStateController {
        public static GameStateController Instance => _instance ??= new GameStateController();
        public event Action<GameState> OnGameStateChanged;
        private static GameStateController _instance;
        private GameState _currentState;
    

        public void SetState(GameState state) {
            _currentState = state;
            OnGameStateChanged?.Invoke(_currentState);
        }
        
        public GameState GetState() {
            return _currentState;
        }
    }
}