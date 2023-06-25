using System;
using System.Collections;
using System.Collections.Generic;
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


        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
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
                }
            }
        }

        public void UpdateGameState(GameState newState)
        {
            prevState = CurrentState;
            CurrentState = newState;
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
