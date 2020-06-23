using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static UnityLoader.Utils;

namespace UnityLoader
{
    internal class CoreListener : ICommandListener
    {
        private readonly Dictionary<string, string> commands = new Dictionary<string, string>()
        {
            { "help", "Displays a list of available commands" },
            { "clear", "Clears the current console output." },
            { "save", "Saves the current console output to a time-stamped text document in the game's root folder." },
            { "version", "Displays the version of this TweakLoader DLL." },
            { "display", "Displays samples of each formal log type for developer reference." },
        };

        public string GetPrefix() { return "@symbol:"; }
        public string GetName() { return "Core"; }
        public Dictionary<string, string> GetCommands() { return commands; }

        public bool ProcessCommand(string command, List<CommandArg>args)
        {
            switch (command.ToLower())
            {
                case "clear":
                    Clear();
                    Debug.Log("Welcome to TweakLoader! Use <i>help</i> for a list of available commands.");
                    break;
                case "display":
                    Display();
                    break;
                case "save":
                    Debug.DumpLog();
                    break;
                case "help":
                    if (args.Count < 1)
                    {
                        Help(this);
                        break;
                    }
                    else
                    {
                        bool found = false;
                        string truePrefix = String.Empty;
                        foreach (ICommandListener l in Console.Listeners)
                        {
                            truePrefix = l.GetPrefix().Trim();
                            if (truePrefix.StartsWith("@symbol:"))
                            {
                                if (truePrefix.Length > 8)
                                    truePrefix = truePrefix.Substring(8, truePrefix.Length - 8);
                                else truePrefix = "";
                            }
                            else { truePrefix = l.GetPrefix().Trim(); }
                            if (truePrefix == args[0].ToString())
                            {
                                Help(l);
                                found = true;
                                break;
                            }
                        }
                        if (!found) Debug.Log("Cannot find a command listener with the prefix <i>" + args[0] + "</i>.", LogType.Severe);
                    }
                    break;
                case "version":
                    Debug.Log("TweakLoader - Version <b>" + Version.Get() + "</b>");
                    break;
                default:
                    return false;
            }
            return true;
        }

        private void Clear()
        {
            Debug.ClearLog();
        }

        private void Display()
        {
            Debug.Log("This is a normal message with a prefix.", "PREFIX", false);
            Debug.Log("This is a normal message without a prefix.");
            Debug.Log("This is an info message without a timestamp.", LogType.Info);
            Debug.Log("This is an info message with a timestamp.", LogType.Info, true);
            Debug.Log("This is a warning message without a timestamp.", LogType.Warning);
            Debug.Log("This is a warning message with a timestamp.", LogType.Warning, true);
            Debug.Log("This is an error message without a timestamp.", LogType.Severe);
            Debug.Log("This is an error message with a timestamp.", LogType.Severe, true);
        }

        private void Help(ICommandListener listener)
        {
            Debug.Log(listener.GetName() + " Commands", LogType.Info);
            foreach (KeyValuePair<string, string> cmd in listener.GetCommands())
            {
                Debug.Log("<i>" + cmd.Key + "</i>" + " - " + cmd.Value);
            }
        }
    }
}
