using System;
using UnityEngine;

namespace PFX
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager I { get; private set; }

        public GameState CurrentState;
        public GameState prevState;
        public event EventHandler OnGameStateChange;
        public event EventHandler OnBuildStateChange;

        public float currentPower;
        public BuildState currentBuildState;
        public Transform UITransform;

        public Transform vCam;


        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            TimeTickSystem.Create();

            CurrentState = GameState.Free;
            currentBuildState = BuildState.Build;
            prevState = CurrentState;
            CheckForBuildStateUpdate();
            CheckForGameStateUpdate();

        }

        public void UpdateGameState(GameState newState)
        {
            prevState = CurrentState;
            CurrentState = newState;
            string text = "Game State = " + newState.ToString();
            Console.SendDebug(text);
            CheckForGameStateUpdate();
        }

        public void UpdateBuildState(BuildState newState)
        {
            currentBuildState = newState;
            string text = "Build State = " + newState.ToString();
            Console.SendDebug(text);
            CheckForBuildStateUpdate();
        }

        public void CheckForGameStateUpdate()
        {
            OnGameStateChange?.Invoke(this, EventArgs.Empty);
        }

        public void CheckForBuildStateUpdate()
        {
            OnBuildStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum BuildState
    {
        Build,
        Destroy,
        Component,
    }

    public enum GameState
    {
        Free,
        Build,
        Typing,
        Paused,
    }
}
