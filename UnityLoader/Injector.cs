using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace UnityLoader
{
    public static class Injector
    {
        private static readonly string assemblyLocation = "Tweaks/";
        public static Assembly LoadedAssemblies { get; private set; }

        public static void LoadAssemblies()
        {
            foreach (string file in Directory.GetFiles(assemblyLocation))
            {
                if (file.ToLower().EndsWith(".dll"))
                {
                    Assembly loadedAssembly = Assembly.LoadFrom(file);
                    bool loadedTweak = false;
                    foreach (Module module in loadedAssembly.Modules)
                    {
                        foreach (MethodInfo method in module.GetMethods())
                        {
                            if (method.Name == "LoadTweak")
                            {
                                method.Invoke(null, null);
                                loadedTweak = true;
                                break;
                            }
                        }
                        if (loadedTweak) { }
                    }
                }
            }
        }
    }
}
