using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PFX
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager I { get; private set; }

        public int toolBarIndex;
        public bool mouseOverUI;
        public ToolBarButton currentButton;

        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            TimeTickSystem.OnTick += TickHandler;
        }

        private void TickHandler(object sender, TimeTickSystem.OnTickEventArgs e)
        {

        }

        public void UpdateToolbarIndex(int index, ToolBarButton button)
        {
            if (currentButton != null)
                currentButton.Deselect();

            currentButton = button;
            toolBarIndex = index;
            CheckIndex(index);
        }

        private void CheckIndex(int index)
        {
            switch(index)
            {
                case 0:
                    GameManager.I.UpdateGameState(GameState.Free); //Free state
                    break;

                case 1:
                    GameManager.I.UpdateGameState(GameState.Build); //Change to build state
                    GameManager.I.UpdateBuildState(BuildState.Build);
                    break;

                case 2:
                    GameManager.I.UpdateGameState(GameState.Build); //Component state
                    GameManager.I.UpdateBuildState(BuildState.Component);
                    break;

                case 3:
                    GameManager.I.UpdateGameState(GameState.Build); //Demolish state
                    GameManager.I.UpdateBuildState(BuildState.Destroy);
                    break;

                case 4:

                    break;
            }

        }


    }
}
