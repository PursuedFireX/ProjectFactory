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
        public int contentIndex;

        public bool mouseOverUI;

        public ToolBarButton currentButton;
        public ToolBarMenuButton currentTBMenuButton;

        public GameObject toolbarMenu; //Top buttons
        public GameObject toolbarContent; //Content toolbar
        public GameObject[] contentHolders;

        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            TimeTickSystem.OnTick += TickHandler;
            CheckToolbarIndex();
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
            CheckToolbarIndex();
        }


        public void UpdateContentHolder(int index)
        {
            contentIndex = index;
            CheckContentIndex();
        }

        public void UpdateContentHolder(int index, ToolBarMenuButton button)
        {
            if (currentTBMenuButton != null)
                currentTBMenuButton.Deselect();

            currentTBMenuButton = button;

            contentIndex = index;
            CheckContentIndex();
        }

        private void CheckContentIndex()
        {
            for (int i = 0; i < contentHolders.Length; i++)
            {
                contentHolders[contentIndex].SetActive(true);
                if(i != contentIndex)
                {
                    contentHolders[i].SetActive(false);
                }

            }
        }


        private void CheckToolbarIndex()
        {
            switch(toolBarIndex)
            {
                case 0:
                    GameManager.I.UpdateGameState(GameState.Free); //Free state
                    toolbarContent.SetActive(false);
                    toolbarMenu.SetActive(false);

                    break;

                case 1:
                    GameManager.I.UpdateGameState(GameState.Build); //Change to build state
                    GameManager.I.UpdateBuildState(BuildState.Build);
                    toolbarContent.SetActive(true);
                    toolbarMenu.SetActive(true);
                    break;

                case 2:
                    GameManager.I.UpdateGameState(GameState.Build); //Component state
                    GameManager.I.UpdateBuildState(BuildState.Component);
                    toolbarContent.SetActive(true);
                    toolbarMenu.SetActive(false);
                    UpdateContentHolder(6);
                    break;

                case 3:
                    GameManager.I.UpdateGameState(GameState.Build); //Demolish state
                    GameManager.I.UpdateBuildState(BuildState.Destroy);
                    toolbarContent.SetActive(false);
                    toolbarMenu.SetActive(false);
                    break;

                case 4:

                    break;
            }

        }


    }
}
