using System;
using System.IO;
using System.Reflection;
using DialogueSystem;
using UnityEngine;

namespace JSON
{
    public class ObjectLoader
    {
        
        protected static string GetFsPath(string folder, string filename) // made a change :  this has been changed from protected to public
        {
            // create folders if not exists
            Directory.CreateDirectory(Application.dataPath + "/Data");
            Directory.CreateDirectory(Application.dataPath + "/Data/" + folder);
            return Application.dataPath + "/Data/" + folder + "/" + filename + ".json";
        }
        protected static string GetJsonString(string folder, string filename) // made a change: this has been changed from protected to public
        {
            string path = GetFsPath(folder, filename);
            string jsonString = "";
            try
            {
                StreamReader reader = new StreamReader(path);
                jsonString = "";
                while (!reader.EndOfStream)
                {
                    jsonString = jsonString + reader.ReadLine();
                }

                Debug.Log("[JSON] read: " + GetFsPath(folder, filename));
                reader.Close();
            }
            catch (FileNotFoundException e)
            {
                Debug.LogWarning("[JSON] could not find file at " + path + " - returning null");
            }
            return jsonString;
        }
        
        
        public static void Save(dynamic data, String folder, String filename)
        {
            string path = GetFsPath(folder, filename);
            string content = JsonUtility.ToJson(data, true);
            StreamWriter outputFile = new StreamWriter(path);
            outputFile.Write(content);
            outputFile.Close();
            Debug.Log("[JSON] write: " + path);
        }
    }
}