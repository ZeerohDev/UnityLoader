using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityLoader
{
    public class CommandArg
    {
        private readonly string _value;

        public CommandArg(string value) => _value = value;
        public double ToDouble() => double.Parse(_value);
        public float ToFloat() => float.Parse(_value);
        public int ToInt() => int.Parse(_value);
        public T ToType<T>() => (T)Convert.ChangeType(_value, typeof(T));
        public override string ToString() => _value;

        internal static List<CommandArg> ConvertArray(string[] args)
        {
            List<CommandArg> ret = new List<CommandArg>();
            foreach (string arg in args) { ret.Add(new CommandArg(arg)); }

            return ret;
        }
    }
}
