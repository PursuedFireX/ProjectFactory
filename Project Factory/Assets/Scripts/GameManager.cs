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
        public event EventHandler OnGameStateChange;
        public event EventHandler OnBuildStateChange;

        public float currentPower;
        public BuildState currentBuildState;


        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            CurrentState = GameState.Free;
            currentBuildState = BuildState.Build;
            CheckForBuildStateUpdate();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.G))
            {
                if (CurrentState == GameState.Free)
                    CurrentState = GameState.Build;
                else if (CurrentState == GameState.Build)
                    CurrentState = GameState.Free;

                CheckForGameStateUpdate();
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
    }
}
