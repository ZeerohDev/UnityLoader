using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static UnityLoader.Console;

namespace UnityLoader
{
    public class MainScript : MonoBehaviour
    {

        Texture2D consoleBackground,
                  inputBackground;

        int loaderIndex = 0;

        Rect consoleRect,
             menuRect;

        Vector2 consoleScroll = Vector2.zero;

        bool consoleOpen = false;

        KeyCode toggleKey = KeyCode.F2,
                enterKey = KeyCode.Return;

        void Start()
        {
            consoleRect = new Rect(0, 0, Screen.width, Screen.height * .35f);
            /*menuRect = new Rect(Screen.width - 300, Screen.height - 400, 300, 400);*/
            consoleBackground = Utils.MakeTexture(1, 1, new NormalizedColor(25, 25, 25, 250));
            inputBackground = Utils.MakeTexture(1, 1, new NormalizedColor(25, 25, 25, 200));
            Debug.Log("Welcome to TweakLoader! Use <i>help</i> for a list of available commands.");
            RegisterListener(new CoreListener());
            RegisterListener(new DebugListener(), true);
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                consoleOpen = !consoleOpen;
            }
            else
            {
                if (Event.current.isKey)
                {
                    if (Event.current.keyCode == enterKey)
                    {
                        if (!string.IsNullOrEmpty(ConsoleInput) && consoleOpen) { ProcessCommands(); }
                    }
                    if (Event.current.type == EventType.KeyDown)
                    {
                        if (Event.current.keyCode == toggleKey && consoleOpen)
                        {
                            GUIUtility.keyboardControl = 0;
                            consoleOpen = false;
                        }
                    }
                }
            }
        }

        void OnGUI()
        {
            if (consoleOpen)
            {
                if (Event.current.keyCode == toggleKey) { GUIUtility.keyboardControl = 0; }
                ApplySkins();
                consoleRect = GUIUtility.AlignRectToDevice(consoleRect);
                consoleRect = GUI.Window(0, consoleRect, OnWindow, "");
            }
        }

        void OnWindow(int id)
        {
            consoleScroll = GUILayout.BeginScrollView(consoleScroll);
            foreach (string message in Debug.GetLog())
            {
                GUILayout.Label(message);
                GUILayout.Space(1f);
            }
            GUILayout.EndScrollView();
            ConsoleInput = GUILayout.TextField(ConsoleInput);
        }

        void ProcessCommands()
        {
            try
            {
                Debug.Log(">" + ConsoleInput);
                bool commandExists = false;
                string[] cmd = ConsoleInput.Split(' '), args;
                int argCount;
                string truePrefix = string.Empty;
                bool symbolPrefix;
                foreach (ICommandListener listener in Listeners)
                {
                    truePrefix = listener.GetPrefix().Trim();
                    if (truePrefix.StartsWith("@symbol:"))
                    {
                        if (truePrefix.Length > 8)
                            truePrefix = truePrefix.Substring(8, truePrefix.Length - 8);
                        else truePrefix = "";
                        symbolPrefix = true;
                    }
                    else { truePrefix = listener.GetPrefix(); symbolPrefix = false; }
                    argCount = (cmd.Length - 2) + Convert.ToInt32(symbolPrefix);
                    args = argCount > 0 ? new string[argCount] : new string[0];
                    if (argCount > 0)
                        for (int i = 2 - Convert.ToInt32(symbolPrefix); i < cmd.Length; i++) { args[i - (2 - Convert.ToInt32(symbolPrefix))] = cmd[i]; }
                    if (cmd[0].StartsWith(truePrefix))
                    {
                        if (listener.ProcessCommand(symbolPrefix ? cmd[0].Substring(truePrefix.Length, cmd[0].Length - truePrefix.Length) : 
                            cmd[1], CommandArg.ConvertArray(args))) { commandExists = true; break; }
                    }
                }
                if (!commandExists) { Debug.Log("Command not recognized.", LogType.Warning); }
            }
            catch (Exception e)
            {
                Debug.Log("Exception caught: " + e.ToString() + " - " + e.Message, LogType.Severe);
            }
            ConsoleInput = "";
            consoleScroll = new Vector2(0, float.MaxValue);
        }

        void ApplySkins()
        {
            GUI.skin.label.richText = true;
            GUI.skin.window.onNormal.background = consoleBackground;
            GUI.skin.window.onNormal.textColor = Color.white;
            GUI.skin.window.onFocused.background = consoleBackground;
            GUI.skin.window.onFocused.textColor = Color.white;
            GUI.skin.window.onActive.background = consoleBackground;
            GUI.skin.window.onActive.textColor = Color.white;
            GUI.skin.window.onHover.background = consoleBackground;
            GUI.skin.window.onHover.textColor = Color.white;
            GUI.skin.window.focused.background = consoleBackground;
            GUI.skin.window.focused.textColor = Color.white;
            GUI.skin.window.hover.background = consoleBackground;
            GUI.skin.window.hover.textColor = Color.white;
            GUI.skin.window.active.background = consoleBackground;
            GUI.skin.window.active.textColor = Color.white;
            GUI.skin.window.normal.background = consoleBackground;
            GUI.skin.window.normal.textColor = Color.white;
            GUI.skin.textField.normal.background = inputBackground;
            GUI.skin.textField.onNormal.background = inputBackground;
            GUI.skin.textField.focused.background = inputBackground;
            GUI.skin.textField.onFocused.background = inputBackground;
            GUI.skin.textField.hover.background = inputBackground;
            GUI.skin.textField.onHover.background = inputBackground;
            GUI.skin.textField.active.background = inputBackground;
            GUI.skin.textField.onActive.background = inputBackground;
        }
    }
}
