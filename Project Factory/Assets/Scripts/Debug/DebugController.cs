using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace PFX
{
    public class DebugController : MonoBehaviour
    {
        private bool showConsole;
        private bool showOverlay;
        private bool showHelp;

        private Vector2 scroll;
        public int commandCount;

        public float consoleWidth;
        public float consoleHeight;

        public int textSize;
        public TMP_InputField inputField;
        public GameObject debugConsole;

        string input;

        public static DebugCommand<int> ADDPOWER;
        public static DebugCommand<int> SETPOWER;
        public static DebugCommand<int> TEST;
        public static DebugCommand HELP;
        public static DebugCommand DUMMYCOMMAND;

        public List<object> commandList;

        private void Awake()
        {
            DUMMYCOMMAND = new DebugCommand("Dummy", "Shows a list of all commands and what they do.", "Help", () => Debug.Log("Dummy"));
            SETPOWER = new DebugCommand<int>("SetPower", "Adds 100 power to grid.", "SetPower <power_amount>", (y) => Commands.I.SetPower(y));
            ADDPOWER = new DebugCommand<int>("AddPower", "Adds 100 power to grid.", "AddPower <power_amount>", (x) => Commands.I.AddPower(x));
            
            TEST = new DebugCommand<int>("Test", "Adds 100 power to grid.", "Test <power_amount>", (x) => Commands.I.AddPower(x));
            HELP = new DebugCommand("Help", "Shows a list of all commands and what they do.", "Help", () => showHelp = true);

            commandList = new List<object>
            {
                DUMMYCOMMAND,
                SETPOWER,
                ADDPOWER,
                TEST,
                HELP,
                
            };
        }


        private void Start()
        {
            consoleWidth = Screen.width / 2;
        }

        private void Update()
        {
            input = inputField.text;
            if (InputManager.I.ToggleDebugConsole())
            {
                ToggleConsole();
            }

            commandCount = commandList.Count;

            if (InputManager.I.ToggleDebugOverlay())
                showOverlay = !showOverlay;

            if (InputManager.I.Enter())
            {
                if(showConsole)
                {
                    
                    HandleInput();
                    input = "";
                }
            }
        }

        private void OnGUI()
        {
            /*if(!showConsole) { return; }

            float y = 0f;
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = textSize;

            //Help box
            if (showHelp)
            {
                GUI.Box(new Rect(0, y, consoleWidth, 100), "");
                Rect viewport = new Rect(0, 0, consoleWidth - 20, 20 * commandList.Count);
                scroll = GUI.BeginScrollView(new Rect(0, y + 5, consoleWidth, 90), scroll, viewport);

                for (int i = 0; i < commandList.Count; i++)
                {
                    DebugCommandBase command = commandList[i] as DebugCommandBase;
                    string label = $"{command.commandFormat} - {command.commandDescription}";
                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                    style.fontSize = 18;
                    GUI.Label(labelRect, label, style);
                }

                GUI.EndScrollView();
                y += 100;
            }



            //Text field
            style.fontSize = textSize;
            GUI.Box(new Rect(0, y, consoleWidth, consoleHeight), "");
            input = GUI.TextField(new Rect(10f, y + 5f, consoleWidth - 20f, 35), input, style);
            */
        }

        private void ToggleConsole()
        {
            showConsole = !showConsole;
            if (showConsole)
            {
                GameManager.I.UpdateGameState(GameState.Typing);
                debugConsole.SetActive(true);
            }
            else
            {
                GameManager.I.UpdateGameState(GameManager.I.prevState);
                debugConsole.SetActive(false);
            }
        }

        private void HandleInput()
        {
            string[] properties = input.Split(" ");
            Debug.Log(properties.Length);
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
                if(input.Contains(commandBase.commandID))
                {
                    if(commandList[i] as DebugCommand != null)
                    {
                        (commandList[i] as DebugCommand).Invoke();
                    }
                    else if (input != null && commandList[i] as DebugCommand<int> != null && properties.Length > 1)
                    {
                        if (int.TryParse(properties[1], out int value))
                        {
                            (commandList[i] as DebugCommand<int>).Invoke(value);
                        }
                    }
                }
            }
        }

    }
}
