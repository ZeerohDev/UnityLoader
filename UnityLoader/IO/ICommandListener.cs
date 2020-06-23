using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityLoader
{
    public interface ICommandListener
    {
        string GetPrefix();
        Dictionary<string, string> GetCommands();
        bool ProcessCommand(string command, List<CommandArg> args);
    }
}
