using System;
using KeyStore;
using UnityEngine;

namespace DialogueSystem.Actor
{
    [Serializable]
    public class Actor
    {
        public string filename;
        public string name;
        public GameObject prefab;
        public KeyStoreDataStorageSystem memory;
        public Actor(string name)
        {
            this.filename = name;
            this.name = name;
            this.memory = new KeyStoreDataStorageSystem();
        }

        public Actor(string filename, string name)
        {
            this.filename = filename;
            this.name = name;
            this.memory = new KeyStoreDataStorageSystem();
        }

        public string GetName()
        {
            return name;
        }

        public void Save()
        {
            JSON.ActorLoader.Save(this, "Actor", name);
        }

        private void ReloadFromDisk()
        {
            Actor a = Actor.Load(filename);
            this.filename = a.filename;
            this.name = a.name;
            this.memory = a.memory;
        }

        public static Actor Load(string actor)
        {
            return JSON.ActorLoader.Load("Actor", actor);
        }

        public KeyStoreDataStorageSystem Memory()
        {
            ReloadFromDisk();
            return this.memory;
        }
    }
}