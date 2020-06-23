using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityLoader.Utils;

namespace UnityLoader
{
    internal class DebugListener : ICommandListener
    {
        private List<GameObject> searchResults = new List<GameObject>();
        private GameObject selectedObject;

        private readonly Dictionary<string, string> commands = new Dictionary<string, string>()
        {
            { "timescale", "Sets the timescale for the game. <i>Usage: dbg timescale [value|reset]</i>" },
            { "search", "Search for GameObjects in the current scene by name. <i>usage: dbg search [value]</i>" },
            { "select", "Select an object based on its index in the search query to perform more operations. <i>usage: dbg select [name] [value]</i>" },
            { "info", "Display different info about the currently selected GameObject. <i>usage: dbg info []</i>" },
            { "reset", "Resets any selected objects and the current search query." }
        };

        public string GetPrefix() { return "dbg"; }
        public Dictionary<string, string> GetCommands() { return commands; }

        public bool ProcessCommand(string command, List<CommandArg> args)
        {
            switch (command.ToLower())
            {
                case "timescale":
                    switch (args[0].ToString())
                    {
                        case "reset":
                            Time.timeScale = 0;
                            Debug.Log("Timescale reset to 1.", "DEBUG");
                            break;
                        default:
                            Time.timeScale = args[0].ToFloat();
                            Debug.Log("Timescale set to " + args[0].ToString() + ".", "DEBUG");
                            break;
                    }
                    break;
                case "reset":
                    searchResults.Clear();
                    selectedObject = null;
                    Debug.Log("Current search query and any selected objects have been reset.", LogType.Info);
                    break;
                case "search":
                    searchResults.Clear();
                    searchResults = Utils.GetGameObjects(args[0].ToString(), true);
                    Debug.Log("Found " + searchResults.Count + " results:", LogType.Info);
                    for (int i = 0; i < searchResults.Count; i++)
                    {
                        Debug.Log("Name: " + searchResults[i].name, (i + 1) + ".");
                    }
                    break;
                case "select":
                    if (searchResults.Count < 1)
                    {
                        Debug.Log("There must be a populated search query in order to select an object from it.", LogType.Warning);
                        break;
                    }
                    else
                    {
                        int index = args[0].ToInt() - 1;
                        if (index >= searchResults.Count || index < 0)
                        {
                            Debug.Log("Selected index from query cannot exceed or go below the query count.", LogType.Severe);
                            break;
                        }
                        else
                        {
                            selectedObject = searchResults[index];
                            Debug.Log("Selected GameObject with name " + searchResults[index].name + ".", LogType.Info);
                        }
                    }
                    break;
                case "info":
                    switch (args[0].ToString())
                    {
                        case "all":
                            break;
                        case "id":
                            Debug.Log("InstanceID: " + selectedObject.GetInstanceID());
                            break;
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}
