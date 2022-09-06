using System;
using UnityEngine;

namespace BomberMan {
    public enum UIState {
        MainMenu,
        InGameHUD,
        PauseManu,
        GameOverPopUp,
        GameWonPopUp,
    }
    public class UIStateController : MonoBehaviour {
        public Action OnUIStateChanged;
        private static GameStateController _instance;
        private GameState _currentState;
    

        public void SetState(GameState state) {
            _currentState = state;
            OnUIStateChanged?.Invoke();
        }

        public GameState GetCurrentGameState() => _currentState;
    }
}