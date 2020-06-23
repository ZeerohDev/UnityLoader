using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityLoader
{
    public class Loader
    {
        private static GameObject go;

        public static void LoadTweak()
        {
            if (go != null) return;
            go = new GameObject();
            go.AddComponent<MainScript>();
            GameObject.DontDestroyOnLoad(go);
            go.SetActive(true);
        }

        public static void UnloadTweak() { GameObject.DestroyImmediate(go); }
    }
}
