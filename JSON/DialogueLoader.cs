using System;
using DialogueSystem;
using DialogueSystem.Dialogue;
using UnityEngine;

namespace JSON
{
    public class DialogueLoader: ObjectLoader
    {
        public static Dialogue Load(String folder, String filename)
        {
            string j = GetJsonString(folder, filename);
            Dialogue d = JsonUtility.FromJson<Dialogue>(j);
            // this is done because WEIRD UNITY SERIALISATION ISSUES
            // Basically: unity serialised stuff *doesn't* use *our* constructor
            // it's constructed using ??? magic ??? who knows.
            // but! yes. because of that, we don't actually do the lookup
            // as such, we need to do an *extra* bit before returning it.
            d.ReloadActor();
            return d;
        }
    }
}