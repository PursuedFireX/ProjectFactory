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

        private void Update()
        {
            if (CurrentState != GameState.Typing)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (prevState == CurrentState)
                        UpdateGameState(GameState.Build);
                    else
                        UpdateGameState(prevState);
                }

                //Toggle Build State
                if (Input.GetKeyDown(KeyCode.B))
                {
                    if (currentBuildState == BuildState.Build)
                        currentBuildState = BuildState.Component;
                    else if (currentBuildState == BuildState.Component)
                        currentBuildState = BuildState.Destroy;
                    else if (currentBuildState == BuildState.Destroy)
                        currentBuildState = BuildState.Build;

                    CheckForBuildStateUpdate();
                    string text = "Build State = " + currentBuildState.ToString();
                    Console.SendDebug(text);
                }
            }
        }



        public void UpdateGameState(GameState newState)
        {
            prevState = CurrentState;
            CurrentState = newState;
            string text = "Game State = " + newState.ToString();
            Console.SendDebug(text);
            CheckForGameStateUpdate();
        }


        private void CheckForGameStateUpdate()
        {
            OnGameStateChange?.Invoke(this, EventArgs.Empty);
        }

        private void CheckForBuildStateUpdate()
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
