using System;
using DialogueSystem;
using DialogueSystem.Actor;
using UnityEngine;

namespace JSON
{
    public class ActorLoader: ObjectLoader
    {
        public static Actor Load(String folder, String filename)
        {
            string j = GetJsonString(folder, filename);
            return JsonUtility.FromJson<Actor>(j);
        }
    }
}