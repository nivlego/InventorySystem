using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace KeyStore
{
    public class KeyStoreHandler
    {
        private static KeyStoreHandler _default;
        // Variables
        private List<IKeyStore> _keyStores;
        // Methods
        public KeyStoreHandler()
        {
            _keyStores = new List<IKeyStore>();
        }

        public static KeyStoreHandler Default()
        {
            if (_default != null)
            {
                return _default;
            }
            KeyStoreHandler k = new KeyStoreHandler();
            k.AddKeyStore(new GenericKeyStore());
            k.AddKeyStore(new ActorKeyStore());
            k.AddKeyStore(new DialogueKeyStore());
            _default = k;
            return _default;
        }

        public dynamic Get(string keyPair)
        {
            // this is used when the person calling this is *lazy* and hasn't bothered to split anything
            Tuple<string, string> splitPair = SplitKeyStorePair(keyPair);
            return Get(splitPair.Item1, splitPair.Item2);
        }

        public dynamic Get(string keySource, string keyName)
        {
            // this looks like super weird but
            // we're just getting the keystores with a matching name
            // whacking them in a new subset
            // then returning the first one
            // if you got matching keys, that should be flagged up earlier during the creation process
            foreach (var keyStore in _keyStores.Where(keyStore => keyStore.GetKeyStoreName().Equals(keySource)))
            {
                return keyStore.GetKey(keyName);
            }

            Debug.LogWarning("no implemented way to get keys from source : " + keySource);
            return null;
        }

        public void Set(string keySource, string keyName, dynamic value)
        {
            foreach (var ks in _keyStores)
            {
                if (ks.GetKeyStoreName().Equals(keySource))
                {
                    ks.SetKey(keyName, value);
                    return;
                }
            }
            
            Debug.LogWarning("no implemented way to set keys into source : " + keySource);
            return;
        }

        public dynamic Set(string keyPair, dynamic value)
        {
            var (keySource, keyName) = SplitKeyStorePair(keyPair);
            return Set(keySource, keyName, value);
        }

        public void AddKeyStore(IKeyStore keyStore)
        {
            foreach (IKeyStore tempKs in _keyStores)
            {
                if (tempKs.GetKeyStoreName().Equals(keyStore.GetKeyStoreName()))
                {
                    throw new DuplicateNameException();
                }
            }
            _keyStores.Add(keyStore);
        }

        public static Tuple<string, string> SplitKeyStorePair(string pair)
        {
            string[] split = pair.Split(':');
            if (split.Length != 2)
            {
                throw new DataException("Key retrieval pair must be in format dataSource:key");
            }

            Tuple<string, string> t = Tuple.Create(split[0], split[1]);
            return t;
        }
    }
}