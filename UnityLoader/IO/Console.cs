using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityLoader
{
    public static class Console
    {
        internal static Dictionary<string, string> Commands { get; private set; } = new Dictionary<string, string>();
        internal static List<ICommandListener> Listeners { get; private set; } = new List<ICommandListener>();
        internal static string ConsoleInput { get; set; }

        public static void RegisterListener(ICommandListener listener, bool showRegistration = false)
        {
            if (string.IsNullOrWhiteSpace(listener.GetPrefix()) || string.IsNullOrEmpty(listener.GetPrefix()))
            {
                Debug.Log("Command listeners cannot use empty, null, or whitespace prefixes. Skipping registration.", LogType.Warning);
                return;
            }
            foreach (ICommandListener l in Listeners)
            {
                if (l.GetPrefix() == listener.GetPrefix())
                {
                    Debug.Log("A command listener with the prefix <i>" + listener.GetPrefix() + "</i> alredy exists. Skipping registration.", LogType.Warning);
                    return;
                }
            }
            Listeners.Add(listener);
            foreach (KeyValuePair<string, string> kvp in listener.GetCommands())
            {
                try
                {
                    Commands.Add(kvp.Key, kvp.Value);
                    if (showRegistration) Debug.Log("Command <i>" + kvp.Key + "</i> registered successfully.", LogType.Info);
                }
                catch (Exception e)
                {
                    Debug.Log("An exception occured while registering command <i>" + kvp.Key + "</i>: " + e.GetType().ToString() + " - " + e.Message, LogType.Severe);
                    continue;
                }
            }
        }
    }
}
