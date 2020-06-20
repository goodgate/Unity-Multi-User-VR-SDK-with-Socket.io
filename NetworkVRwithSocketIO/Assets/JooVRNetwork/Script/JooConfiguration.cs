using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;

namespace JYJ_Utils
{

    public static class JooConfiguration
    {
        private static Dictionary<Type, object> dict = new Dictionary<Type, object>();
        public static T GetConfig<T>() where T : class, new()
        {
            object obj = null;
            if(dict.TryGetValue(typeof(T), out obj))
            {
                return (T)obj;
            }
            else
            {
                obj = LoadFromJson<T>();
                if(obj == null)
                {
                    obj = new T();
                    SaveToJson<T>((T)obj);
                   
                }
                dict.Add(typeof(T), obj);
                return (T)obj;
            }
        }

        public const string kDirectory = "./JooConfigs";

        private static T LoadFromJson<T>() where T : class, new()
        {
            if (!Directory.Exists(kDirectory))
            {
                Directory.CreateDirectory(kDirectory);
            }
            var path = Path.Combine(kDirectory, typeof(T).ToString() + ".json");
            if (!File.Exists(path))
            {
                return null;
            }
            using (StreamReader sr = new StreamReader(path))
            {
                string jsonString = sr.ReadToEnd();
                sr.Close();
                T obj = JsonConvert.DeserializeObject<T>(jsonString);
                return obj;
            }
        }
        public static void SaveToJson<T>(T obj) where T : class, new()
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(kDirectory, typeof(T).ToString() + ".json")))
            {
                string jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
                sw.Write(jsonString);
                sw.Close();
            }
        }
    }
}
