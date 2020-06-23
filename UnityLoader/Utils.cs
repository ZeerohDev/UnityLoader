using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityLoader
{
    public static class Utils
    {
        public static Texture2D MakeTexture(int width, int height, NormalizedColor color)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = color.ColorToUnity();

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        public static Sprite SpriteFromTexture(string filePath, Vector2 pivot)
        {
            Sprite ret;
            Texture2D tex = new Texture2D(1, 1);
            byte[] buffer = File.ReadAllBytes(filePath);
            if (ImageConversion.LoadImage(tex, buffer))
            {
                ret = Sprite.Create(tex, new Rect(0f, 0f, (float)tex.width, (float)tex.height), pivot);
                return ret;
            }
            return null;
        }

        public static T GetComponent<T>()
        {
            foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
            {
                T comp = go.GetComponent<T>();
                if (comp != null)
                {
                    return comp;
                }
            }
            return default(T);
        }

        public static T GetComponent<T>(int index)
        {
            int i = 0;
            foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
            {
                T comp = go.GetComponent<T>();
                if (comp != null)
                {
                    if (i == index) return comp;
                    else i++;
                }
            }
            return default(T);
        }

        //Name + index

        public static T GetComponent<T>(string goName, bool caseSensitive = false)
        {
            foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
            {
                T comp = go.GetComponent<T>();
                if (comp != null)
                {
                    if (caseSensitive)
                        if (go.name.Contains(goName)) return comp;
                        else
                        if (go.name.ToLower().Contains(goName)) return comp;
                }
            }
            return default(T);
        }

        public static List<T> GetComponents<T>()
        {
            List<T> ret = new List<T>();
            foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
            {
                T comp = go.GetComponent<T>();
                if (comp != null)
                {
                    ret.Add(comp);
                }
            }
            return ret;
        }

        public static List<GameObject> GetGameObjects(string name, bool containing = false, bool caseSensitive = false)
        {
            List<GameObject> ret = new List<GameObject>();

            foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
            {
                if (containing)
                {
                    if (caseSensitive) { if (go.name.Contains(name)) { ret.Add(go); } }
                    else { if (go.name.ToLower().Contains(name.ToLower())) { ret.Add(go); } }
                }
                else
                {
                    if (caseSensitive) { if (go.name == name) { ret.Add(go); } }
                    else { if (go.name.ToLower() == name.ToLower()) { ret.Add(go); } }
                }
            }

            return ret;
        }
    }
}
