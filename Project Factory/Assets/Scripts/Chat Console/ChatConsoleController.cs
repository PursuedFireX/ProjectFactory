using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace PFX
{
    public class ChatConsoleController : MonoBehaviour
    {
        public static ChatConsoleController I { get; private set; }
        private Commands commands;

        private bool showConsole;
        

        [Header("Chat Settings")]
        [SerializeField] private float chatFadeTime;
        [SerializeField] private float chatFadeSpd;
        [SerializeField] private Vector2 chatPopPos;
        [SerializeField] private int maxChatMessages = 25;

        [Header("Text Settings")]
        public Color textColor;
        public Color commandColor;
        public Color errorColor;

        [Header("Misc")]
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private GameObject chatConsole;
        [SerializeField] private GameObject chatPanel;

        [HideInInspector] public GameObject currentChatPopup;

        private string input;
        public List<ChatMessage> chatLog;

        private void Awake()
        {
            I = this;
            chatLog = new List<ChatMessage>();
        }

        private void Start()
        {
            commands = Commands.I;
        }

        private void Update()
        {

            if(Input.GetKeyDown(KeyCode.V))
            {
                Popup.Create(chatPopPos, "Boop", chatFadeTime, chatFadeSpd, errorColor, false, true);
            }


            input = inputField.text;
            if (InputManager.I.ToggleChatConsole())
            {
                ToggleConsole();
            }

            if(GameManager.I.CurrentState != GameState.Typing)
            {
                if (InputManager.I.OpenChat())
                {
                    ToggleConsole();
                }

                if (InputManager.I.OpenDebugChat())
                {
                    showConsole = true;
                    GameManager.I.UpdateGameState(GameState.Typing);
                    chatConsole.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
                    inputField.text += "/";

                }
            }

            if (InputManager.I.Enter())
            {
                if(showConsole)
                { 
                    HandleInput();
                    input = "";
                }
            }
        }

        private void ToggleConsole()
        {
            showConsole = !showConsole;
            if (showConsole)
            {
                GameManager.I.UpdateGameState(GameState.Typing);
                chatConsole.SetActive(true);
                EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            }
            else
            {
                GameManager.I.UpdateGameState(GameManager.I.prevState);
                inputField.text = "";
                chatConsole.SetActive(false);
                EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            }
        }

        public void SendChat(string text, Color textColor)
        {
            if (chatLog.Count >= maxChatMessages)
            {
                DeleteChat(0);
            }

            ChatMessage chatMessage = new ChatMessage();
            chatMessage.text = text;

            GameObject newText = Instantiate(AssetManager.I.chatMessage, chatPanel.transform);
            chatMessage.textObject = newText.GetComponent<TMP_Text>();
            chatMessage.textObject.text = text;
            chatMessage.textObject.color = textColor;

            Popup.Create(chatPopPos, text, chatFadeTime, chatFadeSpd, textColor, false, true);
            chatLog.Add(chatMessage);
        }

        public void DeleteChat(int index)
        {
            Destroy(chatLog[index].textObject.gameObject);
            chatLog.RemoveAt(index);
        }

        private void HandleInput()
        {
            Color c = textColor;
            string[] properties = input.Split(" ");

            for (int i = 0; i < commands.commandList.Count; i++)
            {
                DebugCommandBase commandBase = commands.commandList[i] as DebugCommandBase;
                if (commandBase != null)
                {
                    if (input.Contains(commandBase.commandID))
                    {
                        if (commands.commandList[i] as DebugCommand != null)
                        {
                            (commands.commandList[i] as DebugCommand).Invoke();
                            c = commandColor;
                        }
                        else if (input != null && commands.commandList[i] as DebugCommand<int> != null && properties.Length > 1)
                        {
                            if (int.TryParse(properties[1], out int value))
                            {
                                (commands.commandList[i] as DebugCommand<int>).Invoke(value);
                                c = commandColor;
                            }
                        }
                    }
                }
            }

            string newChat = input;
            SendChat(newChat, c);
            inputField.text = "";
            ToggleConsole();
            EventSystem.current.SetSelectedGameObject(null, null);
        }

        

    }

    [System.Serializable]
    public class ChatMessage
    {
        public string text;
        public TMP_Text textObject;
    }

    public static class Console
    {
        public static void SendChat(string text)
        {
            ChatConsoleController.I.SendChat(text, ChatConsoleController.I.textColor);
        }

        public static void SendDebug(string text)
        {
            ChatConsoleController.I.SendChat("Console: " + text, ChatConsoleController.I.textColor);
            Debug.Log(text);
        }

        public static void SendError(string text)
        {
            ChatConsoleController.I.SendChat(text, ChatConsoleController.I.errorColor);
        }
    }

}
