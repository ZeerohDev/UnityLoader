using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UnityLoader
{
    //Rich Text for Unity reference: https://docs.unity3d.com/Manual/StyledText.html

    public enum LogType : int
    {
        Info = 1,
        Warning = 2,
        Severe = 3,
    }

    public static class Debug
    {
        private static CommandHistory immediateLog = new CommandHistory();

        public static List<string> GetLog() { return immediateLog.Values.ToList<string>(); }

        public static void Log(string message, LogType severity, bool showTimeStamp = false, bool saveToFile = false)
        {
            string output = String.Empty;
            switch (severity)
            {
                case LogType.Info:
                    output = "<color=#19A6D2><b>[INFO" + (showTimeStamp ?  " - " + DateTime.Now.ToShortTimeString().ToString() : "") + "]</b> " + message + "</color>";
                    break;
                case LogType.Warning:
                    output = "<color=#DEB112><b>[WARN" + (showTimeStamp ? " - " + DateTime.Now.ToShortTimeString().ToString() : "") + "]</b> " + message + "</color>";
                    break;
                case LogType.Severe:
                    output = "<color=red><b>[ERROR" + (showTimeStamp ? " - " + DateTime.Now.ToShortTimeString().ToString() : "") + "]</b> " + message + "</color>";
                    break;
            }
            immediateLog.Add(message, output);
            if (saveToFile)
            {
                using (StreamWriter writer = new StreamWriter("Log.txt", true))
                {
                    writer.WriteLine(message);
                }
            }
        }

        public static void Log(string message, string prefix = "", bool saveToFile = false)
        {
            immediateLog.Add(message, (prefix != "" ? "<b>[" + prefix + "]</b> " : "" ) + message);
            if (saveToFile)
            {
                using (StreamWriter writer = new StreamWriter("Log.txt", true))
                {
                    writer.WriteLine(message);
                }
            }
        }

        public static void DumpLog()
        {
            string timeDateFile = "Log " + DateTime.Now.ToString("[MM-dd-yyyy]-hh_mm_tt") + ".txt";
            using (StreamWriter writer = new StreamWriter(timeDateFile))
            {
                foreach (string message in immediateLog.Keys)
                {
                    writer.WriteLine(message);
                }
            }
            Log("Log successfully dumped to " + Environment.CurrentDirectory.Replace('\\', '/') + "/" + timeDateFile + "!", LogType.Info);
        }

        public static void ClearLog()
        {
            immediateLog.Clear();
        }

        private class CommandHistory
        {
            public List<string> Keys { get; set; } = new List<string>();
            public List<string> Values { get; set; } = new List<string>();

            public void Add(string key, string value)
            {
                Keys.Add(key);
                Values.Add(value);
            }

            public void Clear()
            {
                Keys.Clear();
                Values.Clear();
            }
        }
    }
}
