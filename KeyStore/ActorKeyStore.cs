using System;
using DialogueSystem.Actor;

namespace KeyStore
{
    public class ActorKeyStore: IKeyStore
    {

        public string GetKeyStoreName()
        {
            return "actor";
        }

        private static string[] SplitKey(string key)
        {
            string[] splitKey = key.Split('.');
            if (splitKey.Length != 2)
            {
                throw new KeyFormatException("actor key value must have two items");
            }
            return splitKey;
        }

        public dynamic GetKey(string key)
        {
            string[] splitKey = ActorKeyStore.SplitKey(key);
            Actor a = Actor.Load(splitKey[0]);
            return a.Memory().Get(splitKey[1]);
        }

        public void SetKey(string key, dynamic value)
        {
            string[] splitKey = ActorKeyStore.SplitKey(key);
            Actor a = Actor.Load(splitKey[0]);
            // splitKey[1] is the actual key IN an actors "memory"
            // the "value" is then the actual value we're passing to the keystore
            a.Memory().Set(splitKey[1], value);
        }
    }
}