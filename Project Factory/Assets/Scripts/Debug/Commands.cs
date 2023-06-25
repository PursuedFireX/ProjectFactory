using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class Commands : MonoBehaviour
    {
        public static Commands I { get; private set; }

        private ChatConsoleController chat;

        public static DebugCommand<int> ADDPOWER;
        public static DebugCommand<int> SETPOWER;
        public static DebugCommand HELP;
        public static DebugCommand CLEARCHAT;

        public List<object> commandList;

        private void Awake()
        {
            I = this;

            SETPOWER = new DebugCommand<int>("/SetPower", "Sets power to X.", "/SetPower X", (y) => SetPower(y));
            ADDPOWER = new DebugCommand<int>("/AddPower", "Adds X to power.", "/AddPower X", (x) => AddPower(x));
            HELP = new DebugCommand("/Help", "Shows a list of all commands and what they do.", "/Help", () => ListCommands());
            CLEARCHAT = new DebugCommand("/ClearChat", "Clears chat history.", "/ClearChat", () => ClearChat());

            commandList = new List<object>
            {
                SETPOWER,
                ADDPOWER,
                HELP,
                CLEARCHAT,
            };
        }

        private void Start()
        {
            chat = ChatConsoleController.I;
        }

        public void AddPower(int amount)
        {
            GameManager.I.currentPower += amount;
        }

        public void SetPower(int amount)
        {
            GameManager.I.currentPower = amount;
        }

        public void ListCommands()
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} - {command.commandDescription}";

                chat.SendChat(label, chat.commandColor);
            }
        }

        public void ClearChat()
        {
            for (int i = 0; i < chat.chatLog.Count; i++)
            {
                chat.DeleteChat(i);
            }
        }

    }
}
