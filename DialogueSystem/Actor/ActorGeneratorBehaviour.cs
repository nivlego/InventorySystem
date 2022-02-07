using System.Collections.Generic;
using KeyStore;
using UnityEngine;

namespace DialogueSystem.Actor
{
    public class ActorGeneratorBehaviour: MonoBehaviour
    {
        public List<Actor> actors;
        public KeyStoreDataStorageSystem memory;

        public void Start()
        {
            if (actors == null)
            {
                actors = new List<Actor>();
            }

            foreach (Actor a in actors)
            {
                a.Save();
            }
        }
    }
}